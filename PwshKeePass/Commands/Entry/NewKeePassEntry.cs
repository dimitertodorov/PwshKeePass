using System.Management.Automation;
using PwshKeePass.Common;
using PwshKeePass.Model;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Entry
{
    [Cmdlet(VerbsCommon.New, "KeePassEntry", SupportsShouldProcess = true)]
    [OutputType("null")]
    [OutputType(typeof(PSKeePassEntry))]
    public class NewKeePassEntry : KeePassEntryModificationCmdlet
    {
        [Parameter(Mandatory = true)]
        [Alias("FullPath")]
        public string KeePassEntryGroupPath { get; set; }

        [Parameter(Mandatory = true)] public string Title { get; set; }

        [Parameter(Mandatory = true)] public string UserName { get; set; }

        public override void ExecuteCmdlet()
        {
            var entryService = new EntryService(KeePassProfile, this, Connection);
            var entry = entryService.CreateEntry(Title, UserName, SecurePassword, Notes, Url, IconName,
                KeePassEntryGroupPath);
            if (PassThru)
            {
                WriteObject(entry);
            }
        }
    }
}