using System.Management.Automation;
using KeePassLib;
using PwshKeePass.Common;
using PwshKeePass.Model;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Group
{
    [Cmdlet(VerbsData.Update, "KeePassGroup", SupportsShouldProcess = true)]
    [OutputType("null")]
    [OutputType(typeof(PSKeePassGroup))]
    public class UpdateKeePassGroup : KeePassConnectedCmdlet
    {
        [Parameter(Mandatory = true,
            ValueFromPipeline = true)]
        public PSObject KeePassGroup { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("FullPath")]
        public string KeePassParentGroupPath { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("GroupName")]
        public string KeePassGroupName { get; set; }

        [Parameter(Mandatory = false)] public PwIcon IconName { get; set; }

        [Parameter(Mandatory = false)] public SwitchParameter PassThru { get; set; }

        [Parameter(HelpMessage = "Force")]
        [ValidateNotNull]
        public SwitchParameter Force { get; set; }

        public override void ExecuteCmdlet()
        {
            var groupService = new GroupService(KeePassProfile, this, Connection);
            var keePassPwGroup = ResolveKeePassGroup(KeePassGroup);
            if (!(Force || ShouldProcess(keePassPwGroup.Name, "Update"))) return;
            var group = groupService.UpdateGroup(keePassPwGroup, KeePassParentGroupPath, KeePassGroupName, IconName);
            if (PassThru)
            {
                WriteObject(group);
            }
        }
    }
}