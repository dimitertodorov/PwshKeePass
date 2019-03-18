// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading;
using PwshKeePass.Common.Utilities;

namespace PwshKeePass.Common.Extensions
{
    public static class CmdletExtensions
    {
        /// <summary>
        /// Execute this cmdlet in the background and return a job that tracks the results
        /// </summary>
        /// <typeparam name="T">The cmdlet type</typeparam>
        /// <param name="cmdlet">The cmdlet to execute</param>
        /// <param name="jobName">The name of the job</param>
        /// <returns>The job tracking cmdlet execution</returns>
        public static Job ExecuteAsJob<T>(this T cmdlet, string jobName) where T : KeePassCmdlet
        {
            if (cmdlet == null)
            {
                throw new ArgumentNullException(nameof(cmdlet));
            }

            return ExecuteAsJob(cmdlet, jobName, cmd => cmd.ExecuteCmdlet());
        }

        /// <summary>
        /// Execute this cmdlet in the background and return a job that tracks the results
        /// </summary>
        /// <typeparam name="T">The cmdlet type</typeparam>
        /// <param name="cmdlet">The cmdlet to execute</param>
        /// <param name="jobName">The name of the job</param>
        /// <param name="executor">The method to execute in the background job</param>
        /// <returns>The job tracking cmdlet execution</returns>
        public static Job ExecuteAsJob<T>(this T cmdlet, string jobName, Action<T> executor) where T : KeePassCmdlet
        {
            if (cmdlet == null)
            {
                throw new ArgumentNullException(nameof(cmdlet));
            }

            if (executor == null)
            {
                throw new ArgumentNullException(nameof(executor));
            }

            var job = LongRunningJob<T>.Create(cmdlet, cmdlet?.MyInvocation?.MyCommand?.Name, jobName, executor);
            cmdlet.SafeAddToJobRepository(job);
            ThreadPool.QueueUserWorkItem(job.RunJob, job);
            return job;
        }

        /// <summary>
        /// Determine if AsJob is present
        /// </summary>
        /// <typeparam name="T">The cmdlet type</typeparam>
        /// <param name="cmdlet">The cmdlet</param>
        /// <returns>True if the cmdlet shoudl run as a Job, otherwise false</returns>
        public static bool AsJobPresent<T>(this T cmdlet) where T : KeePassCmdlet
        {
            if (cmdlet == null)
            {
                throw new ArgumentNullException(nameof(cmdlet));
            }

            return (cmdlet.MyInvocation?.BoundParameters != null
                    && cmdlet.MyInvocation.BoundParameters.ContainsKey("AsJob"));
        }

        /// <summary>
        /// Execute the given cmdlet synchronously os as a job, based on input parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdlet"></param>
        public static void ExecuteSynchronouslyOrAsJob<T>(this T cmdlet) where T : KeePassCmdlet
        {
            if (cmdlet == null)
            {
                throw new ArgumentNullException(nameof(cmdlet));
            }

            cmdlet.ExecuteSynchronouslyOrAsJob(c => c.ExecuteCmdlet());
        }

        /// <summary>
        /// Decide whether to execute this cmdlet as a job or synchronously, based on input parameters
        /// </summary>
        /// <typeparam name="T">The cmdlet type</typeparam>
        /// <param name="cmdlet">The cmdlet to execute</param>
        /// <param name="executor">The cmdlet method to execute</param>
        public static void ExecuteSynchronouslyOrAsJob<T>(this T cmdlet, Action<T> executor) where T : KeePassCmdlet
        {
            if (cmdlet == null)
            {
                throw new ArgumentNullException(nameof(cmdlet));
            }

            if (executor == null)
            {
                throw new ArgumentNullException(nameof(executor));
            }

            if (cmdlet.AsJobPresent())
            {
                cmdlet.WriteObject(cmdlet.ExecuteAsJob(cmdlet.ImplementationBackgroundJobDescription, executor));
            }
            else
            {
                executor(cmdlet);
            }
        }

        /// <summary>
        /// Safely Attempt to copy a property value from source to target
        /// </summary>
        /// <typeparam name="T">The type fo the source and target objects</typeparam>
        /// <param name="property">The property to copy</param>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The target object to copy to</param>
        public static void SafeCopyValue<T>(this PropertyInfo property, T source, T target)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            try
            {
                property.SetValue(target, property.GetValue(source));
            }
            catch
            {
                // ignore get and set errors
            }
        }

        /// <summary>
        /// Safely Attempt to copy a field value from source to target
        /// </summary>
        /// <typeparam name="T">The type of the source and target objects</typeparam>
        /// <param name="field">The field to copy</param>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The target object to copy to</param>
        public static void SafeCopyValue<T>(this FieldInfo field, T source, T target)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            try
            {
                field.SetValue(target, field.GetValue(source));
            }
            catch
            {
                // ignore get and set errors
            }
        }

        /// <summary>
        /// Safely copy the selected parameter set from one cmdlet to another
        /// </summary>
        /// <typeparam name="T">The cmdlet type</typeparam>
        /// <param name="source">The cmdlet to copy the parameter set name from</param>
        /// <param name="target">The cmdlet to copy to</param>
        public static void SafeCopyParameterSet<T>(this T source, T target) where T : KeePassCmdlet
        {
            if (source != null && target != null)
            {
                if (!string.IsNullOrWhiteSpace(source.ParameterSetName))
                {
                    try
                    {
                        target.SetParameterSet(source.ParameterSetName);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Return the value of a paramater, or null if not set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmdlet">the executing cmdlet</param>
        /// <param name="parameterName">The name of the parameter to return</param>
        /// <returns>true if the parameter was provided by the user, otherwise false</returns>
        public static bool IsBound(this PSCmdlet cmdlet, string parameterName)
        {
            return cmdlet.MyInvocation.BoundParameters.ContainsKey(parameterName);
        }

        public static string AsAbsoluteLocation(this string realtivePath)
        {
            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, realtivePath));
        }

        public static string TryResolvePath(this PSCmdlet psCmdlet, string path)
        {
            try
            {
                return psCmdlet.ResolvePath(path);
            }
            catch
            {
                return path;
            }
        }

        public static string ResolvePath(this PSCmdlet psCmdlet, string path)
        {
            if (path == null)
            {
                return null;
            }

            if (psCmdlet.SessionState == null)
            {
                return path;
            }

            path = path.Trim('"', '\'', ' ');
            var result = psCmdlet.SessionState.Path.GetResolvedPSPathFromPSPath(path);
            string fullPath = string.Empty;

            if (result != null && result.Count > 0)
            {
                fullPath = result[0].ProviderPath;
            }

            return fullPath;
        }

        public static bool IsParameterBound<TPSCmdlet, TProp>(this TPSCmdlet cmdlet,
            Expression<Func<TPSCmdlet, TProp>> propertySelector) where TPSCmdlet : PSCmdlet
        {
            var propName = ((MemberExpression) propertySelector.Body).Member.Name;
            return cmdlet.MyInvocation.BoundParameters.ContainsKey(propName);
        }

        #region PowerShell Commands

        public static void InvokeBeginProcessing(this PSCmdlet cmdlt)
        {
            MethodInfo dynMethod =
                (typeof(PSCmdlet)).GetMethod("BeginProcessing", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(cmdlt, null);
        }

        public static void SetParameterSet(this PSCmdlet cmdlt, string value)
        {
            FieldInfo dynField =
                (typeof(Cmdlet)).GetField("_parameterSetName", BindingFlags.NonPublic | BindingFlags.Instance);
            dynField.SetValue(cmdlt, value);
        }

        public static void SetBoundParameters(this PSCmdlet cmdlt, IDictionary<string, object> parameters)
        {
            foreach (var pair in parameters)
            {
                cmdlt.MyInvocation.BoundParameters.Add(pair.Key, pair.Value);
            }
        }

        public static void InvokeEndProcessing(this PSCmdlet cmdlt)
        {
            MethodInfo dynMethod =
                (typeof(PSCmdlet)).GetMethod("EndProcessing", BindingFlags.NonPublic | BindingFlags.Instance);
            dynMethod.Invoke(cmdlt, null);
        }

        #endregion


        static void SafeAddToJobRepository(this KeePassCmdlet cmdlet, Job job)
        {
            try
            {
                cmdlet.JobRepository.Add(job);
            }
            catch
            {
                // ignore errors in adding the job to the repository
            }
        }
    }
}