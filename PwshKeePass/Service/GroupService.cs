using System.Collections.Generic;
using System.Management.Automation;
using KeePassLib;
using PwshKeePass.Common;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Model;
using PwshKeePass.Profile;

namespace PwshKeePass.Service
{
    public class GroupService : KeePassService
    {
        private readonly PwDatabase m_connection;

        public GroupService(KeePassProfile keePassProfile, KeePassCmdlet keePassCmdlet, PwDatabase connection) : base(
            keePassProfile, keePassCmdlet)
        {
            m_connection = connection;
            if (!connection.IsOpen)
                throw new PSArgumentOutOfRangeException("Connection is not open.");
        }

        public List<PSKeePassGroup> GetGroups()
        {
            var psKeePassGroups = new List<PSKeePassGroup>();
            var groups = new List<PwGroup> {m_connection.RootGroup};
            groups.AddRange(m_connection.RootGroup.GetFlatGroupList());
            foreach (var group in groups)
            {
                var pso = group.ConvertToPsObject();
                psKeePassGroups.Add(pso);
            }

            return psKeePassGroups;
        }

        public PSKeePassGroup GetGroup(string fullPath)
        {
            var entries = new List<PSKeePassGroup>();
            var groups = new List<PwGroup> {m_connection.RootGroup};
            groups.AddRange(m_connection.RootGroup.GetFlatGroupList());

            foreach (var group in groups)
            {
                if (!string.IsNullOrEmpty(fullPath) & !group.MatchFilter("FullPath", fullPath))
                    continue;

                var pso = group.ConvertToPsObject();
                entries.Add(pso);
            }

            if (entries.Count == 1)
            {
                return entries[0];
            }

            throw new PSArgumentException($"Provided Group {fullPath} Found [{entries.Count}] Times.");
        }

        public PwGroup GetRecycleBin()
        {
            if (m_connection.RecycleBinEnabled)
            {
                var recycleBin = m_connection.RootGroup.FindGroup(m_connection.RecycleBinUuid, true);
                if (recycleBin == null)
                {
                    recycleBin = new PwGroup(true, true, "Recycle Bin", PwIcon.TrashBin)
                    {
                        EnableAutoType = false, EnableSearching = false
                    };
                    m_connection.RootGroup.AddGroup(recycleBin, true);
                    m_connection.RecycleBinUuid = recycleBin.Uuid;
                    m_connection.Save(null);
                    recycleBin = m_connection.RootGroup.FindGroup(m_connection.RecycleBinUuid, true);
                }

                return recycleBin;
            }

            return null;
        }

        public PSKeePassGroup UpdateGroup(PwGroup keePassGroup, string keePassGroupParentPath,
            string keePassGroupName, PwIcon iconName)
        {
            keePassGroup = GetGroup(keePassGroup.GetFullPath("/", true)).KPGroup;

            if (!string.IsNullOrEmpty(keePassGroupParentPath))
            {
                var parentGroup = GetGroup(keePassGroupParentPath);
                var updatedGroup = keePassGroup.CloneDeep();
                updatedGroup.Uuid = new PwUuid(true);
                parentGroup.KPGroup.AddGroup(updatedGroup, true, true);
                m_connection.Save(null);
                keePassGroup.ParentGroup.Groups.Remove(keePassGroup);
                m_connection.Save(null);
                keePassGroup = updatedGroup;
            }

            if (!string.IsNullOrEmpty(keePassGroupName))
            {
                keePassGroup.Name = keePassGroupName;
            }

            if (iconName != keePassGroup.IconId)
                keePassGroup.IconId = iconName;
            m_connection.Save(null);
            return keePassGroup.ConvertToPsObject();
        }

        public PSKeePassGroup CreateGroup(string keePassGroupParentPath, string keePassGroupName, PwIcon iconName)
        {
            var group = new PwGroup(true,true);
            group.Name = keePassGroupName;
            if (iconName != group.IconId)
                group.IconId = iconName;
            var parentGroup = GetGroup(keePassGroupParentPath);
            parentGroup.KPGroup.AddGroup(group, true);
            m_connection.Save(null);
            return group.ConvertToPsObject();
        }

        public void RemoveGroup(PwGroup group, bool noRecycle)
        {
            var recycleBin = GetRecycleBin();
            if (recycleBin != null && !noRecycle)
            {
                var deletedKeePassGroup = group.CloneDeep();
                deletedKeePassGroup.Uuid = new PwUuid(true);
                recycleBin.AddGroup(deletedKeePassGroup, true);
                m_connection.Save(null);
            }

            group.ParentGroup.Groups.Remove(group);
            m_connection.Save(null);
        }
    }
}