[CmdletBinding()]
PARAM
(
    [Parameter(Mandatory=$true)]
    [ValidateSet("192.168.0.43,1433", "192.168.0.41,1433")]
    [string]$DBServer,
    [Parameter(Mandatory=$true)]
    [string]$DBName,
    [Parameter(Mandatory=$true)]
    [ValidateSet("CZ", "SK")]
    [string]$CountryCode,
    [Parameter(Mandatory=$true)]
    [string]$BaseDir,
    [Parameter(Mandatory=$false)]
    [string]$ReleaseName,
    [Parameter(Mandatory=$false)]
    [string]$JobName,
    [Parameter(Mandatory=$false)]
    [string]$BuildNr,
    [Parameter(Mandatory=$false)]
    [string]$StartedBy
)

# TODO - can be removed some from input parameters

$gitRepoDir = "$BaseDir\repos"
$outputDir = "$BaseDir\output"

$genesisDir = "$outputDir\genesis"
$genesisFile = "$genesisDir\#_BASIC_STRUCTURE_Bernardyn.SQL"

$dbViewsDir = "$outputDir\pohledy"
$dbViewsDeployFile = "$dbViewsDir\_deploy_views.sql"

$dbFunctionsDir = "$outputDir\funkce"
$dbFunctionsDeployFile = "$dbFunctionsDir\_deploy_functions.sql"

$dbProceduresDir = "$outputDir\procedury"
$dbProceduresDeployFile = "$dbProceduresDir\_deploy_procedures.sql"

$dbReleasesBaseDir = "$outputDir\releasy"
$dbReleaseDir = "$dbReleasesBaseDir\$ReleaseName"
$dbReleaseDeployFile = "$dbReleaseDir\_deploy_release.sql"

$versionFile = "$outputDir\version.txt"

$sqlCmd = "sqlcmd"
$sqlCmdCommonParams = "-S $DBServer -d $DBName "

$useVerboseOutput = $false

$ignoredCountryCode = "XX"


function LogStartingFunction($functionName)
{
    Write-Output "Executing $functionName ..."
}

function SetVerboseOutput
{
    if ($VerbosePreference -eq "Continue")
    {
        $script:useVerboseOutput = $true
    }
}

function LogInputParameters
{
    LogStartingFunction "LogInputParameters"

    Write-Host "Input parameters:"
    Write-Host "================="

    Write-Host "  DBServer:    $DBServer"
    Write-Host "  DBName:      $DBName"
    Write-Host "  CountryCode: $CountryCode"
    Write-Host "  BaseDir:     $BaseDir"
    Write-Host "  ReleaseName: $ReleaseName"
    Write-Host "  JobName:     $JobName"
    Write-Host "  BuildNr:     $BuildNr"
    Write-Host "  StartedBy:   $StartedBy"
}

function SetIgnoredCountry
{
    switch ($CountryCode)
    {
        "CZ" { $global:ignoredCountryCode = "SK"; break; }
        "SK" { $global:ignoredCountryCode = "CZ"; break; }
        default { throw "Unsupported country code $CountryCode"; break; }
    }
}

function PrepareOutputDir
{
    LogStartingFunction "PrepareOutputDir"

    if (Test-Path -Path $outputDir -Verbose:$useVerboseOutput)
    {
        Remove-Item -Path $outputDir -Recurse -Force -Verbose:$useVerboseOutput
    }

    New-Item -ItemType Directory -Path $outputDir -Verbose:$useVerboseOutput > $null
    Copy-Item -Path "$gitRepoDir\DB\*" -Destination $outputDir -Recurse -Verbose:$useVerboseOutput

    Get-ChildItem -Path $outputDir -Filter $global:ignoredCountryCode -Recurse | ?{ $_.PSIsContainer } | Remove-Item -Recurse -Verbose:$useVerboseOutput
    Get-ChildItem -Path $dbReleasesBaseDir | ?{ $_.PSIsContainer -and $_.BaseName -ne $ReleaseName} | Remove-Item -Recurse -Verbose:$useVerboseOutput

    #New-Item $genesisDir -Type Directory -Verbose:$useVerboseOutput > $null
    #Copy-Item -Path "$gitRepoDir\genesis\*" -Destination $genesisDir -Recurse -Force -Verbose:$useVerboseOutput
}

function DeployGenesisScript
{
    #LogStartingFunction "DeployGenesisScript"

    #DeploySQLScript $genesisFile
}

function DeployViews
{
    LogStartingFunction "DeployViews"

    if (Test-Path -Path $dbViewsDir -Verbose:$useVerboseOutput)
    {
        Write-Verbose "Deploying views from $dbViewsDir"

        $views = Get-ChildItem $dbViewsDir -Recurse | ?{ ! $_.PSIsContainer }
        GenerateDeployScript $dbViewsDeployFile $views

        DeploySQLScript $dbViewsDeployFile
    }
}

function DeployFunctions
{
    LogStartingFunction "DeployFunctions"

    if (Test-Path -Path $dbFunctionsDir -Verbose:$useVerboseOutput)
    {
        Write-Verbose "Deploying functions from $dbFunctionsDir"

        $functions = Get-ChildItem $dbFunctionsDir -Recurse | ?{ ! $_.PSIsContainer }
        GenerateDeployScript $dbFunctionsDeployFile $functions

        DeploySQLScript $dbFunctionsDeployFile
    }
}

function DeployProcedures
{
    LogStartingFunction "DeployProcedures"

    if (Test-Path -Path $dbProceduresDir -Verbose:$useVerboseOutput)
    {
        Write-Verbose "Deploying procedures from $dbProceduresDir"

        $procedures = Get-ChildItem $dbProceduresDir -Recurse | ?{ ! $_.PSIsContainer }
        GenerateDeployScript $dbProceduresDeployFile $procedures

        DeploySQLScript($dbProceduresDeployFile)
    }
}

function DeployRelease
{
    LogStartingFunction "DeployRelease"

    if ($ReleaseName -and (Test-Path -Path $dbReleaseDir -Verbose:$useVerboseOutput))
    {
        Write-Verbose "Deploying procedures from $dbReleaseDir"

        $releaseScripts = Get-ChildItem $dbReleaseDir -Recurse | ?{ ! $_.PSIsContainer }
        GenerateDeployScript $dbReleaseDeployFile $releaseScripts

        DeploySQLScript($dbReleaseDeployFile)
    }
}

function GenerateDeployScript($deployFile, $files)
{
    Write-Verbose "Generating deploy script $deployFile"

    foreach ($file in $files)
    {
        Write-Verbose "  Adding script $($file.FullName)"
        Add-Content $deployFile ":r ""$($file.FullName)"""
        Add-Content $deployFile "GO"
    }
}

function DeploySQLScript($file)
{
    $params = $sqlCmdCommonParams + "-i $file"

    Write-Verbose "Starting sqlcmd.exe $params"

    Start-Process $sqlCmd -ArgumentList $params -Wait -NoNewWindow
}

# TODO - makes this function sense?
function GenerateVersionFile
{
    LogStartingFunction "GenerateVersionFile"
    Write-Verbose "Generating version file: $versionFile"

    $gitBranch = git -C $gitRepoDir rev-parse --abbrev-ref HEAD
    $gitLongVersion = git -C $gitRepoDir rev-parse HEAD
    $gitShortVersion = git -C $gitRepoDir rev-parse --short HEAD

    $versionInfo = 
        "Jenkins: `r`n" +
        "  build number: $BuildNr `r`n" +
        "  build started by: $StartedBy `r`n" +
        "  build date: " + (Get-Date).ToString() + "`r`n`r`n" +
        "Git: `r`n" +
        "  branch: $gitBranch `r`n" +
        "  short version: $gitShortVersion `r`n" +
        "  long version:  $gitLongVersion `r`n"

    Write-Verbose "Version info:"
    Write-Verbose "============="
    Write-Verbose $versionInfo

    Set-Content $versionFile $versionInfo
}


SetVerboseOutput
LogInputParameters
SetIgnoredCountry
PrepareOutputDir

#DeployGenesisScript

DeployViews
DeployFunctions
DeployProcedures
DeployRelease

#GenerateVersionFile

Write-Output "DONE"


# TODO:
    # po deploy zjistit seznam nevalidnich objektu a pripadne je prekompilovat
