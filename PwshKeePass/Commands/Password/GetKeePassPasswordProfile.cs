using System.Management.Automation;
using PwshKeePass.Common;
using PwshKeePass.Profile;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Password
{
    [Cmdlet(VerbsCommon.Get, "KeePassPasswordProfile")]
    [OutputType(typeof(PwProfileSettings))]
    public class GetKeePassPasswordProfile : KeePassCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string PasswordProfileName { get; set; }

        public override void ExecuteCmdlet()
        {
            var passwordService = new PasswordService(KeePassProfile, this);
            var profile = passwordService.GetKeePassPasswordProfile(PasswordProfileName);
            if (profile != null)
                WriteObject(profile);
        }
    }
}