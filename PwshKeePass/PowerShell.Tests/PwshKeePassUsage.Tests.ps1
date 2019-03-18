. "$( $PSScriptRoot )\PwshKeePassTest.common.ps1"

Describe "Get-KeePassDatabaseConfiguration - UnitTest" -Tag UnitTest {
    Remove-KeePassConfigurationFile -Force

    Context "Example 1: Get a KeePass Database Configuration Profile" {

        It "Example 1.1: Get Database Configuration Profile - Valid - By Name" {
            New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save | Should Be $null

            $DatabaseConfiguration = Get-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile'

            $DatabaseConfiguration.Name | Should Be 'SampleProfile'
            $DatabaseConfiguration.DatabasePath | Should BeLike "*MasterKey.kdbx"
            $DatabaseConfiguration.KeyPath | Should Be $null
            $DatabaseConfiguration.UseMasterKey | Should Be 'True'
            $DatabaseConfiguration.AuthenticationType | Should Be 'Master'
        }

        It "Example 1.2: Get Database Configuration Profile - Valid - All" {
            $DatabaseConfiguration = Get-KeePassDatabaseConfiguration
            $DatabaseConfiguration.Name | Should Be 'SampleProfile'
            $DatabaseConfiguration.DatabasePath | Should BeLike "*MasterKey.kdbx"
            $DatabaseConfiguration.KeyPath | Should Be $null
            $DatabaseConfiguration.UseMasterKey | Should Be $true
            $DatabaseConfiguration.AuthenticationType | Should Be 'Master'
        }
    }

    Remove-KeePassConfigurationFile -Force
}




Describe "New-KeePassConnection - UnitTest" -Tag UnitTest {

    Context "Example 1: Open with PSKeePass Credential Object - KeyFile" {

        It "Example 1.1: Get KeePass Database Connection with KeyFile - Valid" {
            $KeePassConnection = New-KeePassConnection -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/KeyFile.kdbx" -KeyPath "$PSScriptRoot/Includes/AuthenticationDatabases/KeyFile.key"
            $KeePassConnection | Should BeOfType 'KeePassLib.PwDatabase'
            $KeePassConnection.IsOpen | Should Be $true
            $KeePassConnection.RootGroup.Name | Should Be 'KeyFile'
            $KeePassConnection.Close() | Should Be $null
            $KeePassConnection.IsOpen | Should Be $false
        }
    }

    Context "Example 2: Open with PSKeePass Credential Object - MasterKey" {

        It "Example 2.1: Get KeePass Database Connection with MasterKey - Valid" {
            $KeePassConnection = New-KeePassConnection -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force )
            $KeePassConnection | Should BeOfType 'KeePassLib.PwDatabase'
            $KeePassConnection.IsOpen | Should Be $true
            $KeePassConnection.RootGroup.Name | Should Be 'MasterKey'
            $KeePassConnection.Close() | Should Be $null
            $KeePassConnection.IsOpen | Should Be $false
        }
    }

    Context "Example 3: Open with PSKeePass Credential Object - MasterKey and KeyFile" {

        It "Example 3.1: Get KeePass Database Connection with KeyAndMaster - Valid" {
            $KeePassConnection = New-KeePassConnection -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force )
            $KeePassConnection | Should BeOfType 'KeePassLib.PwDatabase'
            $KeePassConnection.IsOpen | Should Be $true
            $KeePassConnection.RootGroup.Name | Should Be 'SampleDb'
            $KeePassConnection.Close() | Should Be $null
            $KeePassConnection.IsOpen | Should Be $false
        }
        It "Example 3.2: Get KeePass Database Connection with KeyAndMaster - Invalid Master File" {
            { New-KeePassConnection -Database "$KPTestDatabaseFile" -KeyPath "$KPTestKeyPath" -MasterKey $( ConvertTo-SecureString -String "ATestsssPassWord" -AsPlainText -Force ) } | Should Throw
        }
    }

}
Describe "Remove-KPConnection - UnitTest" -Tag UnitTest {

    Context "Example 1: Close an Open PSKeePass Database Connection" {
        Remove-KeePassConfigurationFile -Force
        It "Example 1.1: Closes a KeePass Database Connection" {
            $KeePassConnection = New-KeePassConnection -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force )
            $KeePassConnection.IsOpen | Should Be $true
            Remove-KeePassConnection -Connection $KeePassConnection | Should Be $null
            $KeePassConnection.IsOpen | Should Be $false
        }
    }
}

Describe "New-KeePassDatabaseConfiguration - UnitTest" -Tag UnitTest {

    Context "Example 1: Create a new KeePass Database Configuration Profile - KeyFile" {
        $ErrorActionPreference = "Stop"
        Remove-KeePassConfigurationFile -Force

        It "Example 1.1: Database Configuration Profile - KeyFile - Valid" {
            New-KeePassDatabaseConfiguration -DatabaseProfileName 'KeyFileTest' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/KeyFile.kdbx" -KeyPath "$PSScriptRoot/Includes/AuthenticationDatabases/KeyFile.key" -Save | Should Be $null
        }

        It "Example 1.2: Database Configuration Profile - KeyFile - Invalid Exists" {
            { New-KeePassDatabaseConfiguration -DatabaseProfileName 'KeyFileTest' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/KeyFile.kdbx" -KeyPath "$PSScriptRoot/Includes/AuthenticationDatabases/KeyFile.key" -Save } | Should Throw
        }

        It "Example 1.3: Database Configuration Profile - KeyFile - Valid with PassThru" {
            $DatabaseConfiguration = New-KeePassDatabaseConfiguration -DatabaseProfileName 'KeyFileTestPassThru' -DatabasePath "$( $PSScriptRoot )/Includes/AuthenticationDatabases/KeyFile.kdbx" -KeyPath "$( $PSScriptRoot )/Includes/AuthenticationDatabases/KeyFile.key" -PassThru

            $DatabaseConfiguration.Name | Should Be 'KeyFileTestPassThru'
            $DatabaseConfiguration.DatabasePath | Should BeLike "*KeyFile.kdbx"
            $DatabaseConfiguration.KeyPath | Should BeLike "*KeyFile.key"
            $DatabaseConfiguration.UseNetworkAccount | Should Be $false
            $DatabaseConfiguration.UseMasterKey | Should Be $false
        }
    }

    Context "Example 2: Create a new KeePass Database Configuration Profile - MasterKey" {


        Remove-KeePassConfigurationFile -Force
        $ErrorActionPreference = "Stop"

        It "Example 2.1: Database Configuration Profile - MasterKey - Valid" {
            New-KeePassDatabaseConfiguration -DatabaseProfileName 'MasterKeyTest' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save | Should Be $null
        }

        It "Example 2.2: Database Configuration Profile - MasterKey - Invalid Exists" {
            { New-KeePassDatabaseConfiguration -DatabaseProfileName 'MasterKeyTest' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save } | Should Throw
        }

        It "Example 2.3: Database Configuration Profile - MasterKey - Valid with PassThru" {
            $DatabaseConfiguration = New-KeePassDatabaseConfiguration -DatabaseProfileName 'MasterKeyTestPassThru' -DatabasePath "$( $PSScriptRoot )/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -PassThru

            $DatabaseConfiguration.Name | Should Be 'MasterKeyTestPassThru'
            $DatabaseConfiguration.DatabasePath | Should BeLike "*MasterKey.kdbx"
            $DatabaseConfiguration.KeyPath | Should Be $null
            $DatabaseConfiguration.UseNetworkAccount | Should Be 'False'
            $DatabaseConfiguration.UseMasterKey | Should Be 'True'
        }
    }

    Context "Example 3: Create a new KeePass Database Configuration Profile - KeyFile And MasterKey" {

        Remove-KeePassConfigurationFile -Force
        $ErrorActionPreference = "Stop"
        It "Example 3.1: Database Configuration Profile - KeyFile And MasterKey - Valid" {
            New-KeePassDatabaseConfiguration -DatabaseProfileName 'KeyFileAndMasterKeyTest' -DatabasePath "$( $PSScriptRoot )/Includes/AuthenticationDatabases/KeyAndMaster.kdbx" -KeyPath "$( $PSScriptRoot )/Includes/AuthenticationDatabases/KeyAndMaster.key" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save  | Should Be $null
        }

        It "Example 3.2: Database Configuration Profile - KeyFile And MasterKey - Invalid Exists" {
            { New-KeePassDatabaseConfiguration -DatabaseProfileName 'KeyFileAndMasterKeyTest' -DatabasePath "$( $PSScriptRoot )/Includes/AuthenticationDatabases/KeyAndMaster.kdbx" -KeyPath "$( $PSScriptRoot )/Includes/AuthenticationDatabases/KeyAndMaster.key" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save } | Should Throw
        }

        It "Example 3.3: Database Configuration Profile - KeyFile And MasterKey - Valid with PassThru" {
            $DatabaseConfiguration = New-KeePassDatabaseConfiguration -DatabaseProfileName 'KeyFileAndMasterKeyTestPassThru' -DatabasePath "$( $PSScriptRoot )/Includes/AuthenticationDatabases/KeyAndMaster.kdbx" -KeyPath "$( $PSScriptRoot )/Includes/AuthenticationDatabases/KeyAndMaster.key" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save  -PassThru

            $DatabaseConfiguration.Name | Should Be 'KeyFileAndMasterKeyTestPassThru'
            $DatabaseConfiguration.DatabasePath | Should BeLike "*KeyAndMaster.kdbx"
            $DatabaseConfiguration.KeyPath | Should BeLike "*KeyAndMaster.key"
            $DatabaseConfiguration.UseNetworkAccount | Should Be 'False'
            $DatabaseConfiguration.UseMasterKey | Should Be 'True'
            $DatabaseConfiguration.AuthenticationType | Should Be 'KeyAndMaster'
        }
    }
}



Describe "Remove-KeePassDatabaseConfiguration - UnitTest" -Tag UnitTest {
    Remove-KeePassConfigurationFile -Force
    Remove-KeePassConfigurationFile -Force

    Context "Example 1: Remove a KeePass Database Configuration Profile" {

        It "Example 1.1: Remove Database Configuration Profile - Valid - By Name" {
            New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save | Should Be $null

            Remove-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' | Should Be $null

            Get-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' | Should Be $null
        }

        It "Example 1.2: Remove Database Configuration Profile - Valid - By Name - Via Pipeline" {
            Remove-KeePassConfigurationFile -Force

            New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save | Should Be $null

            Get-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile'

            Get-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' | Remove-KeePassDatabaseConfiguration | Should Be $null

            Get-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' | Should Be $null
        }

        It "Example 1.3: Remove Database Configuration Profile - Valid - Multiple - Via Pipeline" {
            Remove-KeePassConfigurationFile -Force

            New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save | Should Be $null

            New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile1' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save | Should Be $null

            Get-KeePassDatabaseConfiguration | Remove-KeePassDatabaseConfiguration -Verbose | Should Be $null

            Get-KeePassDatabaseConfiguration | Should Be $null
        }
    }

}

#Describe "New-KeePassConnection - Profile - UnitTest" -Tag UnitTest {
#
#    Context "Example 1: Open with PSKeePass Credential Object - KeyFile - Profile" {
#        Remove-KeePassConfigurationFile -Force
#
#        It "Example 2.1: Get KeePass Database Connection with MasterKey from a Profile - Valid" {
#            New-KeePassDatabaseConfiguration -DatabaseProfileName 'MasterKeyTest' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $(ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force) -Save | Should Be $null
#            $KeePassConnection = New-KeePassConnection -DatabaseProfileName 'MasterKeyTest'
#            $KeePassConnection | Should BeOfType 'KeePassLib.PwDatabase'
#            $KeePassConnection.IsOpen | Should Be $true
#            $KeePassConnection.RootGroup.Name | Should Be 'MasterKey'
#            $KeePassConnection.Close() | Should Be $null
#            $KeePassConnection.IsOpen | Should Be $false
#        }
#    }
#
#
#    ## Holding off on Network Account Testing until I can script the creation of a database.
#}
#
#
#Remove-KeePassConfigurationFile -Force