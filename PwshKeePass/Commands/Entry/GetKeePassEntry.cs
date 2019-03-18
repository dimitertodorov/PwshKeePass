using System;
using System.Management.Automation;
using PwshKeePass.Common;
using PwshKeePass.Model;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Entry
{
    [Cmdlet(VerbsCommon.Get, "KeePassEntry")]
    [OutputType(typeof(PSKeePassEntry))]
    public class GetKeePassEntry : KeePassConnectedCmdlet
    {
        [Parameter(Mandatory = false)] public string Title { get; set; }

        [Parameter(Mandatory = false)] public string Uuid { get; set; }

        [Parameter(Mandatory = false)] public string UserName { get; set; }

        [Parameter(Mandatory = false)] public string KeePassEntryGroupPath { get; set; }

        public override void ExecuteCmdlet()
        {
            var entryService = new EntryService(KeePassProfile, this, Connection);
            if (!String.IsNullOrEmpty(KeePassEntryGroupPath))
            {
                var groupService = new GroupService(KeePassProfile, this, Connection);
                var unused = groupService.GetGroup(KeePassEntryGroupPath);
            }

            var entries = entryService.GetEntry(Uuid, Title, UserName, KeePassEntryGroupPath);
            foreach (var entry in entries.ToArray())
            {
                WriteObject(entry);
            }
        }
    }
}