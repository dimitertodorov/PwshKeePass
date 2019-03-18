. "$( $PSScriptRoot )\PwshKeePassTest.common.ps1"


Describe "Remove-KeePassEntry - UnitTest" -Tag UnitTest {
    Remove-KeePassConfigurationFile -Force

    Context "Example 1: Remove a KeePass Entry" {

        # It "Example 1.1: Removes a KeePass Entry - Invalid - No Profile" {
        #     { Remove-KeePassEntry -KeePassEntry $( New-Object KeePassLib.PwEntry($true, $true)) }| Should Throw 'InvalidKeePassConfiguration : No KeePass Configuration has been created.'
        # }

        ## Create Profile
        New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force ) -Save -Force

        ## Reset Test DB
        Remove-Item -Path $Global:KPTestDatabaseFile -Force
        Copy-Item -Path $Global:KPTestBackupDatabaseFile -Destination "$($PSScriptRoot)\Includes\"


        It "Example 1.2: Removes a KeePass Entry - Valid " {
            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test1' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test1' }
            Remove-KeePassEntry -KeePassEntry $KeePassEntry -DatabaseProfileName 'SampleProfile' -Force | Should Be $null
        }

        It "Example 1.3: Removes a KeePass Entry - Valid - NoRecycle " {
            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test2' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test2' }
            Remove-KeePassEntry -KeePassEntry $KeePassEntry -DatabaseProfileName 'SampleProfile' -NoRecycle -Force | Should Be $null
        }

        It "Example 1.4: Removes a KeePass Entry - Valid - Pipeline " {
            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test3' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test3' }
            Remove-KeePassEntry -KeePassEntry $KeePassEntry -DatabaseProfileName 'SampleProfile' -Force | Should Be $null
        }

        It "Example 1.5: Removes a KeePass Entry - Valid - Pipeline - PWEntry" {
            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test444' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test444' }
            $KeePassEntry | Remove-KeePassEntry -DatabaseProfileName 'SampleProfile' -Force | Should Be $null
        }
    }

    Remove-KeePassConfigurationFile -Force
}

Describe "Get-KeePassEntry - UnitTest" -Tag UnitTest {

    Context "Example 1: Gets KeePass Entries." {

        Remove-KeePassConfigurationFile -Force

        # It "Example 1.1: Gets All KeePass Entries - Invalid - No Database Configuration Profiles." {

        #     { Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb/BadPath' } | Should Throw 'InvalidKeePassConfiguration : No KeePass Configuration has been created.'
        # }

        ## Create Profile
        New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force ) -Save -Force

        ## Reset Test DB
        Remove-Item -Path $Global:KPTestDatabaseFile -Force
        Copy-Item -Path $Global:KPTestBackupDatabaseFile -Destination "$($PSScriptRoot)/Includes/"

        It "Example 1.2 Gets All KeePass Entries - Valid" {
            $ResultEntries = Get-KeePassEntry -DatabaseProfileName SampleProfile
            $ResultEntries.Count | Should Be 9
        }

        It "Example 1.2 Gets All KeePass Entries - MasterKey Profile - Valid" {
            New-KeePassDatabaseConfiguration -DatabaseProfileName 'MasterKeyTest' -DatabasePath "$PSScriptRoot/Includes/AuthenticationDatabases/MasterKey.kdbx" -MasterKey $( ConvertTo-SecureString -String "ATestPassWord" -AsPlainText -Force ) -Save | Should Be $null
            $ResultEntries = Get-KeePassEntry -DatabaseProfileName 'MasterKeyTest'
            $ResultEntries.Count | Should Be 2
        }

        It "Example 1.3 Gets All KeePass Entries - Valid As Plain Text" {
            $ResultEntries = Get-KeePassEntry -DatabaseProfileName SampleProfile
            $ResultEntries.Count | Should Be 9
            $ResultEntries[0].Title | Should Be 'test1234'
            $ResultEntries[1].Title | Should Be 'nep0nb4q.wr0'
        }

        It "Example 1.4: Gets All KeePass Entries Of Specific Group - Valid" {

            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'SubGroupTest' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' -KeePassPassword "Potato" | Should Be $null

            #$ResultEntries = Get-KeePassEntry -DatabaseProfileName SampleProfile -KeePassEntryGroupPath 'SampleDb'
            #$ResultEntries.Title | Should Be 'SubGroupTest'
        }

        It "Example 1.5: Gets All KeePass Entries Of Specific Group - Invalid - Bad Path" {

            { Get-KeePassEntry -DatabaseProfileName SampleProfile -KeePassEntryGroupPath 'SampleDb/BadPath' } | Should Throw
        }

    }

    Remove-KeePassConfigurationFile -Force
}


Describe "New-KeePassEntry - UnitTest" -Tag UnitTest {

    Context "Example 1: Creates a New KeePass Entry." {

        Remove-KeePassConfigurationFile -Force

        ## Reset Test DB
        Remove-Item -Path $Global:KPTestDatabaseFile -Force
        Copy-Item -Path $Global:KPTestBackupDatabaseFile -Destination "$( $PSScriptRoot )/Includes/"

        ## Create Profile
        New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force ) -Save -Force

        It "Example 1.1: Creates a New KeePass Entry - Valid" {
            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' -KeePassPassword "123" | Should Be $null
        }

        It "Example 1.2: Creates a New KeePass Entry - Valid - PassThru" {

            $PassThruResult = New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'testPassThru' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' -KeePassPassword "123" -PassThru

            $PassThruResult.KPEntry | Should BeOfType KeePassLib.PwEntry
            $PassThruResult.KPEntry.ParentGroup.Name | Should BeLike 'SampleDb'
            $PassThruResult.KPEntry.Strings.ReadSafe('Title') | Should Be 'testPassThru'
            $PassThruResult.KPEntry.Strings.ReadSafe('UserName') | Should Be 'testuser'
            $PassThruResult.KPEntry.Strings.ReadSafe('Notes') | Should Be 'testnotes'
            $PassThruResult.KPEntry.Strings.ReadSafe('URL') | Should be 'http://url.test.com'
        }

        It "Example 1.3: Creates a New KeePass Entry - Invalid - Group Path does not Exist" {
            { New-KeePassEntry -KeePassEntryGroupPath 'BadPath' -Title 'test' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' } | Should Throw
        }

        It "Example 1.4: Creates a New KeePass Entry with manaully specified Password - Valid" {
            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'testPass' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -KeePassPassword $( ConvertTo-SecureString -String 'teststring' -AsPlainText -Force ) -DatabaseProfileName 'SampleProfile' | Should Be $null
        }

        It "Example 1.5: Creates a New KeePass Entry with a generated Password - Valid" {
            $GeneratedPassword = New-KeePassPassword -Upper -Lower -Digits -Length 50

            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'testPass' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -KeePassPassword $GeneratedPassword -DatabaseProfileName 'SampleProfile' | Should Be $null
        }

        It "Example 1.7: Creates a New KeePass Entry - Valid - PassThru - Icon" {

            $PassThruResult = New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'testPassThruIcon' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' -KeePassPassword "123" -IconName Apple -PassThru

            $PassThruResult.KPEntry | Should BeOfType KeePassLib.PwEntry
            $PassThruResult.KPEntry.ParentGroup.Name | Should BeLike 'SampleDb'
            $PassThruResult.KPEntry.Strings.ReadSafe('Title') | Should Be 'testPassThruIcon'
            $PassThruResult.KPEntry.Strings.ReadSafe('UserName') | Should Be 'testuser'
            $PassThruResult.KPEntry.Strings.ReadSafe('Notes') | Should Be 'testnotes'
            $PassThruResult.KPEntry.Strings.ReadSafe('URL') | Should Be 'http://url.test.com'
            $PassThruResult.KPEntry.IconId | Should Be 'Apple'
        }
    }
}

Describe "Update-KeePassEntry - UnitTest" -Tag UnitTest {

    Context "Example 1: Updates a KeePass Entry." {


        Remove-KeePassConfigurationFile -Force
        # It "Example 1.1: Creates a New KeePass Entry - Invalid - No Profile" {
        #     { Update-KeePassEntry -KeePassEntry $( New-Object KeePassLib.PwEntry($true, $true)) -KeePassEntryGroupPath 'database' -Title 'test' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' }| Should Throw 'InvalidKeePassConfiguration : No KeePass Configuration has been created.'
        # }

        ## Create Profile
        New-KeePassDatabaseConfiguration -DatabaseProfileName 'SampleProfile' -DatabasePath $KPTestDatabaseFile -KeyPath $KPTestKeyPath -MasterKey $( ConvertTo-SecureString -String "testpassword" -AsPlainText -Force ) -Save -Force

        ## Reset Test DB
        Remove-Item -Path $Global:KPTestDatabaseFile -Force
        Copy-Item -Path $Global:KPTestBackupDatabaseFile -Destination "$($PSScriptRoot)/Includes/" -Force

        It "Example 1.2: Updates a KeePass Entry - Valid - Properties" {
            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test1' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' -KeePassPassword "POTATO" | Should Be $null
            $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test1' }
            Update-KeePassEntry -KeePassEntry $KeePassEntry -KeePassEntryGroupPath 'SampleDb' -title 'UpdateTest1' -UserName 'UpdateTestUser' -Notes 'UpdateTestNotes' -URL 'http://UpdateURL.Test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
        }

        It "Example 1.3: Updates a KeePass Entry - Valid - Properties - Via Pipeline" {
            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test23' -UserName 'testuser23' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' -KeePassPassword "POTATO" | Should Be $null
            $entry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test23' }
            Update-KeePassEntry -KeePassEntry $entry -KeePassEntryGroupPath 'SampleDb' -title 'UpdateTest23' -UserName 'UpdateTestUser' -Notes 'UpdateTestNotes' -URL 'http://UpdateURL.Test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
        }

        It "Example 1.4: Update a KeePass Entry - Valid - Properties - PassThru" {

            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test3' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test3' }
            $UpdatePassThruResult = Update-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -KeePassEntry $KeePassEntry -title 'UpdateTest3' -UserName 'UpdateTestUser' -Notes 'UpdateTestNotes' -URL 'http://UpdateURL.Test.com' -DatabaseProfileName 'SampleProfile' -PassThru

            $UpdatePassThruResult.KPEntry | Should BeOfType KeePassLib.PwEntry
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('Title') | Should Be 'UpdateTest3'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('UserName') | Should Be 'UpdateTestUser'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('Notes') | Should Be 'UpdateTestNotes'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('URL') | Should Be 'http://UpdateURL.Test.com'
        }

        It "Example 1.5: Update a KeePass Entry - Valid - Group & Properties - PassThru" {

            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test4' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test4' }
            $UpdatePassThruResult = Update-KeePassEntry -KeePassEntry $KeePassEntry -title 'UpdateTest4' -UserName 'UpdateTestUser' -Notes 'UpdateTestNotes' -URL 'http://UpdateURL.Test.com' -KeePassEntryGroupPath 'SampleDb/12345' -DatabaseProfileName 'SampleProfile' -PassThru

            $UpdatePassThruResult.KPEntry | Should BeOfType KeePassLib.PwEntry
            $UpdatePassThruResult.KPEntry.ParentGroup.Name | Should Be '12345'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('Title') | Should Be 'UpdateTest4'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('UserName') | Should Be 'UpdateTestUser'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('Notes') | Should Be 'UpdateTestNotes'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('URL') | Should Be 'http://UpdateURL.Test.com'
        }

        It "Example 1.6: Update a KeePass Entry - Invalid - Group & Properties - PassThru - BadPath" {

            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test5' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test5' }
            { Update-KeePassEntry -KeePassEntry $KeePassEntry -title 'UpdateTest5' -UserName 'UpdateTestUser' -Notes 'UpdateTestNotes' -URL 'http://UpdateURL.Test.com' -KeePassEntryGroupPath 'SampleDb/BadPath' -DatabaseProfileName 'SampleProfile' -PassThru } | Should Throw
        }

        It "Example 1.7: Update a KeePass Entry - Valid - Properties - PassThru - Icon" {

            New-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -Title 'test6' -UserName 'testuser' -Notes 'testnotes' -URL 'http://url.test.com' -DatabaseProfileName 'SampleProfile' | Should Be $null
            $KeePassEntry = Get-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -DatabaseProfileName 'SampleProfile' | Where-Object { $_.Title -eq 'test6' }
            $UpdatePassThruResult = Update-KeePassEntry -KeePassEntryGroupPath 'SampleDb' -KeePassEntry $KeePassEntry -title 'UpdateTest6' -UserName 'UpdateTestUser' -Notes 'UpdateTestNotes' -URL 'http://UpdateURL.Test.com' -DatabaseProfileName 'SampleProfile' -IconName Apple -PassThru

            $UpdatePassThruResult.KPEntry | Should BeOfType KeePassLib.PwEntry
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('Title') | Should Be 'UpdateTest6'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('UserName') | Should Be 'UpdateTestUser'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('Notes') | Should Be 'UpdateTestNotes'
            $UpdatePassThruResult.KPEntry.Strings.ReadSafe('URL') | Should Be 'http://UpdateURL.Test.com'
            $UpdatePassThruResult.IconId | Should Be 'Apple'
        }
    }
    Remove-KeePassConfigurationFile -Force
}