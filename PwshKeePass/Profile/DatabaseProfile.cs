using System.IO;
using System.Security;
using KeePassLib;
using KeePassLib.Interfaces;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using Newtonsoft.Json;
using PwshKeePass.Common.Extensions;
using PwshKeePass.Service;

namespace PwshKeePass.Profile
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DatabaseProfile
    {
        [JsonProperty] public string Name { get; set; }

        [JsonProperty] public string AuthenticationType { get; set; }
        [JsonProperty] public string DatabasePath { get; set; }
        [JsonProperty] public string KeyPath { get; set; }
        [JsonProperty] public bool UseMasterKey { get; set; }
        [JsonProperty] public bool UseNetworkAccount { get; set; }
        public SecureString MasterKey { get; set; }

        public PwDatabase PwDatabase { get; set; }

        public CompositeKey InitPwDatabaseKey()
        {
            if (PwDatabase != null && PwDatabase.IsOpen)
                return null;
            var key = new CompositeKey();

            if (UseMasterKey && MasterKey != null)
            {
                key.AddUserKey(new KcpPassword(MasterKey.ConvertToUnsecureString()));
            }
            else if (UseMasterKey && MasterKey == null)
            {
                var manualMaster = KeePassService.GetPassword($"Enter Master password for '{DatabasePath}':");
                MasterKey = manualMaster;
                key.AddUserKey(new KcpPassword(MasterKey.ConvertToUnsecureString()));
            }


            if (!string.IsNullOrEmpty(KeyPath))
            {
                key.AddUserKey(new KcpKeyFile(Path.GetFullPath(KeyPath)));
            }


            return key;
        }

        public void Connect()
        {
            var key = InitPwDatabaseKey();
            var ioInfo = new IOConnectionInfo {Path = Path.GetFullPath(DatabasePath)};
            PwDatabase = new PwDatabase();
            PwDatabase.Open(ioInfo, key, new NullStatusLogger());
        }
    }
}