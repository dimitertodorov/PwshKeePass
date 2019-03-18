using System.Management.Automation;
using KeePassLib;
using PwshKeePass.Common;
using PwshKeePass.Model;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Group
{
    [Cmdlet(VerbsCommon.New, "KeePassGroup", SupportsShouldProcess = true)]
    [OutputType("null")]
    [OutputType(typeof(PSKeePassGroup))]
    public class NewKeePassGroup : KeePassConnectedCmdlet
    {
        [Parameter(Mandatory = true)]
        [Alias("FullPath")]
        public string KeePassGroupParentPath { get; set; }

        [Parameter(Mandatory = true)]
        [Alias("GroupName")]
        public string KeePassGroupName { get; set; }

        [Parameter(Mandatory = false)] public PwIcon IconName { get; set; }

        [Parameter(Mandatory = false)] public SwitchParameter PassThru { get; set; }

        public override void ExecuteCmdlet()
        {
            var groupService = new GroupService(KeePassProfile, this, Connection);
            var group = groupService.CreateGroup(KeePassGroupParentPath, KeePassGroupName, IconName);
            if (PassThru)
            {
                WriteObject(group);
            }
        }
    }
}