PARAM
(
    [Parameter(Mandatory=$true)]
    [string]$Branch,
    [Parameter(Mandatory=$true)]
    [string]$Release
)

$newGuid = 'RFI-TEST'
$workingDir = 'C:\TestRFI'

$dotnetSVNBranch = "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/$Branch"
$vb6SVNBranch = "https://subversion.homecredit.net/repos/vb/cs/$Branch"

$svnCommitMsg = "New SuperModul"

$dotnetDir = "$workingDir\$Branch\dotnet"
$commonDir = "$dotnetDir\Common"
$commonCsProjPath = "$commonDir\Common.csproj"

$vb6Dir = "$workingDir\$Branch\vb"
$supermodulDir = "$vb6Dir\HCF_SuperModul"
$supermodulVbProjPath = "$supermodulDir\HCF_SuperModul.vbp"
$oldSuperModuleName = "HCF_SuperModul_\d+"
$newSuperModuleName = "HCF_SuperModul_$Release"

$vb6compiler = "c:\Program Files\Microsoft Visual Studio\VB98\vb6.exe"
$vb6compilerParameters = "/make $supermodulVbProjPath /out $supermodulDir\compile_output.log /outdir $supermodulDir"


function PrepareSuperModulProjForBuild
{
    $supermodulVbProjContent = Get-Content $supermodulVbProjPath

    $supermodulVbProjContent = $supermodulVbProjContent -replace $oldSuperModuleName, $newSuperModuleName
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'CompatibleMode="2"', 'CompatibleMode="0"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'VersionCompatible32="1"', 'VersionCompatible32="0"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'AutoIncrementVer=1', 'AutoIncrementVer=0'

    Set-Content -Value $supermodulVbProjContent -Path $supermodulVbProjPath
}

function BuildSuperModul
{
    Start-Process $vb6compiler -ArgumentList $vb6compilerParameters -Wait
}

function SetSuperModulProjAfterBuild
{
    $supermodulVbProjContent = Get-Content $supermodulVbProjPath

    $supermodulVbProjContent = $supermodulVbProjContent -replace 'CompatibleMode="0"', 'CompatibleMode="2"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'VersionCompatible32="0"', 'VersionCompatible32="1"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'AutoIncrementVer=0', 'AutoIncrementVer=1'

    Set-Content -Value $supermodulVbProjContent -Path $supermodulVbProjPath
}

function AdjustDotNetCommon
{
    $commonCsProjContent = (Get-Content $commonCsProjPath)
    $commonCsProjContent = $commonCsProjContent -replace $oldSuperModuleName, $newSuperModuleName
    
    $commonCsProjContentXML = [xml]$commonCsProjContent
    foreach ($itemGroup in $commonCsProjContentXML.Project.ItemGroup)
    {
        foreach ($comRef in $itemGroup.COMReference)
        {
            if ($comRef -ne $null -and $comRef.Include -eq $newSuperModuleName)
            {
                $comRef.Guid = $newGuid;
                $commonCsProjContentXML.Save($commonCsProjPath);
                break;
            }
        }
    }
    
    #TODO - commit
    #svn commit $commonDir -m $svnCommitMsg
}


Write-Output "***************** Start SuperModul magic for $Branch *****************"


#svn checkout $dotnetSVNBranch $dotnetDir

svn checkout $vb6SVNBranch $vb6Dir
svn lock $supermodulVbProjPath
svn lock "$supermodulDir\*.dll"

PrepareSuperModulProjForBuild
BuildSuperModul
SetSuperModulProjAfterBuild

$dllFiles = @(Get-ChildItem -Path $supermodulDir -Filter *.dll) | Sort-Object -Property LastWriteTime
if ($dllFiles.Length -gt 1)
{
    svn del $dllFiles[0].FullName

    svn add $dllFiles[1].FullName
    svn propset svn:needs-lock on $dllFiles[1].FullName
}

svn commit $supermodulDir -m $svnCommitMsg


Write-Output "***************** Done SuperModul magic for $Branch ******************"


#AdjustDotNetCommon


#build SM jobem


# TODO: error handling
#    - super module was not successfuly build
#    -Raise error if Guid was not changed