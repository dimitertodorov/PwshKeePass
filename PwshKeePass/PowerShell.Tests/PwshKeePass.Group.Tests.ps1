. "$( $PSScriptRoot )\PwshKeePassTest.common.ps1"

Describe "New-KeePassGroup - UnitTest" -Tag UnitTest {

    Context "Example 1: Creates a New KeePass Group." {

        Remove-KeePassConfigurationFile -Force

        # It "Example 1.1: Creates a New KeePass Group - Invalid - No Profile" {
        #     { New-KeePassGroup -KeePassGroupParentPath 'database' -KeePassGroupName 'test' } | Should Throw 'InvalidKeePassConfiguration : No KeePass Configuration has been created.'
        # }

        ## Create Profile
        New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force ) -Save -Force

        ## Reset Test DB
        Remove-Item -Path $Global:KPTestDatabaseFile -Force
        Copy-Item -Path $Global:KPTestBackupDatabaseFile -Destination "$($PSScriptRoot)\Includes\"

        It "Example 1.2: Creates a New KeePass Group - Valid" {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test1' -DatabaseProfileName 'SampleProfile' | Should Be $null
        }

        It "Example 1.3: Creates a New KeePass Group - Valid - PassThru" {

            $PassThruResult = New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test2PassThru' -DatabaseProfileName 'SampleProfile' -PassThru

            $PassThruResult.KPGroup | Should BeOfType KeePassLib.PwGroup
            $PassThruResult.KPGroup.ParentGroup.Name | Should Be 'SampleDb'
            $PassThruResult.KPGroup.Name | Should Be 'test2PassThru'
        }

        It "Example 1.4: Creates a New KeePass Entry - Invalid - Group Path does not Exist" {
            { New-KeePassGroup -KeePassGroupParentPath 'BadPath' -KeePassGroupName 'test3' -DatabaseProfileName 'SampleProfile' } | Should Throw
        }

        It "Example 1.5: Creates a New KeePass Group - Valid - PassThru - Icon" {

            $PassThruResult = New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test4PassThru' -DatabaseProfileName 'SampleProfile' -IconName 'Clock' -PassThru

            $PassThruResult.KPGroup | Should BeOfType KeePassLib.PwGroup
            $PassThruResult.KPGroup.ParentGroup.Name | Should Be 'SampleDb'
            $PassThruResult.KPGroup.Name | Should Be 'test4PassThru'
            $PassThruResult.KPGroup.IconId | Should Be 'Clock'
        }
    }
    Remove-KeePassConfigurationFile -Force
}

Describe "Get-KeePassGroup - UnitTest" -Tag UnitTest {

    Context "Example 1: Gets KeePass Groups." {

        Remove-KeePassConfigurationFile -Force



        ## Create Profile
        New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force ) -Save -Force

        ## Reset Test DB
        Remove-Item -Path $Global:KPTestDatabaseFile -Force
        Copy-Item -Path $Global:KPTestBackupDatabaseFile -Destination "$( $PSScriptRoot )\Includes\"

        It "Example 1.1: Gets All KeePass Groups - Invalid - No Database Configuration Profiles." {
            { Get-KeePassGroup -FullPath 'SampleDb/Bad' } | Should Throw 'InvalidKeePassConnection : No KeePass -Connection passed, and no -DatabaseProfileName provided.'
        }

        It "Example 1.2 Gets All KeePass Groups - Valid" {
            $ResultGroups = Get-KeePassGroup -DatabaseProfileName SampleProfile
            $ResultGroups.Count | Should Be 6
        }
        It "Example 1.3: Gets a KeePass Group - Valid" {

            $ResultGroups = Get-KeePassGroup -DatabaseProfileName SampleProfile -FullPath 'SampleDb/12345'
            $ResultGroups.Name | Should Be '12345'
            $ResultGroups.ParentGroup | Should Be 'SampleDb'
        }

        It "Example 1.4: Gets a KeePass Group - Invalid - Bad Path" {

            { Get-KeePassEntry -DatabaseProfileName SampleProfile -KeePassEntryGroupPath 'SampleDb/BadPath213' } | Should Throw
        }

    }

    Remove-KeePassConfigurationFile -Force
}

Describe "Update-KeePassGroup - UnitTest" -Tag UnitTest {

    Context "Example 1: Updates a KeePass Group." {

        Remove-KeePassConfigurationFile -Force

        # It "Example 1.1: Updates a KeePass Group - Invalid - No Profile" {
        #     { Update-KeePassGroup -KeePassGroup $( New-Object KeePassLib.PwGroup($true, $true)) -Force } | Should Throw 'InvalidKeePassConfiguration : No KeePass Configuration has been created.'
        # }

        ## Create Profile
        New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force ) -Save -Force

        ## Reset Test DB
        Remove-Item -Path $Global:KPTestDatabaseFile -Force
        Copy-Item -Path $Global:KPTestBackupDatabaseFile -Destination "$($PSScriptRoot)\Includes\"

        It "Example 1.2: Updates a KeePass Group - Valid - Name" {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test1' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test1'
            $KeePassGroup.Name | Should Be 'test1'
            Update-KeePassGroup -KeePassGroup $KeePassGroup -GroupName 'Test1Update' -DatabaseProfileName 'SampleProfile' -Force | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/Test1Update'
            $KeePassGroup.Name | Should Be 'Test1Update'
        }

        It "Example 1.3: Updates a KeePass Group - Valid - Name" {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test2' -DatabaseProfileName 'SampleProfile' | Should Be $null
            Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test2' |
                    Update-KeePassGroup  -DatabaseProfileName 'SampleProfile' -GroupName 'Test2Update' -Force | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/Test2Update'
            $KeePassGroup.Name | Should Be 'Test2Update'
        }

        It "Example 1.4: Updates a KeePass Group - Valid - Name" {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test3' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test3'
            $KeePassGroup.Name | Should Be 'test3'
            $KeePassGroup = Update-KeePassGroup -KeePassGroup $KeePassGroup -GroupName 'Test3Update' -DatabaseProfileName 'SampleProfile' -Force -PassThru
            $KeePassGroup.Name | Should Be 'Test3Update'
            $KeePassGroup.KPGroup.ParentGroup.Name | Should be 'SampleDb'
        }

        It "Example 1.5: Updates a KeePass Group - Valid - ParentGroup - Pipeline" {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test4' -DatabaseProfileName 'SampleProfile' | Should Be $null
            Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test4' |
                    Update-KeePassGroup -KeePassParentGroupPath 'SampleDb/12345' -DatabaseProfileName 'SampleProfile' -Force | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/12345/test4'
            $KeePassGroup.Name | Should Be 'test4'
            $KeePassGroup.KPGroup.ParentGroup.Name | Should be '12345'
        }

        It "Example 1.6: Updates a KeePass Group - Invalid - ParentGroup - BadPath" {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test5' -DatabaseProfileName 'SampleProfile' | Should Be $null
            { Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test5' |
                    Update-KeePassGroup -KeePassParentGroupPath 'SampleDb/BadPath' -DatabaseProfileName 'SampleProfile' -Force } | Should Throw
        }

        It "Example 1.7: Updates a KeePass Group - Valid - Name - PassThru - Icon" {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test6' -DatabaseProfileName 'SampleProfile' -IconName "Folder" | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test6'
            $KeePassGroup.Name | Should Be 'test6'
            $KeePassGroup.IconId | Should Be 'Folder'
            $KeePassGroup = Update-KeePassGroup -KeePassGroup $KeePassGroup -GroupName 'Test6Update' -DatabaseProfileName 'SampleProfile' -IconName 'Clock' -Force -PassThru
            $KeePassGroup.Name | Should Be 'Test6Update'
            $KeePassGroup.IconId | Should Be 'Clock'
            $KeePassGroup.KPGroup.ParentGroup.Name | Should be 'SampleDb'
        }
    }

    Remove-KeePassConfigurationFile -Force
}

Describe "Remove-KeePassGroup - UnitTest" -Tag UnitTest {
    Remove-KeePassConfigurationFile -Force

    Context "Example 1: Remove a KeePass Group" {

        # It "Example 1.1: Removes a KeePass Group - Invalid - No Profile" {
        #     { Remove-KeePassGroup -KeePassGroup $( New-Object KeePassLib.PwGroup($true, $true)) }| Should Throw 'InvalidKeePassConfiguration : No KeePass Configuration has been created.'
        # }

        ## Create Profile
        New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force ) -Save -Force

        ## Reset Test DB
        Remove-Item -Path $Global:KPTestDatabaseFile -Force
        Copy-Item -Path $Global:KPTestBackupDatabaseFile -Destination "$($PSScriptRoot)\Includes\"


        It "Example 1.2: Removes a KeePass Group - Valid " {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test1' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test1'
            $KeePassGroup.Name | Should Be 'test1'
            Remove-KeePassGroup -KeePassGroup $KeePassGroup -DatabaseProfileName 'SampleProfile' -Force | Should Be $null
            $Check = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/Recycle Bin/test1'
            $Check.Name | Should Be 'test1'
        }

        It "Example 1.3: Removes a KeePass Group - Valid - NoRecycle " {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test2' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test2'
            $KeePassGroup.Name | Should Be 'test2'
            Remove-KeePassGroup -KeePassGroup $KeePassGroup -DatabaseProfileName 'SampleProfile' -NoRecycle -Force | Should Be $null
            { Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test2' } | Should Throw
            { Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/Recycle Bin/test2' } | Should Throw
        }

        It "Example 1.4: Removes a KeePass Group - Valid - Pipeline - AsPlainText" {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test3' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test3'
            $KeePassGroup.Name | Should Be 'test3'
            $KeePassGroup | Remove-KeePassGroup -DatabaseProfileName SampleProfile -Force | Should Be $null
            Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/Recycle Bin/test3' | Should Not Be $null
        }

        It "Example 1.4: Removes a KeePass Group - Valid - Pipeline - PwGroup" {
            New-KeePassGroup -KeePassGroupParentPath 'SampleDb' -KeePassGroupName 'test4' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassGroup = Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/test4'
            $KeePassGroup.Name | Should Be 'test4'
            $KeePassGroup | Remove-KeePassGroup -DatabaseProfileName SampleProfile -Force | Should Be $null
            { Get-KeePassGroup -DatabaseProfileName SampleProfile -KeePassGroupPath 'SampleDb/RecycleBin/test4' } | Should Throw
        }
    }
    Remove-KeePassConfigurationFile -Force
}
