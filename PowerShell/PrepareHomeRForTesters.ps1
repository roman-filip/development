$vb6componentsPath = "v:\_DEVELOPMENT\MODULY\CS\600_COMPONENTS\*"
$vb6ModulesPath = "v:\_DEVELOPMENT\MODULY\CS\200_PRERELEASE_TEST\2014.03\HOMER.VB6\"
$srcDir = "c:\_svn\dotnet\CS\trunk_build"

$dateExtension = Get-Date -Format yyyyMMdd_HHmm
$targetDir = "o:\filip\MSI_" + $dateExtension

# Register the latest SuperModule
Write-Output "Installing SuperModul"
InstallSuperModule.ps1 HCF_SuperModul_143

# Copy VB6 Modules
Write-Output "Copying VB6 modules and components"
Start-Job -Scriptblock{
    param($sourceVB6Mod,$sourceVB6Comp,$destination)
    Copy-Item $sourceVB6Mod $destination -recurse -force
    Copy-Item $sourceVB6Comp $destination -recurse -force
} -ArgumentList $vb6ModulesPath,$vb6componentsPath,$targetDir

# Update from SVN
Write-Output "Updating sources from SVN"
svn up $srcDir

# Build .NET
Write-Output "Building .NET"
c:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild "$srcDir\DafneShell_VS2010.sln" "/p:configuration=Release;Platform=x86"

# Copy .NET
Write-Output "Copying .NET"
Copy-Item -Path "$srcDir\bin\Release\*" -Destination $targetDir -Recurse -Force

Write-Output "DONE!!!"