PARAM
(
    [Parameter(Mandatory=$true)]
    [string]$Branch,
    [Parameter(Mandatory=$true)]
    [string]$WorkingDir
)

$hciLibDir = "\\cz-vw84\HomeR_Deploy\HCILibrary2\"
$buildScriptDir = "$WorkingDir\BuildScript"
$copyHCILibPath = "$buildScriptDir\CopyHCILib.bat"

function GetNewestVersion
{
    $hciLibraries = Get-ChildItem $hciLibDir -Directory -Exclude "LastVersion" | Sort-Object -Descending -Property LastWriteTime
    $newestHciLibrary = $hciLibraries[0]

    return $newestHciLibrary.Name
}

try
{
    Write-Output "Starting with .NET library freeze ..."

    svn checkout "$Branch/BuildScript" $buildScriptDir

    $version = GetNewestVersion

    $copyHCILibContent = (Get-Content $copyHCILibPath)
    $copyHCILibContent = $copyHCILibContent -replace "SET version=LastVersion", "SET version=$version"
    Set-Content -Value $copyHCILibContent -Path $copyHCILibPath

    Write-Output "HCILibrary version set to $version"

    svn commit $buildScriptDir -m "Fix to stable version of HCILibrary"

    if ($LASTEXITCODE -eq 0)
    {
        Write-Output ".NET library freeze finished successfully"
        exit 0
    }
    else
    {
        Write-Output ".NET library freeze finished with error"
        exit 1
    }
}
catch
{
    Write-Output "`nShit happened:"
    Write-Output "=============="
    Write-Output $_

    exit 2
}