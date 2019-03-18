using System.Management.Automation;
using System.Security;
using PwshKeePass.Common;
using PwshKeePass.Profile;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Database
{
    [Cmdlet(VerbsCommon.New, "KeePassDatabaseConfiguration")]
    [OutputType(typeof(DatabaseProfile))]
    public class NewKeePassDatabaseConfiguration : KeePassCmdlet
    {
        public const string Key = "Key";
        public const string Master = "Master";
        public const string KeyAndMaster = "KeyAndMaster";

        [Parameter(Mandatory = true)] public string DatabaseProfileName { get; set; }

        [Parameter(ParameterSetName = Key,
            Position = 2, Mandatory = true)]
        [Parameter(ParameterSetName = Master,
            Position = 2, Mandatory = true)]
        [Parameter(ParameterSetName = KeyAndMaster,
            Position = 2, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string DatabasePath { get; set; }

        [Parameter(ParameterSetName = KeyAndMaster,
            Position = 3, Mandatory = true)]
        [Parameter(ParameterSetName = Key,
            Position = 3, Mandatory = true)]
        public string KeyPath { get; set; }


        [Parameter(ParameterSetName = KeyAndMaster,
            Position = 4, Mandatory = true)]
        [Parameter(ParameterSetName = Master,
            Position = 4, Mandatory = true)]
        public SecureString MasterKey { get; set; }

        [Parameter(Mandatory = false)] public SwitchParameter Default { get; set; }

        [Parameter(Mandatory = false)] public SwitchParameter Save { get; set; }

        [Parameter(Mandatory = false)] public SwitchParameter PassThru { get; set; }

        [Parameter(Mandatory = false,
            HelpMessage = "Force Configuration. (This will OverWrite existing configuration)")]
        public SwitchParameter Force { get; set; }

        public override void ExecuteCmdlet()
        {
            DatabaseProfile dbProfile = DatabaseProfileService.NewKeePassDatabaseConfiguration(
                ParameterSetName, DatabaseProfileName, DatabasePath, KeyPath, MasterKey,
                Default.ToBool(), Save.ToBool(), Force.ToBool());
            if (PassThru)
                WriteObject(dbProfile);
        }
    }
}