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
        private readonly PwDatabase _mConnection;

        public GroupService(KeePassProfile keePassProfile, KeePassCmdlet keePassCmdlet, PwDatabase connection) : base(
            keePassProfile, keePassCmdlet)
        {
            _mConnection = connection;
            if (!connection.IsOpen)
                throw new PSArgumentOutOfRangeException("Connection is not open.");
        }

        public List<PSKeePassGroup> GetGroups()
        {
            var psKeePassGroups = new List<PSKeePassGroup>();
            var groups = new List<PwGroup> {_mConnection.RootGroup};
            groups.AddRange(_mConnection.RootGroup.GetFlatGroupList());
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
            var groups = new List<PwGroup> {_mConnection.RootGroup};
            groups.AddRange(_mConnection.RootGroup.GetFlatGroupList());

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
            if (_mConnection.RecycleBinEnabled)
            {
                var recycleBin = _mConnection.RootGroup.FindGroup(_mConnection.RecycleBinUuid, true);
                if (recycleBin == null)
                {
                    recycleBin = new PwGroup(true, true, "Recycle Bin", PwIcon.TrashBin)
                    {
                        EnableAutoType = false, EnableSearching = false
                    };
                    _mConnection.RootGroup.AddGroup(recycleBin, true);
                    _mConnection.RecycleBinUuid = recycleBin.Uuid;
                    _mConnection.Save(null);
                    recycleBin = _mConnection.RootGroup.FindGroup(_mConnection.RecycleBinUuid, true);
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
                _mConnection.Save(null);
                keePassGroup.ParentGroup.Groups.Remove(keePassGroup);
                _mConnection.Save(null);
                keePassGroup = updatedGroup;
            }

            if (!string.IsNullOrEmpty(keePassGroupName))
            {
                keePassGroup.Name = keePassGroupName;
            }

            if (iconName != keePassGroup.IconId)
                keePassGroup.IconId = iconName;
            _mConnection.Save(null);
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
            _mConnection.Save(null);
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
                _mConnection.Save(null);
            }

            group.ParentGroup.Groups.Remove(group);
            _mConnection.Save(null);
        }
    }
}