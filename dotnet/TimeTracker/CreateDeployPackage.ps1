function BuildApplication
{
    & 'C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe' .\TimeTracker.sln /t:Rebuild /p:Configuration=Release
}

function CreateZipPackage
{
    Add-Type -assembly "system.io.compression.filesystem"

    $source = "$PSScriptRoot\bin\Release\"

    $version = GetApplicationVersion
    $destination = "$PSScriptRoot\Pichacky_$version.zip"
    
    [io.compression.zipfile]::CreateFromDirectory($source, $destination)
}

function GetApplicationVersion
{
    $assemblyInfo = Get-Content "$PSScriptRoot\TimeTracker.WindowsViews\Properties\AssemblyInfo.cs"
    $version = ($assemblyInfo | Select-String '\d.\d.\d').Matches.Groups[0].Value

    return $version
}

BuildApplication
CreateZipPackage