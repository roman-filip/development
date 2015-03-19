[CmdletBinding()]
PARAM
(
    [Parameter(Mandatory=$true)]
    [string]$BaseDir,
    [Parameter(Mandatory=$true)]
    [string]$TargetDir
)

$gitRepoDir = "$BaseDir\repos"
$outputDir = "$BaseDir\output"
$versionFile = "$outputDir\version.txt"

$jobName = $env:JOB_NAME
$buildNr = $env:BUILD_NUMBER
$startedBy = $env:BUILD_USER
$gitBranch = $env:GIT_BRANCH

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
    Write-Host "  BaseDir:   $BaseDir"
    Write-Host "  TargetDir: $TargetDir"
}

function PrepareOutputDir
{
    LogStartingFunction "PrepareOutputDir"

    if (Test-Path -Path $outputDir -Verbose:$useVerboseOutput)
    {
        Remove-Item -Path $outputDir -Recurse -Force -Verbose:$useVerboseOutput
    }

    New-Item -ItemType Directory -Path $outputDir -Verbose:$useVerboseOutput > $null
}

function PrepareHtml
{
    LogStartingFunction "PrepareHtml"

    Copy-Item -Path "$gitRepoDir\web\*" -Destination $outputDir -Recurse -Verbose:$useVerboseOutput
}

function GenerateVersionFile
{
    LogStartingFunction "GenerateVersionFile"
    Write-Verbose "Generating version file: $versionFile"

    $gitLongVersion = git -C $gitRepoDir rev-parse HEAD
    $gitShortVersion = git -C $gitRepoDir rev-parse --short HEAD

    $versionInfo = 
        "Jenkins: `n" +
        "  job name: $jobName `n" +
        "  build number: $buildNr `n" +
        "  build started by: $startedBy `n" +
        "  build date: " + (Get-Date).ToString() + "`n`n" +
        "Git: `n" +
        "  branch: $gitBranch `n" +
        "  short version: $gitShortVersion `n" +
        "  long version:  $gitLongVersion `n"

    Write-Verbose "Version info:"
    Write-Verbose "============="
    Write-Verbose $versionInfo

    Set-Content $versionFile $versionInfo
}

function CleanWebServer
{
    LogStartingFunction "CleanWebServer"

    Remove-Item -Path "$TargetDir\*" -Recurse -Force -Verbose:$useVerboseOutput
}

function DeployToWebServer
{
    LogStartingFunction "DeployToWebServer"
    Write-Verbose "Deploying Bernardyn HTML to $TargetDir"

    Copy-Item -Path "$outputDir\*" -Destination $TargetDir -Recurse -Verbose:$useVerboseOutput
}


Write-Output "Executing script $($MyInvocation.InvocationName) ..."

SetVerboseOutput
LogInputParameters
PrepareOutputDir
PrepareHtml
GenerateVersionFile
CleanWebServer
DeployToWebServer

Write-Output "Script $($MyInvocation.InvocationName) finished"


# TODO
#   pridat soubor s prefixem verze do repozitare
#   $deployBackupDir = "TODO"  # historie balicku co se nasazovaly