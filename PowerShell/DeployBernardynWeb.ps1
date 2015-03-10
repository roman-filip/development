[CmdletBinding()]
PARAM
(
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

    Write-Host "  BaseDir:   $BaseDir"
    Write-Host "  TargetDir: $TargetDir"
    Write-Host "  JobName:   $JobName"
    Write-Host "  BuildNr:   $BuildNr"
    Write-Host "  StartedBy: $StartedBy"
}

function PrepareOutputDir
{
    LogStartingFunction("PrepareOutputDir")
    
    if (Test-Path -Path $outputDir -Verbose:$useVerboseOutput)
    {
        Remove-Item -Path $outputDir -Recurse -Force -Verbose:$useVerboseOutput
    }

    New-Item -ItemType Directory -Path $outputDir -Verbose:$useVerboseOutput > $null
}

function PrepareHtml
{
    LogStartingFunction("PrepareHtml")

    Copy-Item -Path "$gitRepoDir\web\*" -Destination $outputDir -Recurse -Verbose:$useVerboseOutput
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

function CleanWebServer
{
    LogStartingFunction("CleanWebServer")

    Remove-Item -Path "$TargetDir\*" -Recurse -Force -Verbose:$useVerboseOutput
}

function DeployToWebServer
{
    LogStartingFunction("DeployToWebServer")
    Write-Verbose "Deploying Bernardyn HTML to $TargetDir"

    Copy-Item -Path "$outputDir\*" -Destination $TargetDir -Recurse -Verbose:$useVerboseOutput
}


SetVerboseOutput
LogInputParameters
PrepareOutputDir
PrepareHtml
GenerateVersionFile
CleanWebServer
DeployToWebServer

Write-Output "DONE"


#TODO
#pridat soubor s prefixem verze do repozitare
#$deployBackupDir = "TODO"  # historie balicku co se nasazovaly