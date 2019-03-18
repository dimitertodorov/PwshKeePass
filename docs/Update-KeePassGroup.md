---
external help file: PwshKeePass.dll-Help.xml
Module Name: PwshKeePass
online version:
schema: 2.0.0
---

# Update-KeePassGroup

## SYNOPSIS
Update a KeePass group. 

## SYNTAX

```
Update-KeePassGroup -KeePassGroup <PSObject> [-KeePassParentGroupPath <String>] [-KeePassGroupName <String>]
 [-IconName <PwIcon>] [-PassThru] [-Force] [-Connection <PwDatabase>] [-DatabaseProfileName <String>] [-WhatIf]
 [-Confirm] [<CommonParameters>]
```

## DESCRIPTION
Update a KeePass group. If group is moved, its' UUID will change.

## EXAMPLES

### Example 1
```powershell
PS C:> $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test3'
PS C:> Update-KeePassGroup -KeePassGroup $KeePassGroup -GroupName 'Test3Update' -DatabaseProfileName 'SampleProfile' -Force
```

Update a group's name with PassThru.

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

### -IconName
IconName

```yaml
Type: PwIcon
Parameter Sets: (All)
Aliases:
Accepted values: Key, World, Warning, NetworkServer, MarkedDirectory, UserCommunication, Parts, Notepad, WorldSocket, Identity, PaperReady, Digicam, IRCommunication, MultiKeys, Energy, Scanner, WorldStar, CDRom, Monitor, EMail, Configuration, ClipboardReady, PaperNew, Screen, EnergyCareful, EMailBox, Disk, Drive, PaperQ, TerminalEncrypted, Console, Printer, ProgramIcons, Run, Settings, WorldComputer, Archive, Homebanking, DriveWindows, Clock, EMailSearch, PaperFlag, Memory, TrashBin, Note, Expired, Info, Package, Folder, FolderOpen, FolderPackage, LockOpen, PaperLocked, Checked, Pen, Thumbnail, Book, List, UserKey, Tool, Home, Star, Tux, Feather, Apple, Wiki, Money, Certificate, BlackBerry, Count

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeePassGroup
Group to Update.
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

### -KeePassGroupName
Specify the Name of the new KeePass Group.

```yaml
Type: String
Parameter Sets: (All)
Aliases: GroupName

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -KeePassParentGroupPath
Specify this parameter if you wish to move the group.
\n Notes: \n\t* Path Separator is the foward slash character '/'

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

### PwshKeePass.Model.PSKeePassGroup

## NOTES

## RELATED LINKS
