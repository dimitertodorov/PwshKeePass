using System.Management.Automation;
using PwshKeePass.Common;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Database
{
    [Cmdlet(VerbsCommon.Remove, "KeePassDatabaseConfiguration")]
    public class RemoveKeePassDatabaseConfiguration : KeePassCmdlet
    {
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        [Alias("Name")]
        public string DatabaseProfileName { get; set; }

        public override void ExecuteCmdlet()
        {
            DatabaseProfileService.RemoveKeePassDatabaseConfiguration(DatabaseProfileName);
        }
    }
}