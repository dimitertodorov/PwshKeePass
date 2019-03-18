using System.IO;
using System.Management.Automation;
using PwshKeePass.Common;
using PwshKeePass.Profile;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Database
{
    [Cmdlet(VerbsCommon.Remove, "KeePassConfigurationFile")]
    public class RemoveKeePassConfigurationFile : KeePassCmdlet
    {
        [Parameter] public SwitchParameter Force { get; set; }

        public override void ExecuteCmdlet()
        {
            if (File.Exists(KeePassProfile.GetDefaultPath()) & !Force.ToBool())
            {
                WriteWarning(
                    "[PROCESS] A KeePass Configuration File already exists. Please rerun with -force to overwrite the existing configuration.");
                return;
            }

            if (File.Exists(KeePassProfile.GetDefaultPath()))
            {
                File.Delete(KeePassProfile.GetDefaultPath());
            }

            if (KeePassProfile.DefaultProfile != null)
            {
                KeePassProfile.DefaultProfile = null;
            }

            KeePassProfile = null;
        }
    }
}