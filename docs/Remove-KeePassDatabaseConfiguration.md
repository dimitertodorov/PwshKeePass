---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Remove-KeePassDatabaseConfiguration

## SYNOPSIS
Remove a database profile. This will remove it from memory as well as from the configuration file.

## SYNTAX

```
Remove-KeePassDatabaseConfiguration [-DatabaseProfileName] <String> [<CommonParameters>]
```

## DESCRIPTION
Remove a database profile. This will remove it from memory as well as from the configuration file.

## EXAMPLES

### Example 1
```powershell
PS C:> Remove-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile'
```

Remove a configuration by name.

### Example 2
```powershell
PS C:> Get-KeePassDatabaseConfiguration -DatabaseProfileName 'TEST' | Remove-KeePassDatabaseConfiguration
```

Remove a configuration from the pipeline.

## PARAMETERS

### -DatabaseProfileName
DatabaseProfileName

```yaml
Type: String
Parameter Sets: (All)
Aliases: Name

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see about_CommonParameters (http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### System.String

## OUTPUTS

### System.Object
## NOTES

## RELATED LINKS
