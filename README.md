# KeePass2 PowerShell Module (Binary)

Makes use of [KeePass2Core](https://github.com/Strangelovian/KeePass2Core)

Most of cmdlet design, tests inspired by [PoshKeePass](https://github.com/PSKeePass/PoShKeePass)

A binary PowerShell module implementation for interacting with KDBX files.
Started as a project for personal use since I use powershell on OSX and *NIX as well as Windows and needed access to my passwoord vault.
## Features

1. **Database Configuration Profiles** - Supports mutliple databases and authentication options.
2. **Getting, Creating, Updating, and Removing KeePass Entries and Groups** - All of these perform as much automatic database authentication as possible using the database configuration profile. 
3. **Generating KeePass Passwords** - Supports most character sets and advanced keepass options. Also supports creating password profiles that can be specified to create a new password with the same rule set.

## Getting Started

### Install
**NOTE:** Not released yet on PSGallery until stable release is reached.
#### Desktop Version
```powershell
Install-Module -Name PwshKeePass
```

#### PowerShell Core Version
```powershell
Install-Module -Name PwshKeePass.NetCore
```

## Versioning Notes
Due to issues with loading certain `netstandard2.0` libraries under .NET 4.7.2 there are two separate distributions.
*System.Drawing is not supported when loaded through netstandard2.0...*

### PwshKeePass
.NET 4.7.2 Compatible Module for interacting with .KDBX databases.

*Could be supported on lower .NET versions, but not tested.*

### PwshKeePass.NetCore
Same as above. But compiled for netstandard2.0 

### Building
#### Requirements
* PowerShell Desktop/Core required depending on what version you are building.
* .NET SDK > 2.2
* Publishing currently only works from Desktop Powershell on Windows (Problem with Nuget provider on OSX/LINUX).

Builds can be triggered from either environment.

PowerShell Dependencies managed using [RamblingCookieMonster/PSDepend](https://github.com/RamblingCookieMonster/PSDepend)

#### Install Powershell Dependencies
```
Install-Module PSDepend -Scope CurrentUser
Invoke-PSDepend .\requirements.psd1 -Install -Target CurrentUser -Force
```

#### Pester Testing
```
.\psake_build.ps1 pester -parameters @{"target"="core"}
```

Or with Desktop Powershell
```
.\psake_build.ps1 pester -parameters @{"target"="framework"}
```
Note: 

## Contributing

* If you are insterested in fixing issues and contributing directly to the code base, please contact me as I get this started.
* If you come across a bug or have a feature request feel free to create an issue with the appropriate label.
* Currently looking for any help to get this codebase to a stable release.
* Help with documentation and build pipeline is much appreciated.

## Shout-Outs

* [PoShKeePass](https://github.com/PSKeePass/PoShKeePass) 
## License

Copyright (c) [Dimiter Todorov](https://github.com/dimitertodorov)

Licensed under the [Apache 2.0](https://github.com/dimitertodorov/PwshKeePass/blob/master/LICENSE.txt) License.