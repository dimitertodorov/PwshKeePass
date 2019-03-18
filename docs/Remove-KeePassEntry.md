---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Remove-KeePassEntry

## SYNOPSIS
Remove a KeePass Entry.

## SYNTAX

```
Remove-KeePassEntry -KeePassEntry <PSObject> [-NoRecycle] [-Force] [-Connection <PwDatabase>]
 [-DatabaseProfileName <String>] [-WhatIf] [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Remove a KeePass Entry.

## EXAMPLES

### Example 1
```powershell
PS C:> $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' `
    -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test1' }
PS C:> Remove-KeePassEntry -KeePassEntry $KeePassEntry -DatabaseProfileName 'SampleProfile' -Force
```

Remove KeePass entry and recycle it.

### Example 1
```powershell
PS C:> $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' `
    -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test1' }
PS C:> Remove-KeePassEntry -KeePassEntry $KeePassEntry -DatabaseProfileName 'SampleProfile' `
    -NoRecycle -Force
```

Remove KeePass entry without recycling.

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
Force

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

### -KeePassEntry
Entry to Remove.
Can be either PwEntry or PSKeePassEntry

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
NoRecycle

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

### System.Object
## NOTES

## RELATED LINKS
