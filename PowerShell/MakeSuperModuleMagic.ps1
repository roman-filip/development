$newGuid = 'RFI-TEST'
$branch = 'cs06_sp17_d2'
$dotnetSVNBranch = "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/$branch"
$vb6SVNBranch = "https://subversion.homecredit.net/repos/vb/cs/branches/$branch"
$workingDir = 'C:\TestRFI'


$release = 147
$commonCsProjPath = "$workingDir\dotnet\Common\Common.csproj"
$supermodulVbProjPath = "$workingDir\vb\HCF_SuperModul\HCF_SuperModul.vbp"
$oldSuperModuleName = "HCF_SuperModul_\d{3}"
$newSuperModuleName = "HCF_SuperModul_$release"


function PrepareSuperModulProjForBuild
{
#TODO - read only vbproj

    $supermodulVbProjContent = Get-Content $supermodulVbProjPath

    $supermodulVbProjContent = $supermodulVbProjContent -replace $oldSuperModuleName, $newSuperModuleName
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'CompatibleMode="2"', 'CompatibleMode="0"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'VersionCompatible32="1"', 'VersionCompatible32="0"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'AutoIncrementVer=1', 'AutoIncrementVer=0'

    Set-Content -Value $supermodulVbProjContent -Path $supermodulVbProjPath
}

function BuildSuperModul
{
    #TODO
    # wait for build
    
    & "c:\Program Files\Microsoft Visual Studio\VB98\vb6.exe" /make $supermodulVbProjPath /out c:\TestRFI\build\compile_output.log /outdir c:\TestRFI\build
}

function SetSuperModulProjAfterBuild
{
    $supermodulVbProjContent = Get-Content $supermodulVbProjPath

    $supermodulVbProjContent = $supermodulVbProjContent -replace 'CompatibleMode="0"', 'CompatibleMode="2"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'VersionCompatible32="0"', 'VersionCompatible32="1"'
    $supermodulVbProjContent = $supermodulVbProjContent -replace 'AutoIncrementVer=0', 'AutoIncrementVer=1'

#TODO - smazat Path32 ?????

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
                
                #TODO - commit
                
                break;
            }
        }
    }

#    foreach ($comRef in $commonCsProjContentXML.Project.ItemGroup.COMReference)
#    {
#        if ($comRef -ne $null -and $comRef.Include.StartsWith('HCF_SuperModul_')) 
#        {
#            $comRef.Guid = $newGuid;
#            $commonCsProjContentXML.Save($commonCsProjPath);
#            break;
#        }
#    }

}


#TODO - remove function
function CheckOutSourceCodes
{
    svn checkout $dotnetSVNBranch "$workingDir\dotnet";
    svn checkout $vb6SVNBranch "$workingDir\vb";
}

CheckOutSourceCodes

#PrepareSuperModulProjForBuild
#BuildSuperModul
#SetSuperModulProjAfterBuild

#commit super module + DLL

#build SM jobem


#AdjustDotNetCommon



# proliant03dev       $commonCsProjContentXML.Project.ItemGroup[6].COMReference[0].Guid = $newGuid;


# Raise error if Guid was not changed