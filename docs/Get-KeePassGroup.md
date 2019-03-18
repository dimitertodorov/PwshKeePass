---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Get-KeePassGroup

## SYNOPSIS
Get a list of KeePass groups from the target database.

## SYNTAX

```
Get-KeePassGroup [-FullPath <String>] [-Connection <PwDatabase>] [-DatabaseProfileName <String>]
 [<CommonParameters>]
```

## DESCRIPTION
Get a list of KeePass groups from the target database.

## EXAMPLES

### Example 1
```powershell
PS C:> Get-KeePassGroup -DatabaseProfileName TEST
```

Get all groups in Database

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

### -FullPath
Full Path

```yaml
Type: String
Parameter Sets: (All)
Aliases: KeePassGroupPath

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

### PwshKeePass.Model.PSKeePassGroup

## NOTES

## RELATED LINKS
