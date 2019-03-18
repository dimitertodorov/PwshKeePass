---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Remove-KeePassGroup

## SYNOPSIS
Remove a KeePass group and all its' entries from a KeePass DB.

## SYNTAX

```
Remove-KeePassGroup -KeePassGroup <PSObject> [-NoRecycle] [-Force] [-Connection <PwDatabase>]
 [-DatabaseProfileName <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Remove a KeePass group and all its' entries from a KeePass DB.

If -NoRecycle is specified, the group is removed permantently. Otherwise it will go into the "Recycle Bin".

## EXAMPLES

### Example 1
```powershell
PS C:> $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test3'
PS C:> Remove-KeePassGroup -KeePassGroup $KeePassGroup  -DatabaseProfileName SampleProfile -Force -NoRecycle
```

Permanently remove a group.

## PARAMETERS

### -Confirm
Prompts you for confirmation before running the cmdlet.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: cf

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

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

### -Force
Specify this option to forcefully delete the Group.

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

### -KeePassGroup
Group to Remove.
Can be either PwGroup or PSKeePassGroup

```yaml
Type: PSObject
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -NoRecycle
Specify this option to Permanently delete the Group and not recycle it.

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

### -WhatIf
Shows what would happen if the cmdlet runs.
The cmdlet is not run.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases: wi

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.Management.Automation.PSObject

## OUTPUTS

### null

## NOTES

## RELATED LINKS
