. "$( $PSScriptRoot )\PwshKeePassTest.common.ps1"

Describe "New-KeePassPassword - UnitTest" -Tag UnitTest {

    Context "Example 1: Generate a new KeePass Password - Options" {

        It "Example 1.1: New Password using all basic options - Valid" {
            New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Minus -UnderScore -Space -Brackets -Length 20 -SaveAs "SADOIJSADASD" | Should BeOfType KeePassLib.Security.ProtectedString
        }

        It "Example 1.2: New Password using all basic options + ExcludeLookALike - Valid" {
            New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Minus -UnderScore -Space -Brackets -Length 20 -ExcludeLookALike | Should BeOfType KeePassLib.Security.ProtectedString
        }

        It "Example 1.3: New Password using all basic options + NoRepeatingCharacters - Valid" {
            New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Minus -UnderScore -Space -Brackets -Length 20 -NoRepeatingCharacters | Should BeOfType KeePassLib.Security.ProtectedString
        }

        It "Example 1.4: New Password using some basic options + NoRepeatingCharacters - Invalid" {
            { New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Length 85 -NoRepeatingCharacters } | Should Throw 'Unable to generate a password with the specified options.'
        }

        It "Example 1.5: New Password using all basic options + ExcludedCharactes - Valid" {
            $SecurePass = New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Minus -UnderScore -Space -Brackets -Length 70 -ExcludeCharacters '1,],-a'

            $SecurePass |  Should BeOfType KeePassLib.Security.ProtectedString

            $SecurePass.ReadString() | Should Not Match ([regex]::Escape("^.*[1\]-a].*$"))
        }
    }
    Context "Example 2: Generate a new KeePass Password - Options - SaveAs" {

        Remove-KeePassConfigurationFile -Force

        It "Example 2.1: New Password using all basic options - Valid" {
            New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Minus -UnderScore -Space -Brackets -Length 20 -SaveAs 'Basic20' | Should BeOfType KeePassLib.Security.ProtectedString

            $PassProfile = Get-KeePassPasswordProfile -PasswordProfileName 'Basic20'
            $PassProfile.Name | Should Be 'Basic20'
            $PassProfile.CharacterSet | Should Be 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!"#$%&''*+,./:;=?@\^`|~-_ []{}()<>'
            $PassProfile.ExcludeLookAlike | Should Be 'False'
            $PassProfile.NoRepeatingCharacters | Should Be 'False'
            $PassProfile.ExcludeCharacters | Should Be ''
            $PassProfile.Length | Should Be 20
        }

        It "Example 2.2: New Password using all basic options + ExcludeLookALike - Valid" {
            New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Minus -UnderScore -Space -Brackets -Length 20 -ExcludeLookALike -SaveAs 'BasicNoLookAlike20' | Should BeOfType KeePassLib.Security.ProtectedString

            $PassProfile = Get-KeePassPasswordProfile -PasswordProfileName 'BasicNoLookAlike20'
            $PassProfile.Name | Should Be 'BasicNoLookAlike20'
            $PassProfile.CharacterSet | Should Be 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!"#$%&''*+,./:;=?@\^`|~-_ []{}()<>'
            $PassProfile.ExcludeLookAlike | Should Be 'True'
            $PassProfile.NoRepeatingCharacters | Should Be 'False'
            $PassProfile.ExcludeCharacters | Should Be ''
            $PassProfile.Length | Should Be 20
        }

        It "Example 2.3: New Password using all basic options + NoRepeatingCharacters - Valid" {
            New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Minus -UnderScore -Space -Brackets -Length 20 -NoRepeatingCharacters -SaveAs 'BasicNoRepeat20' | Should BeOfType KeePassLib.Security.ProtectedString

            $PassProfile = Get-KeePassPasswordProfile -PasswordProfileName 'BasicNoRepeat20'
            $PassProfile.Name | Should Be 'BasicNoRepeat20'
            $PassProfile.CharacterSet | Should Be 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!"#$%&''*+,./:;=?@\^`|~-_ []{}()<>'
            $PassProfile.ExcludeLookAlike | Should Be 'False'
            $PassProfile.NoRepeatingCharacters | Should Be 'True'
            $PassProfile.ExcludeCharacters | Should Be ''
            $PassProfile.Length | Should Be 20
        }

        It "Example 2.4: New Password using some basic options + NoRepeatingCharacters - Invalid" {
            { New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Length 85 -NoRepeatingCharacters -SaveAs 'BasicNoRepeatInvalid' } | Should Throw 'Unable to generate a password with the specified options.'

            Get-KeePassPasswordProfile -PasswordProfileName 'BasicNoRepeatInvalid' | Should Be $null
        }

        It "Example 2.5: New Password using all basic options + ExcludedCharactes - Valid" {
            $SecurePass = New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Minus -UnderScore -Space -Brackets -Length 70 -ExcludeCharacters '1,],-a' -SaveAs 'BasicExcudle1]-a'

            $SecurePass | Should BeOfType KeePassLib.Security.ProtectedString
            $SecurePass.ReadString() | Should Not Match ([regex]::Escape("^.*[1\]-a].*$"))

            $PassProfile = Get-KeePassPasswordProfile -PasswordProfileName 'BasicExcudle1]-a'
            $PassProfile.Name | Should Be 'BasicExcudle1]-a'
            $PassProfile.CharacterSet | Should Be 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!"#$%&''*+,./:;=?@\^`|~-_ []{}()<>'
            $PassProfile.ExcludeLookAlike | Should Be 'False'
            $PassProfile.NoRepeatingCharacters | Should Be 'False'
            $PassProfile.ExcludeCharacters | Should Be '1,],-a'
            $PassProfile.Length | Should Be 70
        }
    }

    Context "Example 3: Generate a new KeePass Password - Profile" {

        It "Example 3.1: New Password using Profile Basic20 - Valid" {
            New-KeePassPassword -PasswordProfileName 'Basic20' | Should BeOfType KeePassLib.Security.ProtectedString
        }

        It "Example 3.2: New Password using Profile BasicNoLookAlike20 - Valid" {
            New-KeePassPassword -PasswordProfileName 'BasicNoLookAlike20' | Should BeOfType KeePassLib.Security.ProtectedString
        }

        It "Example 3.3: New Password using Profile BasicNoRepeat20 - Valid" {
            New-KeePassPassword -PasswordProfileName 'BasicNoRepeat20' | Should BeOfType KeePassLib.Security.ProtectedString
        }

        It "Example 3.4: New Password using Profile BasicNoRepeatInvalid - Invalid - Does Not Exist" {
            { New-KeePassPassword -PasswordProfileName 'BasicNoRepeatInvalid' } | Should Throw
        }

        It "Example 3.5: New Password using Profile BasicExcudle1]-a - Valid" {
            $SecurePass = New-KeePassPassword -PasswordProfileName 'BasicExcudle1]-a'

            $SecurePass | Should BeOfType KeePassLib.Security.ProtectedString
            $SecurePass.ReadString() | Should Not Match ([regex]::Escape("^.*[1\]-a].*$"))
        }
    }
}