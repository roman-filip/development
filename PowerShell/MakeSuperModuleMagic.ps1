PARAM
(
    [Parameter(Mandatory=$true)]
    [string]$Branch,
    [Parameter(Mandatory=$true)]
    [string]$Release,
    [Parameter(Mandatory=$true)]
    [string]$WorkingDir
)

$newGuid

$dotnetSVNBranch = "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/$Branch"
$vb6SVNBranch = "https://subversion.homecredit.net/repos/vb/cs/$Branch"

$svnCommitMsg = "New SuperModul"

$dotnetDir = "$WorkingDir\$Branch\dotnet"
$commonDir = "$dotnetDir\Common"

$vb6Dir = "$WorkingDir\$Branch\vb"
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
    Write-Output "Building SuperModul ..."
    
    Start-Process $vb6compiler -ArgumentList $vb6compilerParameters -Wait
    
    Write-Output "SuperModul compiled"
}

function SetSuperModulProjAfterBuild
{
    $supermodulVbProjContent = Get-Content $supermodulVbProjPath

    $supermodulVbProjContent = $supermodulVbProjContent -replace 'CompatibleMode="0"', 'CompatibleMode="2"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'VersionCompatible32="0"', 'VersionCompatible32="1"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'AutoIncrementVer=0', 'AutoIncrementVer=1'

    Set-Content -Value $supermodulVbProjContent -Path $supermodulVbProjPath
}

function MakeVB6Magic
{
    Write-Output "************** Begining with VB6 magic **************"
    
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
    
    Write-Output "************** VB6 magic finished **************"
}

function GetSuperModulGuid
{
    $superModuleDll = Get-ChildItem -Path $supermodulDir -Filter *.dll

    $guid = .\GetGUID.exe $superModuleDll.FullName
    return $guid
}

function AdjustDotNetCommonCsproj
{
    $commonCsProjPath = "$commonDir\Common.csproj"

    $commonCsProjContent = (Get-Content $commonCsProjPath)
    $commonCsProjContent = $commonCsProjContent -replace $oldSuperModuleName, $newSuperModuleName
    
    $commonCsProjContentXML = [xml]$commonCsProjContent
    foreach ($itemGroup in $commonCsProjContentXML.Project.ItemGroup)
    {
        foreach ($comRef in $itemGroup.COMReference)
        {
            if ($comRef -ne $null -and $comRef.Include -eq $newSuperModuleName)
            {
                $comRef.Guid = [string]$newGuid;
                $commonCsProjContentXML.Save($commonCsProjPath);
                break;
            }
        }
    }
}

function AdjustNetInterfaceFile
{
    $netInterfacePath = "$commonDir\ExternalModules\NetInterfaceCZ.cs"
    
    $netInterfaceContent = (Get-Content $netInterfacePath)
    $netInterfaceContent = $netInterfaceContent -replace $oldSuperModuleName, $newSuperModuleName
    Set-Content $netInterfacePath $netInterfaceContent
}

function MakeDotNetMagic
{
    Write-Output "************** Begining with .NET magic **************"
    
    svn checkout "$dotnetSVNBranch/Common" $commonDir
    
    $newGuid = GetSuperModulGuid
    
    AdjustDotNetCommonCsproj
    
    AdjustNetInterfaceFile
    
    svn commit $commonDir -m $svnCommitMsg
    
    Write-Output "************** .NET magic finished **************"
}


Write-Output "************** Begining with SuperModul magic for $Branch **************"

MakeVB6Magic

MakeDotNetMagic

Write-Output "************** SuperModul magic for $Branch finished ***************"


# TODO: error handling
#    - super module was not successfuly build
#    -Raise error if Guid was not changed