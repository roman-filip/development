function GetRevision($file)
{
    $fileContent = Get-Content $file.FullName
   
    foreach($line in $fileContent)
    {
        if ($line -match '\$Revision: \d* \$')
        {   # if file contains $Revision then return its number
            if ($Matches[0] -match ' \d*')
            {
                return $Matches[0]
            }
        }
    }
    
    return " NO REVISION"
}

# ************************************** Start **************************************

cls

$directory = $args[0]

foreach ($file in Get-ChildItem $directory -Recurse -Include *.pck, *.spc)
{
    $revision = GetRevision($file)
    Write-Output $file.FullName "-$revision"
}
