$supermodulesDir = "\\proliant03dev\HomeR_Deploy\CS\Supermoduls\"

function GetBranchName
{
    $url = $svnInfo.info.entry.url
    $urlElements = $url.Split("/")
    $branch = $urlElements[$urlElements.Length - 2]

    return $branch
}

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

function InstallSuperModule
{
    $superModule = Get-ChildItem $localSuperModuleDir -Filter "HCF_SuperModul_*.dll" | Sort-Object -Property LastWriteTime -Descending | Select-Object -First 1

    regsvr32 /s $superModule.FullName
}


$svnInfo = [xml](svn info --xml)
$dotNetDir = $svnInfo.info.entry.'wc-info'.'wcroot-abspath'
$localSuperModuleDir = $dotNetDir + "\Lib\SuperModul"

$branchName = GetBranchName
UnzipSuperModule
InstallSuperModule