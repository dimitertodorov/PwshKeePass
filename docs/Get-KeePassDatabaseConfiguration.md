---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Get-KeePassDatabaseConfiguration

## SYNOPSIS
Get a database configuration from the local profile or memory.

## SYNTAX

```
Get-KeePassDatabaseConfiguration [[-DatabaseProfileName] <String>] [<CommonParameters>]
```

## DESCRIPTION
Grab a database configuration from the local profile or memory.

## EXAMPLES

### Example 1
```powershell
PS C:> Get-KeePassDatabaseConfiguration -DatabaseProfileName "TEST"
       
       
       Name               : TEST
       AuthenticationType : KeyAndMaster
       DatabasePath       : C:\doc\PwshKeePass\SampleDb.kdbx
       KeyPath            : C:\doc\PwshKeePass\SampleDb.key
       UseMasterKey       : True
       UseNetworkAccount  : False
       MasterKey          : System.Security.SecureString
       PwDatabase         : KeePassLib.PwDatabase
```

Get a database configuration from profile.

## PARAMETERS

### -DatabaseProfileName
DatabaseProfileName

```yaml
Type: String
Parameter Sets: (All)
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

### PwshKeePass.Profile.DatabaseProfile

## NOTES

## RELATED LINKS
