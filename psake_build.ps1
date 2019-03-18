    [CmdletBinding(DefaultParameterSetName = 'help')]
    param (
        [parameter(ParameterSetName = 'task', Position = 0)]
        [string[]]$Task = 'default',

        [parameter(ParameterSetName = 'help')]
        [switch]$Help,

        [parameter(ParameterSetName = 'task', Position = 1)]
        $Parameters = @{"Target" = "all"}
    )
    $ErrorActionPreference = "Stop"
    $psakeFile = (Join-Path -Path $PSScriptRoot -ChildPath 'support\psakeFile.ps1')
    if ( $PSBoundParameters.ContainsKey('help'))
    {
        Get-PSakeScriptTasks -buildFile $psakeFile |
                Format-Table -Property Name, Description, Alias, DependsOn
    }
    else
    {
        Invoke-psake -buildFile $psakeFile -taskList $Task -nologo -parameters $Parameters
        exit ( [int]( -not$psake.build_success))
    }