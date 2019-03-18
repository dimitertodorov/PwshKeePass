using System.Management.Automation;
using KeePassLib;
using PwshKeePass.Common;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Database
{
    [Cmdlet(VerbsCommon.Remove, "KeePassConnection")]
    public class RemoveKeePassConnection : KeePassCmdlet
    {
        [Parameter(Position = 0, Mandatory = true)]
        public PwDatabase Connection { get; set; }

        public override void ExecuteCmdlet()
        {
            if (Connection.IsOpen)
            {
                Connection.Close();
            }
            else
            {
                WriteWarning("[PROCESS] The KeePass Database Specified is already closed or does not exist.");
            }
        }
    }
}