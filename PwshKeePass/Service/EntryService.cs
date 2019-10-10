using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Security;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Security;
using PwshKeePass.Common;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Model;
using PwshKeePass.Profile;
using static System.String;

namespace PwshKeePass.Service
{
    public class EntryService : KeePassService
    {
        private readonly PwDatabase m_connection;
        private readonly GroupService m_groupService;

        public EntryService(KeePassProfile keePassProfile, KeePassCmdlet keePassCmdlet, PwDatabase connection) : base(
            keePassProfile, keePassCmdlet)
        {
            m_connection = connection;
            if (!connection.IsOpen)
                throw new PSArgumentOutOfRangeException("Connection is not open.");
            m_groupService = new GroupService(keePassProfile, Cmdlet, m_connection);
        }

        public List<PSKeePassEntry> GetEntry(string uuid, string title, string userName, string keePassEntryGroupPath)
        {
            List<PSKeePassEntry> entries = new List<PSKeePassEntry>();

            foreach (var entry in m_connection.RootGroup.GetEntries(true))
            {
                if (!IsNullOrEmpty(title) & !entry.MatchFilter("Title", title))
                    continue;
                if (!IsNullOrEmpty(uuid) & !entry.MatchFilter("Uuid", uuid))
                    continue;
                if (!IsNullOrEmpty(userName) & !entry.MatchFilter("UserName", userName))
                    continue;
                if (!IsNullOrEmpty(keePassEntryGroupPath) & !entry.MatchFilter("FullPath", keePassEntryGroupPath))
                    continue;

                var pso = entry.ConvertToPsKeePassEntry();
                entries.Add(pso);
            }

            Cmdlet.WriteVerbose($"Enumerated {entries.Count} PwEntry from {m_connection.Name}");
            return entries;
        }
        

        public PwEntry UpdateEntry(PwEntry entry, string title, string userName, SecureString keePassPassword,
            string notes,
            string url, string iconName, string keePassGroupPath)
        {
            if (!IsNullOrEmpty(title))
            {
                entry.Strings.Set("Title", new ProtectedString(m_connection.MemoryProtection.ProtectTitle, title));
            }

            if (!IsNullOrEmpty(userName))
            {
                entry.Strings.Set("UserName",
                    new ProtectedString(m_connection.MemoryProtection.ProtectUserName, userName));
            }

            if (!IsNullOrEmpty(url))
            {
                entry.Strings.Set("URL", new ProtectedString(m_connection.MemoryProtection.ProtectUrl, url));
            }

            if (!IsNullOrEmpty(notes))
            {
                entry.Strings.Set("Notes", new ProtectedString(m_connection.MemoryProtection.ProtectNotes, notes));
            }

            if (!IsNullOrEmpty(iconName))
            {
                if (entry.IconId.ToString() != iconName)
                {
                    try
                    {
                        entry.IconId = (PwIcon) Enum.Parse(typeof(PwIcon), iconName);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }

            if (keePassPassword != null)
            {
                var passwordString = new ProtectedString();
                passwordString = passwordString
                    .Insert(0, Marshal.PtrToStringAuto(Marshal.SecureStringToBSTR(keePassPassword)))
                    .WithProtection(true);
                entry.Strings.Set("Password", passwordString);
            }

            entry.LastAccessTime = DateTime.UtcNow;
            entry.LastModificationTime = DateTime.UtcNow;

            if (!IsNullOrEmpty(keePassGroupPath))
            {
                var newKeePassGroup = m_groupService.GetGroup(keePassGroupPath);

                if (entry.ParentGroup != null)
                {
                    if (!entry.ParentGroup.MatchFilter("FullPath", keePassGroupPath))
                    {
                        entry.ParentGroup.Entries.Remove(entry);
                    }
                }

                newKeePassGroup.KPGroup.AddEntry(entry, true, true);
                newKeePassGroup.KPGroup.Touch(true);
            }

            return entry;
        }

        public PSKeePassEntry CreateEntry(string title, string userName, SecureString keePassPassword, string notes,
            string url, string iconName, string keePassGroupPath)
        {
            var entry = new PwEntry(true, true);

            entry = UpdateEntry(entry, title, userName, keePassPassword, notes, url, iconName, keePassGroupPath);
            m_connection.Save(new NullStatusLogger());
            return entry.ConvertToPsKeePassEntry();
        }

        public void RemoveEntry(PwEntry entry, bool noRecycle)
        {
            var recycleBin = m_groupService.GetRecycleBin();
            if (recycleBin != null && !noRecycle)
            {
                var deletedKeePassEntry = entry.CloneDeep();
                deletedKeePassEntry.Uuid = new PwUuid(true);
                recycleBin.AddEntry(deletedKeePassEntry, true);
                m_connection.Save(null);
            }

            entry.ParentGroup.Entries.Remove(entry);
            m_connection.Save(null);
        }
    }
}