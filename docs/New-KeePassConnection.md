---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# New-KeePassConnection

## SYNOPSIS
Initiates an in-memory connection to a KeePass database. 

## SYNTAX

### Key
```
New-KeePassConnection -DatabasePath <String> -KeyPath <String> [<CommonParameters>]
```

### Master
```
New-KeePassConnection -DatabasePath <String> -MasterKey <SecureString> [<CommonParameters>]
```

### KeyAndMaster
```
New-KeePassConnection -DatabasePath <String> -KeyPath <String> -MasterKey <SecureString> [<CommonParameters>]
```

### DatabaseProfileParameterSet
```
New-KeePassConnection -DatabaseProfileName <String> [<CommonParameters>]
```

## DESCRIPTION
Initiates an in-memory connection to a KeePass database. 

Does not persist across sessions.

## EXAMPLES

### Example 1
```powershell
PS C:> $conn = New-KeePassConnection -DatabaseProfileName 'SampleProfile' -DatabasePath `
    "MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force )
```

Create a connection for a database with just Master Key (Password) protection.

### Example 2
```powershell
PS C:> $conn = New-KeePassConnection -DatabaseProfileName 'KeyFileTest' `
    -DatabasePath "KeyFile.kdbx" -KeyPath "KeyFile.key"
```

Create a connection for a database with private key protection.

### Example 3
```powershell
PS C:> $conn = New-KeePassConnection -DatabaseProfileName 'KeyFileAndMasterKeyTestPassThru' -DatabasePath "KeyAndMaster.kdbx"  `
    -KeyPath "KeyAndMaster.key" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force )
```

Create a connection for a database with private key and master password protection.

## PARAMETERS

### -DatabasePath
DatabasePath

```yaml
Type: String
Parameter Sets: Key, Master, KeyAndMaster
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DatabaseProfileName
Master Key

```yaml
Type: String
Parameter Sets: DatabaseProfileParameterSet
Aliases:

Required: True
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
Position: Named
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

### KeePassLib.PwDatabase

## NOTES

## RELATED LINKS
