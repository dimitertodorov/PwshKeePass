---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Remove-KeePassConfigurationFile

## SYNOPSIS
Clear out any .pwshKeePass configuration files in %USERPROFILE% or %HOME%.

## SYNTAX

```
Remove-KeePassConfigurationFile [-Force] [<CommonParameters>]
```

## DESCRIPTION
Clear out any .pwshKeePass configuration files in %USERPROFILE% or %HOME%.
This will reset any saved database profiles.

## EXAMPLES

### Example 1
```powershell
PS C:> Remove-KeePassConfigurationFile -Force
```

Remove configuration forcefully.

## PARAMETERS

### -Force
Force Removal

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

### System.Object
## NOTES

## RELATED LINKS
