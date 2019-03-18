properties {
    $projectRoot = (Resolve-Path "$PSScriptRoot\..\" ).Path
    $psVersion = $PSVersionTable.PSVersion.Major
    $commonParameters = @{
        "MSBuildConfig"="Release"
    }
}

task default -depends Init

task Init {
    Assert($Target -in @("core","framework","all")) " Target must be one of core, framework, all"
    "`nSTATUS: Building with PowerShell $psVersion"
} -description 'Initialize build environment'


task Build -depends Init {
    
    if (($Target -eq $null) -or ($Target -ieq "all"))
    {
        foreach($target in @("BuildCore","BuildFramework")){
            $commonParameters["MSBuildTarget"] = $target
            Invoke-Psake .\build.psake.ps1 -parameters $commonParameters -task Build
        }
    }
    else
    {
        if ($Target -ieq "core")
        {
            
            $commonParameters= @{
                "MSBuildTarget"="BuildCore"
            }
        }
        elseif($Target -ieq "framework")
        {
            $commonParameters= @{
                "MSBuildTarget"="BuildFramework"
            }
        }
        Invoke-Psake .\build.psake.ps1 -parameters $commonParameters -task Build
    }
    

} -description 'Build Specified Targets. Available [netcore,framework,all]. Default: All'

task PublishLocal -depends Init {
    if (($Target -eq $null) -or ($Target -ieq "all"))
    {
        foreach($target in @("BuildCore","BuildFramework")){
            $commonParameters["MSBuildTarget"] = $target
            Invoke-Psake .\build.psake.ps1 -parameters $commonParameters -task PublishLocal
        }
        
    }
    else
    {
        if ($Target -ieq "core")
        {
            $commonParameters["MSBuildTarget"] = "BuildCore"
        }
        elseif($Target -ieq "framework")
        {
            $commonParameters["MSBuildTarget"] = "BuildFramework"
        }
        Invoke-Psake .\build.psake.ps1 -parameters $commonParameters -task PublishLocal
    }
    
} -description 'Build Specified Targets. Available [netcore,framework]. Default: All'

task Publish -depends Init {
    if (($Target -eq $null) -or ($Target -ieq "all"))
    {
        foreach($target in @("BuildCore","BuildFramework")){
            $commonParameters["MSBuildTarget"] = $target
            Invoke-Psake .\build.psake.ps1 -parameters $commonParameters -task Publish
        }

    }
    else
    {
        if ($Target -ieq "core")
        {
            $commonParameters["MSBuildTarget"] = "BuildCore"
        }
        elseif($Target -ieq "framework")
        {
            $commonParameters["MSBuildTarget"] = "BuildFramework"
        }
        Invoke-Psake .\build.psake.ps1 -parameters $commonParameters -task Publish
    }

} -description 'Build Specified Targets. Available [netcore,framework]. Default: All'


task Pester -depends Init {
    if (($Target -eq $null) -or ($Target -ieq "all"))
    {
        foreach($target in @("BuildCore","BuildFramework")){
            $commonParameters["MSBuildTarget"] = $target
            Invoke-Psake .\build.psake.ps1 -parameters $commonParameters -task Pester
        }

    }
    else
    {
        if ($Target -ieq "core")
        {
            $commonParameters["MSBuildTarget"] = "BuildCore"
        }
        elseif($Target -ieq "framework")
        {
            $commonParameters["MSBuildTarget"] = "BuildFramework"
        }
        Invoke-Psake .\build.psake.ps1 -parameters $commonParameters -task Pester
    }

} -description 'Builds and Tests Specified Targets. Available [netcore,framework]. Default: All'

## Only works on Windows due to PlatyPS restriction.
task UpdateDocs -depends Init {
    $commonParameters["MSBuildTarget"] = "BuildFramework"
    Invoke-Psake .\build.psake.ps1 -parameters $commonParameters -task Build
    
    $docsPath = Join-Path $PSScriptRoot "..\docs"
    if(-not (Test-Path $docsPath)){
        New-Item $docsPath -ItemType Directory | Out-Null
    }

    $publishedPath = Join-Path $projectRoot "Package/Modules/PwshKeePass/PwshKeePass"
    $publishedNetCorePath = Join-Path $projectRoot "Package/Modules/PwshKeePass/PwshKeePass.NetCore"
    $frameworkExternalHelpPath = Join-Path $publishedPath "en-US"
    $netCoreExternalHelpPath = Join-Path $publishedNetCorePath "en-US"
    if(Test-Path $frameworkExternalHelpPath){
        Remove-Item -Path $frameworkExternalHelpPath -Recurse -Force    
    }
    if(Test-Path $netCoreExternalHelpPath){
        Remove-Item -Path $netCoreExternalHelpPath -Recurse -Force
    }
    $docCommand = "-NoProfile -Command `"cd $( $publishedPath ); Import-Module .\PwshKeePass.psd1 -Force; `""
    $docCommand += " Update-MarkdownHelpModule -Path `"$docsPath`" -UpdateInputOutput; "
    Invoke-Expression "& `"powershell.exe`" $docCommand"
    
    $externalHelpCommand = "-NoProfile -Command `"cd $( $publishedPath );`""
    $externalHelpCommand += " New-ExternalHelp -Path `"$docsPath`" -OutputPath `"$frameworkExternalHelpPath`" -Force;"
    $externalHelpCommand += " New-ExternalHelp -Path `"$docsPath`" -OutputPath `"$netCoreExternalHelpPath`" -Force"
    Invoke-Expression "& `"powershell.exe`" $externalHelpCommand"
} -description 'Update documentation'

