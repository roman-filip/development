[CmdletBinding()]
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

$gitRepoDir = "$BaseDir\repos"
$outputDir = "$BaseDir\output"
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
    LogStartingFunction("LogInputParameters")

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
    LogStartingFunction("PrepareOutputDir")
    
    if (Test-Path -Path $outputDir -Verbose:$useVerboseOutput)
    {
        Remove-Item -Path $outputDir -Recurse -Force -Verbose:$useVerboseOutput
    }

    New-Item -ItemType Directory -Path $outputDir -Verbose:$useVerboseOutput > $null

    Copy-Item -Path "$gitRepoDir\DB $CountryCode\*" -Destination $outputDir -Recurse -Verbose:$useVerboseOutput
}

function DeployGenesisScript
{
    LogStartingFunction("DeployGenesisScript")

    $params = $sqlCmdCommonParams + "-i $genesisFile"

    Write-Verbose "Params for sqlcmd: $params"
    
    Start-Process $sqlCmd -ArgumentList $params -Wait -NoNewWindow
}

function GenerateVersionFile
{
    LogStartingFunction("GenerateVersionFile")
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
PrepareOutputDir
DeployGenesisScript


#GenerateVersionFile

Write-Output "DONE"


#TODO:
# - v SK skriptech neni genesis a je tam o 10 souboru mene