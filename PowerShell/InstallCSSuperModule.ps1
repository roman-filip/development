$supermodulesDir = "\\proliant03dev\HomeR_Deploy\CS\Supermoduls\"
$vb6ModulesDir = "v:\_DEVELOPMENT\MODULY\CS\200_PRERELEASE_TEST\"

function GetBranchName
{
    $url = $svnInfo.info.entry.url
    $urlElements = $url.Split("/")
    $branch = $urlElements[$urlElements.Length - 2]

    return $branch
}

# This method is not anymore used
# It was used for copying of files from build server
function UnzipSuperModule
{
    $searchFilter = "SuperModul_" + $branchName + "_*.zip"

    $zippedSupermodule = Get-ChildItem $supermodulesDir -Filter $searchFilter | Sort-Object -Property LastWriteTime -Descending | Select-Object -First 1
    if ($zippedSupermodule -eq $null)
    {
        return "ERROR: Supermodule with name " + $searchFilter + " doesn't found"
    }

    $fileToUnzip = $supermodulesDir + $zippedSupermodule

    unzip -o $fileToUnzip -d $localSuperModuleDir
}

function CopySuperModule
{
    $superModulePath = $vb6ModulesDir + "CS$releaseNumber\$branchName\HOMER.VB6\HCF_SuperModul_*.dll"
    Copy-Item -Path $superModulePath -Destination $localSuperModuleDir
}

function InstallSuperModule
{
    $superModule = Get-ChildItem $localSuperModuleDir -Filter "HCF_SuperModul_*.dll" | Sort-Object -Property LastWriteTime -Descending | Select-Object -First 1

    regsvr32 /s $superModule.FullName
}

function GetReleseNumber
{
    $assemblyInfoFile = $dotNetDir + "\DafneShell\Properties\AssemblyInfo.cs"
    $assemblyInfo = Get-Content -Path $assemblyInfoFile
    
    foreach($line in $assemblyInfo)
    {
        if ($line -match 'AssemblyVersion\("\w*')
        {   # if file contains $AssemblyVersion then return its number
            if ($Matches[0] -match '\d+')
            {
                return $Matches[0]
            }
        }
    }

    throw "Cannot get release number from the \DafneShell\Properties\AssemblyInfo.cs file"
}


$svnInfo = [xml](svn info --xml)
$dotNetDir = $svnInfo.info.entry.'wc-info'.'wcroot-abspath'
$localSuperModuleDir = $dotNetDir + "\Lib\SuperModul"

$releaseNumber = GetReleseNumber
$branchName = GetBranchName

#UnzipSuperModule
CopySuperModule
InstallSuperModule