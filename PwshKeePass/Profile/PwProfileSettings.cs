using KeePassLib.Cryptography.PasswordGenerator;
using Newtonsoft.Json;

namespace PwshKeePass.Profile
{
    public class PwProfileSettings
    {
        [JsonProperty] public string Name;
        [JsonProperty] public string CharacterSet;
        [JsonProperty] public bool ExcludeLookAlike;
        [JsonProperty] public bool NoRepeatingCharacters;
        [JsonProperty] public string ExcludeCharacters;
        [JsonProperty] public uint Length;

        public PwProfileSettings()
        {
            CharacterSet = "";
            ExcludeLookAlike = true;
            NoRepeatingCharacters = false;
            ExcludeCharacters = "";
            Length = 20;
        }

        public PwProfile ToPwProfile()
        {
            var pwProfile = new PwProfile {CharSet = new PwCharSet()};
            pwProfile.CharSet.Add(CharacterSet);
            pwProfile.ExcludeLookAlike = ExcludeLookAlike;
            pwProfile.ExcludeCharacters = ExcludeCharacters;
            pwProfile.NoRepeatingCharacters = NoRepeatingCharacters;
            pwProfile.Length = Length;
            return pwProfile;
        }
    }
}