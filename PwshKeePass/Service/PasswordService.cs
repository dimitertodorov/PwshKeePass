using System.Collections;
using System.Management.Automation;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using PwshKeePass.Common;
using PwshKeePass.Profile;
using static System.String;

namespace PwshKeePass.Service
{
    public class PasswordService : KeePassService
    {

        public PasswordService(KeePassProfile keePassProfile, KeePassCmdlet keePassCmdlet) : base(
            keePassProfile, keePassCmdlet)
        {
        }


        public PwProfileSettings GetKeePassPasswordProfile(string profileName)
        {
            return KeePassProfile.PasswordProfiles.ContainsKey(profileName)
                ? KeePassProfile.PasswordProfiles[profileName]
                : null;
        }

        public PwProfile NewKeePassPasswordProfile(bool upperCase, bool lowerCase, bool digits, bool specialCharacters,
            bool minus, bool underScore, bool space, bool brackets, bool excludeLookAlike, bool noRepeatingCharacters,
            string excludeCharacters, int length, string saveAs)
        {
            var profileSettings = new PwProfileSettings();
            if (upperCase)
            {
                profileSettings.CharacterSet += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            if (lowerCase)
            {
                profileSettings.CharacterSet += "abcdefghijklmnopqrstuvwxyz";
            }

            if (digits)
            {
                profileSettings.CharacterSet += "0123456789";
            }

            if (specialCharacters)
            {
                profileSettings.CharacterSet += @"!""#$%&'*+,./:;=?@\^`|~";
            }

            if (minus)
            {
                profileSettings.CharacterSet += '-';
            }

            if (underScore)
            {
                profileSettings.CharacterSet += '_';
            }

            if (space)
            {
                profileSettings.CharacterSet += ' ';
            }

            if (brackets)
            {
                profileSettings.CharacterSet += @"[]{}()<>";
            }

            profileSettings.ExcludeLookAlike = excludeLookAlike;
            profileSettings.NoRepeatingCharacters = noRepeatingCharacters;
            if (!IsNullOrEmpty(excludeCharacters))
            {
                profileSettings.ExcludeCharacters = excludeCharacters;
            }

            if (length != 0)
            {
                profileSettings.Length = (uint) length;
            }


            var pwProfile = profileSettings.ToPwProfile();
            var testPassword = GeneratePassword(profileSettings.ToPwProfile());
            if (!IsNullOrEmpty(saveAs) && testPassword != null)
            {
                profileSettings.Name = saveAs;
                KeePassProfile.PasswordProfiles[saveAs] = profileSettings;
                KeePassProfile.SaveToDisk();
            }

            return pwProfile;
        }

        public ProtectedString GeneratePassword(PwProfile pwProfile)
        {
            var protectedString = new ProtectedString();
            var genPool = new CustomPwGeneratorPool();
            var result = PwGenerator.Generate(out protectedString, pwProfile, null, genPool);
            if (result != PwgError.Success)
            {
                if (result == PwgError.TooFewCharacters)
                {
                    Cmdlet.WriteWarning(
                        $"[PROCESS] Result Text {result}, typically means that you specified a length that is longer than the possible generated outcome.");
                    var excludeCharacters = !IsNullOrEmpty(pwProfile.ExcludeCharacters)
                        ? ((ICollection)
                            pwProfile.ExcludeCharacters.Split(',')).Count
                        : 0;
                    if (pwProfile.NoRepeatingCharacters &
                        (pwProfile.Length > pwProfile.CharSet.Size - excludeCharacters))
                    {
                        Cmdlet.WriteWarning(
                            $"[PROCESS] Checked for the invalid specification. \n\tSpecified Length: {pwProfile.Length}. \n\tCharacterSet Count: {pwProfile.CharSet.Size}. \n\tNo Repeating Characters is set to: {pwProfile.NoRepeatingCharacters}. \n\tExclude Character Count: {excludeCharacters}.");
                        Cmdlet.WriteWarning(
                            $"[PROCESS] Specify More characters, shorten the length, remove the no repeating characters option, or removed excluded characters.");
                    }
                }

                throw new PSArgumentException($"Unable to generate a password with the specified options. Result {result}");
            }

            return protectedString;
        }
    }
}