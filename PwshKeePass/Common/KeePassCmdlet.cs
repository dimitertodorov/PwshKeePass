using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Management.Automation;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Common.Utilities;
using PwshKeePass.Profile;
using PwshKeePass.Service;

namespace PwshKeePass.Common
{
    [CmdletBinding()]
    public abstract class KeePassCmdlet : PSCmdlet, IDisposable
    {
        public KeePassProfile KeePassProfile;
        public DatabaseProfileService DatabaseProfileService;

        private const string PSVERSION = "PSVersion";
        private const string DEFAULT_PSVERSION = "3.0.0.0";

        public ConcurrentQueue<string> DebugMessages { get; private set; }

        protected static string _errorRecordFolderPath = null;
        protected static string _sessionId = Guid.NewGuid().ToString();
        protected const string _fileTimeStampSuffixFormat = "yyyy-MM-dd-THH-mm-ss-fff";
        protected string _clientRequestId = Guid.NewGuid().ToString();

        /// <summary>
        /// Indicates installed PowerShell version
        /// </summary>
        private string _psVersion;

        /// <summary>
        /// Get PsVersion returned from PowerShell.Runspace instance
        /// </summary>
        protected string PSVersion
        {
            get
            {
                if (string.IsNullOrEmpty(_psVersion))
                {
                    if (this.Host != null)
                    {
                        _psVersion = this.Host.Version.ToString();
                    }
                    else
                    {
                        //We are doing this for perf. reasons. This code will execute during tests and so reducing the perf. overhead while running tests.
                        _psVersion = DEFAULT_PSVERSION;
                    }
                }

                return _psVersion;
            }
        }

        /// <summary>
        /// Gets the PowerShell module name used for user agent header.
        /// By default uses "PwshKeePass"
        /// </summary>
        protected virtual string ModuleName
        {
            get { return "PwshKeePass"; }
        }

        /// <summary>
        /// Gets PowerShell module version used for user agent header.
        /// </summary>
        protected string ModuleVersion
        {
            get { return "TODO"; }
        }

        private SessionState _sessionState;

        public new SessionState SessionState
        {
            get { return _sessionState; }
            set { _sessionState = value; }
        }

        private RuntimeDefinedParameterDictionary _asJobDynamicParameters;

        public RuntimeDefinedParameterDictionary AsJobDynamicParameters
        {
            get
            {
                if (_asJobDynamicParameters == null)
                {
                    _asJobDynamicParameters = new RuntimeDefinedParameterDictionary();
                }

                return _asJobDynamicParameters;
            }
            set { _asJobDynamicParameters = value; }
        }

        /// <summary>
        /// Initializes AzurePSCmdlet properties.
        /// </summary>
        public KeePassCmdlet()
        {
            DebugMessages = new ConcurrentQueue<string>();
        }


        protected virtual void LogCmdletStartInvocationInfo()
        {
            if (string.IsNullOrEmpty(ParameterSetName))
            {
                WriteDebugWithTimestamp($"{this.GetType().Name} begin processing " + "without ParameterSet.");
            }
            else
            {
                WriteDebugWithTimestamp($"{this.GetType().Name} begin processing " +
                                        $"with ParameterSet '{ParameterSetName}'.");
            }
        }

        protected virtual void LogCmdletEndInvocationInfo()
        {
            string message = $"{this.GetType().Name} end processing.";
            WriteDebugWithTimestamp(message);
        }


        protected bool IsVerbose()
        {
            bool verbose = MyInvocation.BoundParameters.ContainsKey("Verbose")
                           && ((SwitchParameter) MyInvocation.BoundParameters["Verbose"]).ToBool();
            return verbose;
        }

        protected new void WriteError(ErrorRecord errorRecord)
        {
            base.WriteError(errorRecord);
        }

        protected new void ThrowTerminatingError(ErrorRecord errorRecord)
        {
            FlushDebugMessages();
            base.ThrowTerminatingError(errorRecord);
        }

        protected new void WriteObject(object sendToPipeline)
        {
            FlushDebugMessages();
            base.WriteObject(sendToPipeline);
        }

        protected new void WriteObject(object sendToPipeline, bool enumerateCollection)
        {
            FlushDebugMessages();
            base.WriteObject(sendToPipeline, enumerateCollection);
        }

        protected new void WriteVerbose(string text)
        {
            FlushDebugMessages();
            base.WriteVerbose(text);
        }

        protected new void WriteWarning(string text)
        {
            FlushDebugMessages();
            base.WriteWarning(text);
        }

        protected new void WriteCommandDetail(string text)
        {
            FlushDebugMessages();
            base.WriteCommandDetail(text);
        }

        protected new void WriteProgress(ProgressRecord progressRecord)
        {
            FlushDebugMessages();
            base.WriteProgress(progressRecord);
        }

        protected new void WriteDebug(string text)
        {
            FlushDebugMessages();
            base.WriteDebug(text);
        }

        protected void WriteVerboseWithTimestamp(string message, params object[] args)
        {
            if (CommandRuntime != null)
            {
                WriteVerbose(string.Format("{0:T} - {1}", DateTime.Now, string.Format(message, args)));
            }
        }

        protected void WriteVerboseWithTimestamp(string message)
        {
            if (CommandRuntime != null)
            {
                WriteVerbose(string.Format("{0:T} - {1}", DateTime.Now, message));
            }
        }

        protected void WriteWarningWithTimestamp(string message)
        {
            if (CommandRuntime != null)
            {
                WriteWarning(string.Format("{0:T} - {1}", DateTime.Now, message));
            }
        }

        protected void WriteDebugWithTimestamp(string message, params object[] args)
        {
            if (CommandRuntime != null)
            {
                WriteDebug(string.Format("{0:T} - {1}", DateTime.Now, string.Format(message, args)));
            }
        }

        protected void WriteDebugWithTimestamp(string message)
        {
            if (CommandRuntime != null)
            {
                WriteDebug(string.Format("{0:T} - {1}", DateTime.Now, message));
            }
        }

        protected void WriteErrorWithTimestamp(string message)
        {
            if (CommandRuntime != null)
            {
                WriteError(
                    new ErrorRecord(new Exception(string.Format("{0:T} - {1}", DateTime.Now, message)),
                        string.Empty,
                        ErrorCategory.NotSpecified,
                        null));
            }
        }

        /// <summary>
        /// Write an error message for a given exception.
        /// </summary>
        /// <param name="ex">The exception resulting from the error.</param>
        protected virtual void WriteExceptionError(Exception ex)
        {
            Debug.Assert(ex != null, "ex cannot be null or empty.");
            WriteError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));
        }

        protected PSObject ConstructPSObject(string typeName, params object[] args)
        {
            return PowerShellUtilities.ConstructPSObject(typeName, args);
        }

        protected void SafeWriteOutputPSObject(string typeName, params object[] args)
        {
            PSObject customObject = this.ConstructPSObject(typeName, args);
            WriteObject(customObject);
        }

        private void FlushDebugMessages(bool record = false)
        {
            string message;
            while (DebugMessages.TryDequeue(out message))
            {
                base.WriteDebug(message);
            }
        }


        //CUSTOM LOGIC HERE

        public virtual void ExecuteCmdlet()
        {
            // Do nothing.
        }

        /// <summary>
        /// Cmdlet begin process. Write to logs, setup Http Tracing and initialize profile
        /// </summary>
        protected override void BeginProcessing()
        {
            SessionState = base.SessionState;

            LogCmdletStartInvocationInfo();

            KeePassProfile = KeePassProfile.SyncFromDisk();

            if (KeePassProfile.TestProfile != null)
            {
                KeePassProfile = KeePassProfile.TestProfile;
            }

            KeePassProfile.DefaultProfile = KeePassProfile;


            DatabaseProfileService = new DatabaseProfileService(KeePassProfile, this);
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            try
            {
                base.ProcessRecord();
                this.ExecuteSynchronouslyOrAsJob();
            }
            catch (Exception ex) when (!IsTerminatingError(ex))
            {
                WriteExceptionError(ex);
            }
        }

        private string _implementationBackgroundJobDescription;

        /// <summary>
        /// Job Name property if this cmdlet is run as a job
        /// </summary>
        public virtual string ImplementationBackgroundJobDescription
        {
            get
            {
                if (_implementationBackgroundJobDescription != null)
                {
                    return _implementationBackgroundJobDescription;
                }

                string name = "Long Running Operation";
                string commandName = MyInvocation?.MyCommand?.Name;
                string objectName = null;
                if (this.IsBound("Name"))
                {
                    objectName = MyInvocation.BoundParameters["Name"].ToString();
                }
                else if (this.IsBound("InputObject") == true)
                {
                    var type = MyInvocation.BoundParameters["InputObject"].GetType();
                    var inputObject = Convert.ChangeType(MyInvocation.BoundParameters["InputObject"], type);
                    if (type.GetProperty("Name") != null)
                    {
                        objectName = inputObject.GetType().GetProperty("Name").GetValue(inputObject).ToString();
                    }
                }

                if (!string.IsNullOrWhiteSpace(commandName))
                {
                    if (!string.IsNullOrWhiteSpace(objectName))
                    {
                        name = string.Format("Long Running Operation for '{0}' on resource '{1}'", commandName,
                            objectName);
                    }
                    else
                    {
                        name = string.Format("Long Running Operation for '{0}'", commandName);
                    }
                }

                return name;
            }
            set => _implementationBackgroundJobDescription = value;
        }

        public void SetBackgroundJobDescription(string jobName)
        {
            ImplementationBackgroundJobDescription = jobName;
        }

        /// <summary>
        /// Perform end of pipeline processing.
        /// </summary>
        protected override void EndProcessing()
        {
            LogCmdletEndInvocationInfo();
            KeePassProfile.DefaultProfile = KeePassProfile;
            base.EndProcessing();
        }

        //Disposal Code Here

        protected virtual void Dispose(bool disposing)
        {
            try
            {
                FlushDebugMessages();
            }
            catch
            {
            }
        }

        public void Dispose()
        {
            //Dispose(true);
            //GC.SuppressFinalize(this);
        }

        public virtual bool IsTerminatingError(Exception ex)
        {
            var pipelineStoppedEx = ex as PipelineStoppedException;
            if (pipelineStoppedEx != null && pipelineStoppedEx.InnerException == null)
            {
                return true;
            }

            return false;
        }
    }
}