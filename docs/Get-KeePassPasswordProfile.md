---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Get-KeePassPasswordProfile

## SYNOPSIS
Get a saved password profile from local profile.

## SYNTAX

```
Get-KeePassPasswordProfile [-PasswordProfileName] <String> [<CommonParameters>]
```

## DESCRIPTION
Get a saved password profile from local profile.

## EXAMPLES

### Example 1
```powershell
PS C:> $password = New-KeePassPassword -UpperCase -LowerCase -Digits -SpecialCharacters -Minus -UnderScore -Space -Brackets -Length 20 -SaveAs 'Basic20'
PS C:> Get-KeePassPasswordProfile -PasswordProfileName 'Basic20'
    Name                  : Basic20
    CharacterSet          : ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!"#$%&'*+,./:;=?@\^`|~-_ []{}()<>
    ExcludeLookAlike      : False
    NoRepeatingCharacters : False
    ExcludeCharacters     :
    Length                : 20
```

Create and retrieve a password profile.

## PARAMETERS

### -PasswordProfileName
PasswordProfileName

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
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

### PwshKeePass.Profile.PwProfileSettings

## NOTES

## RELATED LINKS
