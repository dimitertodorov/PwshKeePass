---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Get-KeePassEntry

## SYNOPSIS
Get a KeePass entry from the target database.

## SYNTAX

```
Get-KeePassEntry [-Title <String>] [-Uuid <String>] [-UserName <String>] [-KeePassEntryGroupPath <String>]
 [-Connection <PwDatabase>] [-DatabaseProfileName <String>] [<CommonParameters>]
```

## DESCRIPTION
Get a KeePass entry from the target database.

## EXAMPLES

### Example 1
```powershell
PS C:> Get-KeePassEntry -DatabaseProfileName SampleProfile
```

Get all entries in KDBX file. This includes recycled entries.

### Example 2
```powershell
PS C:> Get-KeePassEntry -Title 'bob.jones@example.com' -DatabaseProfileName 'TEST'
```

Get all entries in KDBX file, filtering by title.

## PARAMETERS

### -Connection
*Retrieve this from New-KeePassConnection
	his Parameter or -DatabaseProfileName is required in order to access your KeePass database.

```yaml
Type: PwDatabase
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -DatabaseProfileName
*This Parameter or -Connection is required in order to access your KeePass database.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeePassEntryGroupPath
KeePassEntryGroupPath

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Title
Title

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -UserName
UserName

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Uuid
UUid

```yaml
Type: String
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

### PwshKeePass.Model.PSKeePassEntry

## NOTES

## RELATED LINKS
