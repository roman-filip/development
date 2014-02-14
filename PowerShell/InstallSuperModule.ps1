if ($args.Length -ne 1)
{
    return "ERROR: missing name of supermodule"
}

$supermoduleNameWithoutExt = $args[0]

# Get country code from name of supermodule
$code = $supermoduleNameWithoutExt[2]
switch ($code)
{
    "B" {$countryCode = "BY"}
    "F" {$countryCode = "CS"}
    "K" {$countryCode = "KZ"}
    "V" {$countryCode = "VN"}
    default {return "ERROR: unsupported name of supermodule"}
}

# Get the newest supermodule for unzipping
$supremodulesDir = "\\proliant03dev\HomeR_Deploy\" + $countryCode + "\Supermoduls\"
$supermoduleVersion = $supermoduleNameWithoutExt.substring(15, 3)
$searchFilter = "SuperModul_" + $countryCode + $supermoduleVersion + "_*.zip"
$zippedSupermodule = Get-ChildItem $supremodulesDir -Filter $searchFilter | sort -Property LastWriteTime -Descending | select-object -first 1
if ($zippedSupermodule -eq $null)
{
    return "ERROR: Supermodule with name " + $searchFilter + " doesn't found"
}

# Unzip supermodule
$unzipPath = "C:\_svn\dotnet\DLLs\"
$fileToUnzip = $supremodulesDir + $zippedSupermodule
unzip -o $fileToUnzip -d $unzipPath

# Register supermodule
$supermoduleName = $unzipPath + $supermoduleNameWithoutExt + ".dll"
regsvr32 $supermoduleName