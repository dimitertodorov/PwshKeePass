using System.Management.Automation;
using System.Security;

namespace PwshKeePass.Common
{
    public abstract class KeePassEntryModificationCmdlet : KeePassConnectedCmdlet
    {
        [Parameter(Mandatory = false)]
        public PSObject KeePassPassword { get; set; }

        [Parameter(Mandatory = false)]
        public string Notes { get; set; }

        [Parameter(Mandatory = false)]
        public string Url { get; set; }

        [Parameter(Mandatory = false)]
        public string IconName { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter PassThru { get; set; }

        protected SecureString SecurePassword;

        protected override void BeginProcessing()
        {
            SecurePassword = ResolveKeePassPassword(KeePassPassword);
            base.BeginProcessing();
        }
    }
}