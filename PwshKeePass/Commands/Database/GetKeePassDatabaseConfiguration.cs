using System.Management.Automation;
using PwshKeePass.Common;
using PwshKeePass.Profile;
// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Database
{
    [Cmdlet(VerbsCommon.Get, "KeePassDatabaseConfiguration")]
    [OutputType(typeof(DatabaseProfile))]
    public class GetKeePassDatabaseConfiguration : KeePassCmdlet
    {
        [Parameter(Position = 0, Mandatory = false)]
        [ValidateNotNullOrEmpty]
        public string DatabaseProfileName { get; set; }

        public override void ExecuteCmdlet()
        {
            if (!string.IsNullOrEmpty(DatabaseProfileName))
            {
                WriteObject(DatabaseProfileService.GetDatabaseProfile(DatabaseProfileName));
            }
            else
            {
                foreach (var profile in KeePassProfile.ActiveProfiles.Values)
                {
                    WriteObject(profile);
                }
            }
        }
    }
}