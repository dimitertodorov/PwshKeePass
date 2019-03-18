---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Update-KeePassEntry

## SYNOPSIS
Update a KeePass entry.

## SYNTAX

```
Update-KeePassEntry -KeePassEntry <PSObject> [-KeePassEntryGroupPath <String>] [-Title <String>]
 [-UserName <String>] [-KeePassPassword <PSObject>] [-Notes <String>] [-Url <String>] [-IconName <String>]
 [-PassThru] [-Connection <PwDatabase>] [-DatabaseProfileName <String>] [-WhatIf] [-Confirm]
 [<CommonParameters>]
```

## DESCRIPTION
Update a KeePass entry.

## EXAMPLES

### Example 1
```powershell
PS C:> $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' `
    -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test1' }
PS C:> Update-KeePassEntry -KeePassEntry $KeePassEntry -KeePassEntryGroupPath 'SampleDb' `
    -title 'UpdateTest1' -UserName 'UpdateTestUser' -Notes 'UpdateTestNotes' -URL 'http://UpdateURL.Test.com' -DatabaseProfileName 'SampleProfile'
```

Update a KeePass entry with specified options.

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

### -IconName
IconName

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

### -KeePassEntry
Entry to Update.
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

### -KeePassEntryGroupPath
FullPath

```yaml
Type: String
Parameter Sets: (All)
Aliases: FullPath

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeePassPassword
KeePassPassword

```yaml
Type: PSObject
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Notes
Notes

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

### -Url
Url

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

### PwshKeePass.Model.PSKeePassEntry

## NOTES

## RELATED LINKS
