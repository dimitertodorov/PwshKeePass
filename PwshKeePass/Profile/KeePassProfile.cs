using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using Newtonsoft.Json;

namespace PwshKeePass.Profile
{
    public class KeePassProfile
    {
        [JsonProperty] public string DefaultProfileName { get; set; }
        [JsonProperty] public Dictionary<string, DatabaseProfile> ActiveProfiles { get; set; }

        [JsonProperty] public Dictionary<string, PwProfileSettings> PasswordProfiles { get; set; }

        public static KeePassProfile DefaultProfile { get; set; }
        public static KeePassProfile TestProfile { get; set; }

        public KeePassProfile()
        {
            ActiveProfiles = new Dictionary<string, DatabaseProfile>();
            PasswordProfiles = new Dictionary<string, PwProfileSettings>();
        }

        public static string GetDefaultPath()
        {
            string path;
            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("USERPROFILE")))
            {
                path = $"{Environment.GetEnvironmentVariable("USERPROFILE")}/.pwshKeePass";
            }
            else if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("HOME")))
            {
                path = $"{Environment.GetEnvironmentVariable("HOME")}/.pwshKeePass";
            }
            else
            {
                throw new FileNotFoundException("Could not find HOME or USERPROFILE Environment Variable");
            }

            return Path.GetFullPath(path);
        }

        public void SaveToDisk()
        {
            var path = GetDefaultPath();
            DefaultProfile = this;
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static KeePassProfile SyncFromDisk()
        {
            var path = GetDefaultPath();
            var oldProfile = DefaultProfile;
            var masterKeyDict = new Dictionary<string, SecureString>();
            if (oldProfile != null)
            {
                foreach (KeyValuePair<string, DatabaseProfile> entry in oldProfile.ActiveProfiles)
                {
                    masterKeyDict.Add(entry.Key, entry.Value.MasterKey);
                }
            }

            try
            {
                var jsonContents = File.ReadAllText(path);
                DefaultProfile = JsonConvert.DeserializeObject<KeePassProfile>(jsonContents);
            }
            catch (FileNotFoundException)
            {
            }


            var profile = DefaultProfile ?? new KeePassProfile();
            foreach (KeyValuePair<string, SecureString> entry in masterKeyDict)
            {
                if (profile.ActiveProfiles.ContainsKey(entry.Key))
                {
                    profile.ActiveProfiles[entry.Key].MasterKey = entry.Value;
                }
            }

            return profile;
        }
    }
}