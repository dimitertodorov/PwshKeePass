using System.Management.Automation;
using PwshKeePass.Common;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Entry
{
    [Cmdlet(VerbsCommon.Remove, "KeePassEntry", SupportsShouldProcess = true)]
    public class RemoveKeePassEntry : KeePassConnectedCmdlet
    {
        [Parameter(Mandatory = true,
            ValueFromPipeline = true)]
        public PSObject KeePassEntry { get; set; }

        [Parameter(HelpMessage = "NoRecycle")]
        [ValidateNotNull]
        public SwitchParameter NoRecycle { get; set; }

        [Parameter(HelpMessage = "Force")]
        [ValidateNotNull]
        public SwitchParameter Force { get; set; }

        public override void ExecuteCmdlet()
        {
            var keePassPwEntry = ResolveKeePassEntry(KeePassEntry);
            var entryService = new EntryService(KeePassProfile, this, Connection);
            var entryDisplayName =
                $"{keePassPwEntry.ParentGroup.GetFullPath("/", true)}/ {keePassPwEntry.Strings.ReadSafe("Title")}";
            var entries = entryService.GetEntry(keePassPwEntry.Uuid.ToHexString(), null, null, null);
            if (entries.Count == 0)
                throw new PSArgumentException(
                    $"Could not find PwEntry with UUID {keePassPwEntry.Uuid} in PwDatabase. Ensure you are using the same Profile/Connection");

            if (Force | ShouldProcess(entryDisplayName, "Remove"))
            {
                entryService.RemoveEntry(keePassPwEntry, NoRecycle.ToBool());
            }
        }
    }
}