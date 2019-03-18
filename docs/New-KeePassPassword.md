---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# New-KeePassPassword

## SYNOPSIS
Generate a new Password.

## SYNTAX

### NoProfile (Default)
```
New-KeePassPassword [-UpperCase] [-LowerCase] [-Digits] [-SpecialCharacters] [-Minus] [-UnderScore] [-Space]
 [-Brackets] [-ExcludeLookALike] [-NoRepeatingCharacters] [[-ExcludeCharacters] <String>] [[-Length] <Int32>]
 [[-SaveAs] <String>] [<CommonParameters>]
```

### Profile
```
New-KeePassPassword [-PasswordProfileName] <String> [<CommonParameters>]
```

## DESCRIPTION
Generate a new password and optionally store as a local password profile.

## EXAMPLES

### Example 1
```powershell
PS C:\> $pass = New-KeePassPassword -UpperCase -LowerCase -Digits -Length 20
PS C:\> $pass.ReadString()
E6ltbm308sjEnJOtrdLX
```

Create a 20 character long password with specified flags.

### Example 2
```powershell
PS C:\> New-KeePassPassword -UpperCase -LowerCase -Digits -Length 20 -SaveAs 'Basic Password' | Out-Null
PS C:\> $pass = New-KeePassPassword -PasswordProfileName 'Basic Password'
PS C:\> $pass.ReadString()
E6ltbm308sjEnJOtrdLX
```

Create a password profile and generates a password based on it.

## PARAMETERS

### -Brackets
If Specified it will add Bracket Characters '()\<\>\[\]{}' to the character set used to generate the password.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 7
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Digits
If Specified it will add Digits to the character set used to generate the password.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludeCharacters
This will take a list of characters to Exclude, and remove them from the character set used to generate the password.

```yaml
Type: String
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 10
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ExcludeLookALike
If Specified it will exclude Characters that Look Similar from the character set used to generate the password.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 8
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Length
This will specify the length of the resulting password.
If not used it will use KeePass's Default Password Profile Length.

```yaml
Type: Int32
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 11
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -LowerCase
If Specified it will add LowerCase Letters to the character set used to generate the password.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Minus
If Specified it will add the Minus Symbol '-' to the character set used to generate the password.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -NoRepeatingCharacters
If Specified it will only allow Characters exist once in the password that is returned.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 9
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PasswordProfileName
DatabasePath

```yaml
Type: String
Parameter Sets: Profile
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SaveAs
Specify the name in which you wish to save the password configuration.

This will save all specified settings the .pwshKeePass file, which can then be specified later when generating a password to match the same settings.

```yaml
Type: String
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 12
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Space
If Specified it will add the Space Character ' ' to the character set used to generate the password.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 6
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SpecialCharacters
If Specified it will add Special Characters '!"#$%&''*+,./:;=?@\^\`|~' to the character set used to generate the password.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UnderScore
If Specified it will add the UnderScore Symbol '_' to the character set used to generate the password.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 5
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UpperCase
If Specified it will add UpperCase Letters to the character set used to generate the password.

```yaml
Type: SwitchParameter
Parameter Sets: NoProfile
Aliases:

Required: False
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### KeePassLib.Security.ProtectedString

## NOTES

## RELATED LINKS
