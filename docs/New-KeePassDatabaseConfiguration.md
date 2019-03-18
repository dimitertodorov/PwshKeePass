---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# New-KeePassDatabaseConfiguration

## SYNOPSIS
Setup a new Database configuration. Specify `-Save` to persist configuration to disk.

## SYNTAX

### Key
```
New-KeePassDatabaseConfiguration -DatabaseProfileName <String> [-DatabasePath] <String> [-KeyPath] <String>
 [-Default] [-Save] [-PassThru] [-Force] [<CommonParameters>]
```

### Master
```
New-KeePassDatabaseConfiguration -DatabaseProfileName <String> [-DatabasePath] <String>
 [-MasterKey] <SecureString> [-Default] [-Save] [-PassThru] [-Force] [<CommonParameters>]
```

### KeyAndMaster
```
New-KeePassDatabaseConfiguration -DatabaseProfileName <String> [-DatabasePath] <String> [-KeyPath] <String>
 [-MasterKey] <SecureString> [-Default] [-Save] [-PassThru] [-Force] [<CommonParameters>]
```

## DESCRIPTION
This is the primary command for adding profiles to your .pwshKeePass profile.

Specify `-Save` to persist the connection to disk.

Master Keys are not persisted accross session, but stay in memory for the duration of your session.

## EXAMPLES

### Example 1
```powershell
PS C:> New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath `
    "MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save
```

Create a persisted configuration for a database with just Master Key (Password) protection.

### Example 2
```powershell
PS C:> New-KeePassDatabaseConfiguration -DatabaseProfileName 'KeyFileTest' `
    -DatabasePath "KeyFile.kdbx" -KeyPath "KeyFile.key" -Save
```

Create a persisted configuration for a database with private key protection.
This is persisted across sessions and will not prompt for master key after saving.

### Example 3
```powershell
PS C:> $configuration = New-KeePassDatabaseConfiguration -DatabaseProfileName 'KeyFileAndMasterKeyTestPassThru' -DatabasePath "KeyAndMaster.kdbx"  `
-KeyPath "KeyAndMaster.key" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save  -PassThru
```

Create a persisted configuration for a database with private key and master password protection.

## PARAMETERS

### -DatabasePath
KeyPath

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DatabaseProfileName
DatabaseProfileName

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Default
Set as Default DB

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Force
Force Configuration.
(This will OverWrite existing configuration)

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeyPath
KeyPath

```yaml
Type: String
Parameter Sets: Key, KeyAndMaster
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MasterKey
Master Key

```yaml
Type: SecureString
Parameter Sets: Master, KeyAndMaster
Aliases:

Required: True
Position: 4
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PassThru
PassThru

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Save
Save DB Configuration to Disk after loading.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
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
