properties {
    $script:publishedPath = $null
    $moduleName = "PwshKeePass"
    $coreModuleName = "PwshKeePass.NetCore"
    $frameworkModuleName = "PwshKeePass"
}
task InitUnique {
    Assert($MSBuildTarget -in @("BuildFramework", "BuildCore")) " MSBuildTarget must be one of BuildCore, BuildFramework - $MSBuildTarget"
    "Initializing target-unique parameters."
    $script:publishedPath = Join-Path $projectRoot "Package/Modules/PwshKeePass/"
    if ($MSBuildTarget -ieq "BuildFramework")
    {
        $script:moduleName = $frameworkModuleName
        $global:powershellExecutable = "powershell"
    }
    elseif($MSBuildTarget -ieq "BuildCore")
    {
        $script:moduleName = $coreModuleName
        $global:powershellExecutable = "pwsh"
    }
    $script:publishedPath += $script:moduleName

    Assert(Test-Path(Resolve-Path $script:publishedPath)) "$( $script:publishedPath ) does not exist. Please ensure targets have been built"
    Set-Location $script:publishedPath
} -description 'Initialize build environment for target-specific tasks'

task EnsureLocalRepoPath {
    if (-not(Test-Path -Path "C:\dev\localnuget\package"))
    {
        New-Item "C:\dev\localnuget\package" -ItemType Directory
    }
}

task RegisterLocalRepo {
    $existingRepo = Get-PSRepository | Where-Object { $_.Name -eq "localnuget" }
    if ($existingRepo -eq $null)
    {
        Register-PSRepository -Name "localnuget" `
            -SourceLocation "C:\dev\localnuget" `
            -PublishLocation "C:\dev\localnuget\package" `
            -InstallationPolicy Trusted `
            -PackageManagementProvider NuGet
    }
}

task RegisterVstsNugetRepo {
    $existingRepo = Get-PSRepository | Where-Object { $_.Name -eq "PowerShellDev" }
    if ($existingRepo -eq $null)
    {
        Register-PSRepository -Name "PowerShellDev" `
            -SourceLocation "https://todorov.pkgs.visualstudio.com/_packaging/PSModules/nuget/v2" `
            -PublishLocation "https://todorov.pkgs.visualstudio.com/_packaging/PSModules/nuget/v2" `
            -InstallationPolicy Trusted `
            -PackageManagementProvider NuGet
    }
}

task Publish -Depends InitUnique, RegisterVstsNugetRepo, Pester {
    Assert($Env:NuGetApiKey -ne $null) "Please set `$Env:NuGetApiKey on the environment before trying to publish"
    $nugetAddCommand = "sources Add -Name `"PowerShellDev`" -Source `"https://todorov.pkgs.visualstudio.com/_packaging/PSModules/nuget/v2`" -UserName dimiter.todorov@gmail.com -Password $( $Env:NuGetApiKey )"
    Invoke-Expression "& `"nuget`" sources remove -Name PowerShellDev"
    Invoke-Expression "& `"nuget`" $nugetAddCommand"
    Publish-Module -NuGetApiKey $Env:NuGetApiKey -Path $publishedPath -Repository "PowerShellDev"
}

task Build {
    Assert($MSBuildTarget -ne $null) "MSBuildTarget must be specified. Preferrable call psakefile.ps1 directly"
    $msBuildString = "$( Join-Path $PSScriptRoot ""..\build.proj"" )"
    $msBuildString += " /t:$( $MSBuildTarget )"
    $msBuildString += " -p:Configuration=Release"
    "Invoking & `"dotnet`" msbuild $msBuildString"
    Invoke-Expression "& `"dotnet`" msbuild $msBuildString"
}

task PublishLocal -depends Build, InitUnique {
    Publish-Module -Path $script:publishedPath -Repository "localnuget" -Verbose
} -description 'Builds and Publishes Specified Targets. Available [BuildCore,]. Default: All'

task Pester -depends Build, InitUnique {
    $testOutputPath = Join-Path $PSScriptRoot "..\build\test"
    if(-not (Test-Path $testOutputPath)){
        New-Item $testOutputPath -ItemType Directory | Out-Null
    }
    $pesterCommand = "-NoProfile -Command `"cd $( $script:publishedPath ); Invoke-Pester -EnableExit`""
    $pesterCommand += " -OutputFormat NUnitXml -OutputFile `"$testOutputPath/pester_output_$($script:moduleName).xml`""
    Invoke-Expression "& `"$( $global:powershellExecutable )`" $pesterCommand"
    Assert($LASTEXITCODE -eq 0) "$LASTEXITCODE Pester Tests Failed. Aborting"
} -description 'Builds and Tests Specified Targets. Available [BuildCore,]. Default: All'