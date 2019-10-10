using System.Management.Automation;
using KeePassLib.Interfaces;
using PwshKeePass.Common;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Model;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Entry
{
    [Cmdlet(VerbsData.Update, "KeePassEntry", SupportsShouldProcess = true)]
    [OutputType("null")]
    [OutputType(typeof(PSKeePassEntry))]
    public class UpdateKeePassEntry : KeePassEntryModificationCmdlet
    {
        [Parameter(Mandatory = true,
            ValueFromPipeline = true)]
        public PSObject KeePassEntry { get; set; }

        [Parameter(Mandatory = false)]
        [Alias("FullPath")]
        public string KeePassEntryGroupPath { get; set; }

        [Parameter(Mandatory = false)] public string Title { get; set; }

        [Parameter(Mandatory = false)] public string UserName { get; set; }

        public override void ExecuteCmdlet()
        {
            var entryService = new EntryService(KeePassProfile, this, Connection);
            var keePassPwEntry = ResolveKeePassEntry(KeePassEntry);

            var entry = entryService.UpdateEntry(keePassPwEntry, Title, UserName, SecurePassword, Notes, Url,
                IconName,
                KeePassEntryGroupPath);
            if (entry != null)
            {
                Connection.Save(new NullStatusLogger());
            }

            if (PassThru)
            {
                WriteObject(entry.ConvertToPsKeePassEntry());
            }
        }
    }
}