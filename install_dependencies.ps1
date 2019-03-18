Install-Module "PSDepend" -Scope CurrentUser -Force
Invoke-PSDepend "$PSScriptRoot/requirements.psd1" -Force
Invoke-PSDepend "$PSScriptRoot/requirements.psd1" -Force -Import
