using System;
using System.Globalization;
using System.Linq;
using KeePassLib;
using PwshKeePass.Model;

namespace PwshKeePass.Common.Extensions
{
    public static class PwGroupExtension
    {
        public static bool MatchFilter(this PwGroup entry, string propertyKey, string filterValue)
        {
            var entryValue = DoSwitch(entry, propertyKey);

            string DoSwitch(PwGroup e, string k)
            {
                switch (k)
                {
                    case "Uuid": return e.Uuid.ToString();
                    case "CreationTime": return e.CreationTime.ToString(CultureInfo.InvariantCulture);
                    case "FullPath": return e.GetFullPath("/", true);
                    default:
                    {
                        throw new ArgumentOutOfRangeException($"Unknown Filter property {k}");
                    }
                }
            }

            return string.Equals(entryValue, filterValue);
        }

        public static PSKeePassGroup ConvertToPsObject(this PwGroup group)
        {
            var psKeePassGroup = new PSKeePassGroup
            {
                Uuid = @group.Uuid.ToHexString(),
                Name = @group.Name,
                CreationTime = @group.CreationTime,
                Expires = @group.Expires,
                ExpiryTime = @group.ExpiryTime,
                LastAccessTime = @group.LastAccessTime,
                LastModificationTime = @group.LastModificationTime,
                LocationChanged = @group.LocationChanged,
                UsageCount = @group.UsageCount,
                FullPath = @group.GetFullPath("/", true),
                Groups = @group.Groups,
                EntryCount = @group.Entries.Count(),
                IconId = @group.IconId,
                KPGroup = @group,
                ParentGroup = @group.ParentGroup != null
                    ? @group.ParentGroup.Name
                    : ""
            };

            return psKeePassGroup;
        }
    }
}