using System.Management.Automation;
using PwshKeePass.Common;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Group
{
    [Cmdlet(VerbsCommon.Remove, "KeePassGroup", SupportsShouldProcess = true)]
    [OutputType("null")]
    public class RemoveKeePassGroup : KeePassConnectedCmdlet
    {
        [Parameter(Mandatory = true,
            ValueFromPipeline = true)]
        public PSObject KeePassGroup { get; set; }

        [Parameter] [ValidateNotNull] public SwitchParameter NoRecycle { get; set; }

        [Parameter] [ValidateNotNull] public SwitchParameter Force { get; set; }

        public override void ExecuteCmdlet()
        {
            var groupService = new GroupService(KeePassProfile, this, Connection);
            var keePassPwGroup = ResolveKeePassGroup(KeePassGroup);
            keePassPwGroup = groupService.GetGroup(keePassPwGroup.GetFullPath("/", true)).KPGroup;

            if (Force | ShouldProcess(keePassPwGroup.GetFullPath("/", true), "Remove"))
            {
                groupService.RemoveGroup(keePassPwGroup, NoRecycle.ToBool());
            }
        }
    }
}