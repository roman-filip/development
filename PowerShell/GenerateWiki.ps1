$clones = @("BY", "CS", "KZ", "VN", "CN", "MN")
$pathTemplate = "C:\_svn\dotnet\{0}\trunk"
$debug = $false

function Debug($message)
{
	if ($debug)
	{
		Write-Host -ForegroundColor Gray $message
	}
}

function GetProjects
{
	$projects = Get-ChildItem ./ -Recurse -Filter "*.csproj" | Sort-Object
	return $projects
}

function Wiki_GenerateHeaderH2($headerText)
{
	Write-Host "h2." $headerText
}

function Wiki_GenerateTableHeader
{
	Write-Host "{table-plus:sortIcon=true|enableHighlighting=true}"
	Write-Host "|| Modul || Cesta || Vlastník || Popis ||"
}

function Wiki_GenerateTableFooter
{
	Write-Host "{table-plus}"
}

function Wiki_GenerateHREF($url)
{
	Write-Host "[$url]"
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
		Write-Host "|" $project.BaseName "|" $project.FullName.Replace($currentPath, "") "| | |"
	}
	
	Wiki_GenerateTableFooter
	
	
	Debug "*******************************************"
}
