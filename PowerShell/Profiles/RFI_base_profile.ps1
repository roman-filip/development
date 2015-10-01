# Defines my basic profile

# This script has to be loaded from file:
# \\proliant23.office.hci\home\filip\My Documents\WindowsPowerShell\Microsoft.PowerShell_profile.ps1
# with command:
# . 'c:\_GitHub\development\PowerShell\Profiles\RFI_base_profile.ps1'

# Information about how to digitaly signed powershell script:
# http://www.hanselman.com/blog/SigningPowerShellScripts.aspx

# Create alias ig (aka install git) what adds support for git to powershell console
New-Alias ig 'c:\Users\filip\AppData\Local\GitHub\PoshGit_869d4c5159797755bc04749db47b166136e59132\profile.example.ps1'

if($host.Name -eq 'ConsoleHost') 
{
    $executionPolicy = Get-ExecutionPolicy
    Set-ExecutionPolicy -ExecutionPolicy Bypass
    
    Import-Module PSReadline
    
    Set-ExecutionPolicy -ExecutionPolicy $executionPolicy
}

#Shortcut variables for directories
$gitHubDir = "c:\_GitHub\development\"
$bernardynDir = "c:\_GitHC\ESO9\Bernardyn\"