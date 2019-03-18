using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using KeePassLib.Security;
using PwshKeePass.Common;
using PwshKeePass.Profile;
using PwshKeePass.Service;

// ReSharper disable MemberCanBePrivate.Global

namespace PwshKeePass.Commands.Password
{
    [Cmdlet(VerbsCommon.New, "KeePassPassword", DefaultParameterSetName = "NoProfile")]
    [SuppressMessage("ReSharper", "ArgumentsStyleNamedExpression")]
    [OutputType(typeof(ProtectedString))]
    public class NewKeePassPassword : KeePassCmdlet
    {
        public const string NoProfile = "NoProfile";
        public const string Profile = "Profile";

        [Parameter(Position = 0, ParameterSetName = Profile, Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string PasswordProfileName { get; set; }

        [Parameter(Position = 0, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter UpperCase { get; set; }

        [Parameter(Position = 1, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter LowerCase { get; set; }

        [Parameter(Position = 2, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter Digits { get; set; }

        [Parameter(Position = 3, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter SpecialCharacters { get; set; }

        [Parameter(Position = 4, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter Minus { get; set; }

        [Parameter(Position = 5, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter UnderScore { get; set; }

        [Parameter(Position = 6, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter Space { get; set; }

        [Parameter(Position = 7, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter Brackets { get; set; }

        [Parameter(Position = 8, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter ExcludeLookALike { get; set; }

        [Parameter(Position = 9, ParameterSetName = NoProfile)]
        [ValidateNotNull]
        public SwitchParameter NoRepeatingCharacters { get; set; }

        [Parameter(Position = 10, ParameterSetName = NoProfile)]
        [ValidateNotNullOrEmpty]
        public string ExcludeCharacters { get; set; }

        [Parameter(Position = 11, ParameterSetName = NoProfile)]
        [ValidateNotNullOrEmpty]
        public int Length { get; set; }

        [Parameter(Position = 12, ParameterSetName = NoProfile)]
        [ValidateNotNullOrEmpty]
        public string SaveAs { get; set; }

        [SuppressMessage("ReSharper", "ArgumentsStyleOther")]
        public override void ExecuteCmdlet()
        {
            var passwordService = new PasswordService(KeePassProfile, this);
            if (string.IsNullOrEmpty(PasswordProfileName))
            {
                var profile = passwordService.NewKeePassPasswordProfile(
                    upperCase: UpperCase.ToBool(),
                    lowerCase: LowerCase.ToBool(),
                    digits: Digits.ToBool(),
                    specialCharacters: SpecialCharacters.ToBool(),
                    minus: Minus.ToBool(),
                    underScore: UnderScore.ToBool(),
                    space: Space.ToBool(),
                    brackets: Brackets.ToBool(),
                    excludeLookAlike: ExcludeLookALike.ToBool(),
                    noRepeatingCharacters: NoRepeatingCharacters.ToBool(),
                    excludeCharacters: ExcludeCharacters,
                    length: Length,
                    saveAs: SaveAs);
                var outPass = passwordService.GeneratePassword(profile);
                WriteObject(outPass);
            }
            else
            {
                var profile = passwordService.GetKeePassPasswordProfile(PasswordProfileName);
                if (profile == null)
                {
                    WriteExceptionError(new PSArgumentOutOfRangeException("PasswordProfileName", PasswordProfileName,
                        $"Could not find PwProfile with name {PasswordProfileName} in {KeePassProfile.GetDefaultPath()}"));
                }
                else
                {
                    var outPass = passwordService.GeneratePassword(profile.ToPwProfile());
                    WriteObject(outPass);
                }
            }
        }
    }
}