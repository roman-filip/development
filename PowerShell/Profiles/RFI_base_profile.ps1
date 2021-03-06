# Defines my basic profile

# This script has to be loaded from file:
# c:\Users\roman.filip\Documents\WindowsPowerShell\Microsoft.PowerShell_profile.ps1
# with command:
# . 'c:\_GitHub\development\PowerShell\Profiles\RFI_base_profile.ps1'

# Information about how to digitaly signed powershell script:
# http://www.hanselman.com/blog/SigningPowerShellScripts.aspx

# Create alias ig (aka install git) what adds support for git to powershell console
New-Alias ig 'c:\Program Files\WindowsPowerShell\Modules\posh-git\0.6.1.20160330\profile.example.ps1'

# Alias for start of the SourceTree application
New-Alias st 'C:\Program Files (x86)\Atlassian\SourceTree\SourceTree.exe'

if($host.Name -eq 'ConsoleHost') 
{
    $executionPolicy = Get-ExecutionPolicy
    Set-ExecutionPolicy -ExecutionPolicy Bypass
    
    Import-Module PSReadline
    
    Set-ExecutionPolicy -ExecutionPolicy $executionPolicy
}

# Shortcut variables for directories
$gitHubDir = "c:\_GitHub\development\"
$bernardynDir = "c:\_GitHC\ESO9\Bernardyn\"
$csDotNETDir = "c:\_svn\dotnet\CS\"
$gitFilipDotNetHomeR = "c:\_GitHCI\filip\cs-homer-dotnet\"
$myloan = "c:\_GitHCI\Custom-CS\myloan\"
$openAPI = "c:\_GitHCI\Custom-CS-OpenAPI\"
