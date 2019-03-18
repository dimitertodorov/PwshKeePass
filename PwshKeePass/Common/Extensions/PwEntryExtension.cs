using System;
using System.Globalization;
using System.Management.Automation;
using System.Security;
using KeePassLib;
using PwshKeePass.Model;

namespace PwshKeePass.Common.Extensions
{
    public static class PwEntryExtension
    {
        public static bool MatchFilter(this PwEntry entry, string propertyKey, string filterValue)
        {
            string entryValue = DoSwitch(entry, propertyKey);

            string DoSwitch(PwEntry e, string k)
            {
                switch (k)
                {
                    case "Uuid": return e.Uuid.ToHexString();
                    case "CreationTime": return e.CreationTime.ToString(CultureInfo.InvariantCulture);
                    case "Tags": return e.Tags != null ? string.Join(",", e.Tags) : "";
                    case "FullPath": return e.ParentGroup.GetFullPath("/", true);
                    default:
                    {
                        if (e.Strings.GetKeys().Contains(k))
                        {
                            return e.Strings.ReadSafe(k);
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException($"Unknown Filter property {k}");
                        }
                    }
                }
            }

            return string.Equals(entryValue, filterValue);
        }

        public static PSKeePassEntry ConvertToPsKeePassEntry(this PwEntry entry)
        {
            var pso = new PSKeePassEntry
            {
                UserName = entry.Strings.ReadSafe("UserName"),
                Uuid = entry.Uuid.ToHexString(),
                CreationTime = entry.CreationTime,
                Expires = entry.Expires,
                ExpiryTime = entry.ExpiryTime,
                LastAccessTime = entry.LastAccessTime,
                LastModificationTime = entry.LastModificationTime,
                LocationChanged = entry.LocationChanged,
                Tags = entry.Tags,
                UsageCount = entry.UsageCount,
                Title = entry.Strings.ReadSafe("Title"),
                Url = entry.Strings.ReadSafe("URL"),
                Notes = entry.Strings.ReadSafe("Notes"),
                IconId = entry.IconId,
                KPEntry = entry
            };
            if (entry.ParentGroup != null)
            {
                pso.ParentGroup = entry.ParentGroup.Name;
                pso.FullPath = entry.ParentGroup.GetFullPath("/", true);
            }

            var securePassword = new SecureString();
            foreach (char c in entry.Strings.ReadSafe("Password"))
                securePassword.AppendChar(c);

            pso.Credential =
                !string.IsNullOrEmpty(entry.Strings.ReadSafe("UserName"))
                    ? new PSCredential(entry.Strings.ReadSafe("UserName"), securePassword)
                    : new PSCredential("none", securePassword);

            pso.Password = securePassword;

            return pso;
        }
    }
}