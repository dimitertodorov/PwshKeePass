if($PSVersionTable.PSEdition -eq "Desktop"){
    $Global:ModuleName = "PwshKeePass"    
}else{
    $Global:ModuleName = "PwshKeePass.NetCore"
}
Import-Module "$PSScriptRoot\..\$($Global:ModuleName).psd1" -force -ErrorAction Stop


$Global:KPTestBackupDatabaseFile = "$( $PSScriptRoot )/Includes/Backup\SampleDb.kdbx"
$Global:KPTestDatabaseFile = "$( $PSScriptRoot )/Includes/SampleDb.kdbx"
$Global:KPTestKeyPath = "$( $PSScriptRoot )/Includes/SampleDb.key"

Copy-Item -Path $Global:KPTestBackupDatabaseFile -Destination $Global:KPTestDatabaseFile -Force


$WarningPreference = 'SilentlyContinue'
$ErrorActionPreference = 'Stop'