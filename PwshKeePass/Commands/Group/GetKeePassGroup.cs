using System.Management.Automation;
using PwshKeePass.Common;
using PwshKeePass.Model;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Group
{
    [Cmdlet(VerbsCommon.Get, "KeePassGroup")]
    [OutputType(typeof(PSKeePassGroup))]
    public class GetKeePassGroup : KeePassConnectedCmdlet
    {
        [Parameter(Mandatory = false)]
        [Alias("KeePassGroupPath")]
        public string FullPath { get; set; }

        public override void ExecuteCmdlet()
        {
            var groupService = new GroupService(KeePassProfile, this, Connection);
            if (FullPath != null)
            {
                var entry = groupService.GetGroup(FullPath);
                WriteObject(entry);
            }
            else
            {
                var entries = groupService.GetGroups();
                foreach (var entry in entries)
                {
                    WriteObject(entry);
                }
            }
        }
    }
}