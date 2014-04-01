function GetProjects
{
    $cnt = 0
    foreach ($file in Get-ChildItem ./ -Recurse -Filter "*.csproj")
    {
        Write-Output $file.Name - $file.DirectoryName
        $cnt++
    }
    Write-Output "Count of projects:" $cnt
}

function GetCntOfFiles([string] $filter)
{
    $files = Get-ChildItem ./ -Recurse -Filter $filter
    return $files.Count
}

function GetFileContent([string] $fileName)
{
    $index = 0
    $items = Get-ChildItem -Path .\ -Recurse $fileName | Select-Object -Property FullName
    foreach($item in $items)
    {
        $index++
        $path = ($item | Get-Member 'FullName') -replace 'System.String FullName=', ''
        Write-Output "$index : $path"
        
        $arr = Get-Content $path;
        $fileContent = [String]::Join([Environment]::NewLine, $arr)
        if($arr -eq $null)
        {
            continue
        }

        Write-Output $fileContent

        Write-Output ""
        Write-Output "*************************************************************"
        Write-Output ""
    }
}

function WriteFilesWithoutText([string] $fileName, [string] $text)
{
    Write-Output "List of files ($fileName) without text '$text':"
    
    $cnt = 0
    $cntOfWrongFiles = 0
    $files = Get-ChildItem -Path .\ -Recurse -Filter $fileName
    foreach($file in $files)
    {
        $cnt++
        
        $arr = Get-Content $file.FullName;
        $fileContent = [String]::Join([Environment]::NewLine, $arr)
        if($arr -eq $null)
        {
            continue
        }
        
        if (!($fileContent -like "*$text*"))
        #if (!($fileContent -like "$text"))
        {
            write $file.FullName
            $cntOfWrongFiles++
        }
    }
    
    Write-Output "Count of wrong files $cntOfWrongFiles/$cnt"
}

cls
cd c:\_svn\dotnet\CS\trunk

GetProjects
#Write-Output ("Count of projects: {0}" -f (GetCntOfFiles "*.csproj"))
#Write-Output ("Count of AssemblyInfo.cs: {0}" -f (GetCntOfFiles "AssemblyInfo.cs"))
#Write-Output ("Count of views: {0}" -f (GetCntOfFiles "*.Designer.cs"))
#GetFileContent("AssemblyInfo.cs")
#GetFileContent("*.csproj")
#WriteFilesWithoutText "AssemblyInfo.cs" "assembly: HCI.Infrastructure.TraceLog.TraceLog"
#WriteFilesWithoutText "*.csproj" "<DontImportPostSharp>True</DontImportPostSharp>"
#WriteFilesWithoutText "*.csproj" '<Import Project="$(MSBuildExtensionsPath)\PostSharp\PostSharp-1.5.targets" />'
