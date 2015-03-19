﻿[CmdletBinding()]
PARAM
(
    [Parameter(Mandatory=$true)]
    [string]$DBName,
    [Parameter(Mandatory=$true)]
    [ValidateSet("CZ", "SK")]
    [string]$CountryCode,
    [Parameter(Mandatory=$true)]
    [string]$BaseDir,
    [Parameter(Mandatory=$true)]
    [string]$TargetDir,
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

$dbViewsDir = "$outputDir\views"
$dbViewsDeployFile = "$dbViewsDir\_deploy_views.sql"

$dbFunctionsDir = "$outputDir\funkce"
$dbFunctionsDeployFile = "$dbFunctionsDir\_deploy_functions.sql"

$dbProceduresDir = "$outputDir\procedury"
$dbProceduresDeployFile = "$dbProceduresDir\_deploy_procedures.sql"

$versionFile = "$outputDir\version.txt"
$genesisFile = "$outputDir\genesis\#_BASIC_STRUCTURE_Bernardyn.SQL"

$sqlCmd = "sqlcmd"
$sqlCmdCommonParams = "-S 192.168.0.43,1433 -d $DBName "

$useVerboseOutput = $false


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

    Write-Host "  DBName:      $DBName"
    Write-Host "  CountryCode: $CountryCode"
    Write-Host "  BaseDir:     $BaseDir"
    Write-Host "  TargetDir:   $TargetDir"
    Write-Host "  JobName:     $JobName"
    Write-Host "  BuildNr:     $BuildNr"
    Write-Host "  StartedBy:   $StartedBy"
}

function PrepareOutputDir
{
    LogStartingFunction "PrepareOutputDir"
    
    if (Test-Path -Path $outputDir -Verbose:$useVerboseOutput)
    {
        Remove-Item -Path $outputDir -Recurse -Force -Verbose:$useVerboseOutput
    }

    New-Item -ItemType Directory -Path $outputDir -Verbose:$useVerboseOutput > $null

    Copy-Item -Path "$gitRepoDir\DB $CountryCode\*" -Destination $outputDir -Recurse -Verbose:$useVerboseOutput
}

function DeployGenesisScript
{
    LogStartingFunction "DeployGenesisScript"

    DeploySQLScript $genesisFile
}

function DeployViews
{
    LogStartingFunction "DeployViews"
    Write-Verbose "Deploying views from $dbViewsDir"

    $views = Get-ChildItem $dbViewsDir -Recurse
    GenerateDeployScript $dbViewsDeployFile $views

    DeploySQLScript $dbViewsDeployFile
}

function DeployFunctions
{
    LogStartingFunction "DeployFunctions"
    Write-Verbose "Deploying functions from $dbFunctionsDir"

    $functions = Get-ChildItem $dbFunctionsDir -Recurse
    GenerateDeployScript $dbFunctionsDeployFile $functions

    DeploySQLScript $dbFunctionsDeployFile
}

function DeployProcedures
{
    LogStartingFunction "DeployProcedures"
    Write-Verbose "Deploying procedures from $dbProceduresDir"

    $procedures = Get-ChildItem $dbProceduresDir -Recurse
    GenerateDeployScript $dbProceduresDeployFile $procedures

    DeploySQLScript($dbProceduresDeployFile)
}

function GenerateDeployScript($deployFile, $files)
{
    Write-Verbose "Generating deploy script $deployFile"

    foreach ($file in $files)
    {
        Write-Verbose "  Adding script $($file.FullName)"
        Add-Content $deployFile ":r $($file.FullName)"
    }
}

function DeploySQLScript($file)
{
    $params = $sqlCmdCommonParams + "-i $file"

    Write-Verbose "Starting sqlcmd.exe $params"

#    Start-Process $sqlCmd -ArgumentList $params -Wait -NoNewWindow
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


# TODO - remove cls
cls

SetVerboseOutput
LogInputParameters
PrepareOutputDir

DeployGenesisScript
DeployViews
DeployFunctions
DeployProcedures


#GenerateVersionFile

Write-Output "DONE"


# TODO:
    # - v SK skriptech neni genesis a je tam o 10 souboru mene je to OK
    # proc je ve ve view USE [BERNARDYN_SK] - a navic v CZ adresari
    # nekde je zase USE [BERNARDYN
    # u vsech objektu by melo byt create or replace

# Call s Kugelem
    # pro testy si odmazat USE, ale v budoucnu uz tam nebude
    # create zmenit na alter, v budoucnu bude spravne uz v souboru
