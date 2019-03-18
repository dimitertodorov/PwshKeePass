using System.Management.Automation;
using PwshKeePass.Common;

namespace PwshKeePass.Test.Mocks
{
    [Cmdlet(VerbsCommon.Get, "KeePassEntry", SupportsShouldProcess = true)]
    public class TestKeePassCmdlet : KeePassCmdlet
    {
        
        protected override void ProcessRecord()
        {
                WriteObject(0);
        }
    }
}