$clones = @("BY", "CS", "KZ", "VN", "CN", "MN")
$pathTemplate = "C:\_svn\dotnet\{0}\trunk"
$debug = $false

function Debug($message)
{
    if ($debug)
    {
        Write-Output -ForegroundColor Gray $message
    }
}

function GetProjects
{
    $projects = Get-ChildItem ./ -Recurse -Filter "*.csproj" | Sort-Object
    return $projects
}

function Wiki_GenerateHeaderH2($headerText)
{
    Write-Output "h2." $headerText
}

function Wiki_GenerateTableHeader
{
    Write-Output "{table-plus:sortIcon=true|enableHighlighting=true}"
    Write-Output "|| Modul || Cesta || Vlastník || Popis ||"
}

function Wiki_GenerateTableFooter
{
    Write-Output "{table-plus}"
}

function Wiki_GenerateHREF($url)
{
    Write-Output "[$url]"
}



cls
foreach ($clone in $clones)
{
    $currentPath = $pathTemplate -f $clone
    Debug $currentPath
    
    cd $currentPath
    
    svn up > $null
    
    Wiki_GenerateHeaderH2 ($clone + " klon")
        
    $svnInfo = [xml](svn info --xml)
    $svnURL = $svnInfo.info.entry.url
    Wiki_GenerateHREF($svnURL)
    
    Wiki_GenerateTableHeader
    
    $projects = GetProjects
    foreach ($project in $projects)
    {
        Write-Output "|" $project.BaseName "|" $project.FullName.Replace($currentPath, "") "| | |"
    }
    
    Wiki_GenerateTableFooter
    
    
    Debug "*******************************************"
}
