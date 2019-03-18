using System;
using System.Management.Automation;
using System.Security;
using KeePassLib;
using PwshKeePass.Common;
// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Database
{
    [Cmdlet(VerbsCommon.New, "KeePassConnection")]
    [OutputType(typeof(PwDatabase))]
    public class NewKeePassConnection : KeePassCmdlet
    {
        public const string Key = "Key";
        public const string Master = "Master";
        public const string KeyAndMaster = "KeyAndMaster";
        public const string DatabaseProfileParameterSet = "DatabaseProfileParameterSet";

        [Parameter(ParameterSetName = Key, Mandatory = true)]
        [Parameter(ParameterSetName = Master, Mandatory = true)]
        [Parameter(ParameterSetName = KeyAndMaster, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string DatabasePath { get; set; }


        [Parameter(ParameterSetName = Key, Mandatory = true)]
        [Parameter(ParameterSetName = KeyAndMaster, Mandatory = true)]
        public string KeyPath { get; set; }

        [Parameter(ParameterSetName = Master, Mandatory = true)]
        [Parameter(ParameterSetName = KeyAndMaster, Mandatory = true)]
        public SecureString MasterKey { get; set; }

        [Parameter(ParameterSetName = DatabaseProfileParameterSet, Mandatory = true)]

        public string DatabaseProfileName { get; set; }

        public override void ExecuteCmdlet()
        {
            string paramSet;
            if (!String.IsNullOrEmpty(KeyPath) && MasterKey != null)
            {
                paramSet = KeyAndMaster;
            }
            else if (!String.IsNullOrEmpty(KeyPath))
            {
                paramSet = Key;
            }
            else if (MasterKey != null)
            {
                paramSet = Master;
            }
            else if (!String.IsNullOrEmpty(DatabaseProfileName))
            {
                paramSet = DatabaseProfileParameterSet;
            }
            else
            {
                throw new PSArgumentNullException(@"Could Not determine Parameter Set");
            }

            if (paramSet == DatabaseProfileParameterSet)
            {
                WriteObject(DatabaseProfileService.GetDatabaseProfile(DatabaseProfileName, true).PwDatabase);
            }
            else
            {
                var dbProfile =
                    DatabaseProfileService.GetDatabaseProfile(paramSet, DatabasePath, KeyPath, MasterKey);
                dbProfile.Connect();
                if (dbProfile.PwDatabase.IsOpen)
                {
                    WriteObject(dbProfile.PwDatabase);
                }
                else
                {
                    WriteErrorWithTimestamp($"KeePass PwDatabase is not Open. Path: ${DatabasePath}");
                }
            }
        }
    }
}