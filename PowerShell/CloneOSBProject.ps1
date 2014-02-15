# Parameters
$debug = $true
$sourceSVN = "https://subversion.homecredit.net/repos/esb/osb/projects/id"
# $sourceSVN = "https://subversion.homecredit.net/repos/esb/osb/projects/id@3474"
$destFolder = "C:\\_svn\\osb\\HCI\\ph5"
$oldCountryCode = "id"
$newCountryCode = "ph"
$oldCountryName = "Indonesia"
$newCountryName = "Philippines"
$releaseCode = "PH_2013_01"

# Functions
function Debug($message)
{
    if ($debug)
    {
        Write-Host -ForegroundColor Gray $message
    }
}

function Get-FileEncoding([string]$Path)
{ 
    [byte[]]$byte = get-content -Encoding byte -ReadCount 4 -TotalCount 4 -Path $Path
 
    # EF BB BF (UTF8)
    if ( $byte[0] -eq 0xef -and $byte[1] -eq 0xbb -and $byte[2] -eq 0xbf )
    { return 'UTF8' }
 
    # FE FF (UTF-16 Big-Endian)
    elseif ($byte[0] -eq 0xfe -and $byte[1] -eq 0xff)
    { return 'Unicode UTF-16 Big-Endian' }
     
    # FF FE (UTF-16 Little-Endian)
    elseif ($byte[0] -eq 0xff -and $byte[1] -eq 0xfe)
    { return 'Unicode UTF-16 Little-Endian' }
     
    # 00 00 FE FF (UTF32 Big-Endian)
    elseif ($byte[0] -eq 0 -and $byte[1] -eq 0 -and $byte[2] -eq 0xfe -and $byte[3] -eq 0xff)
    { return 'UTF32 Big-Endian' }
     
    # FE FF 00 00 (UTF32 Little-Endian)
    elseif ($byte[0] -eq 0xfe -and $byte[1] -eq 0xff -and $byte[2] -eq 0 -and $byte[3] -eq 0)
    { return 'UTF32 Little-Endian' }
     
    # 2B 2F 76 (38 | 38 | 2B | 2F)
    elseif ($byte[0] -eq 0x2b -and $byte[1] -eq 0x2f -and $byte[2] -eq 0x76 -and ($byte[3] -eq 0x38 -or $byte[3] -eq 0x39 -or $byte[3] -eq 0x2b -or $byte[3] -eq 0x2f) )
    { return 'UTF7' }
     
    # F7 64 4C (UTF-1)
    elseif ( $byte[0] -eq 0xf7 -and $byte[1] -eq 0x64 -and $byte[2] -eq 0x4c )
    { return 'UTF-1' }
     
    # DD 73 66 73 (UTF-EBCDIC)
    elseif ($byte[0] -eq 0xdd -and $byte[1] -eq 0x73 -and $byte[2] -eq 0x66 -and $byte[3] -eq 0x73)
    { return 'UTF-EBCDIC' }
     
    # 0E FE FF (SCSU)
    elseif ( $byte[0] -eq 0x0e -and $byte[1] -eq 0xfe -and $byte[2] -eq 0xff )
    { return 'SCSU' }
     
    # FB EE 28 (BOCU-1)
    elseif ( $byte[0] -eq 0xfb -and $byte[1] -eq 0xee -and $byte[2] -eq 0x28 )
    { return 'BOCU-1' }
     
    # 84 31 95 33 (GB-18030)
    elseif ($byte[0] -eq 0x84 -and $byte[1] -eq 0x31 -and $byte[2] -eq 0x95 -and $byte[3] -eq 0x33)
    { return 'GB-18030' }
     
    else
    { return 'ASCII' }
}

function DeleteDestFolder()
{
    Debug "DeleteDestFolder"

    if (Test-Path $destFolder)
    {
        Remove-Item $destFolder -Recurse -Force
    }
    
    Debug "*********************************************************"
}

function ReplaceTextInFile([string]$fileName, [string]$oldString, [string]$newString)
{
    $fileContent =
        (Get-Content $fileName -Raw) |
        Foreach-Object {$_ -creplace $oldString, $newString}
    
    #Set-Content $fileName
    
    $fileEncoding = Get-FileEncoding($fileName);
    if ($fileEncoding -eq "UTF8")
    {
        [System.IO.File]::WriteAllText($fileName, $fileContent, [System.Text.Encoding]::UTF8)
    }
    else
    {
        [System.IO.File]::WriteAllText($fileName, $fileContent, [System.Text.Encoding]::Default)
    }
}

function CreateUpdates()
{
    Debug "CreateUpdates"
    
    $destFolder2 = $destFolder + "\\trunk\\configuration\\"
    $updates = 
        Get-ChildItem $destFolder -Include customization -Directory -Recurse |
        ForEach-Object {$_.fullname -creplace "\\customization", ""} |
        ForEach-Object {$_ -creplace $destFolder2, ""} |
        ForEach-Object {$_ -creplace "\\", "/"}
    
    $updatesFile = $destFolder2 + "deployment\\updates"
    Set-Content $updatesFile $updates
    
    Debug "*********************************************************"
}

function ChangeCountryCodes()
{
    Debug "ChangeEndpoints"
    
    # .project
    $projectFile = $destFolder + "\\trunk\\configuration\\.project"
    ReplaceTextInFile $projectFile (">" + $oldCountryCode + "<") (">" + $newCountryCode + "<")
    
    # .settings
    $settingsFile = $destFolder + "\\trunk\\configuration\\.settings\\org.eclipse.wst.common.component"
    ReplaceTextInFile $settingsFile "`"$oldCountryCode`"" "`"$newCountryCode`""
    
    # Endpoints etc
    foreach ($f in Get-ChildItem $destFolder -Include *.biz, *.proxy, *.xml, *.txt  -Recurse)
    {
        ReplaceTextInFile $f.fullname ($oldCountryCode + "/") ($newCountryCode + "/")
        ReplaceTextInFile $f.fullname ($oldCountryCode + "_") ($newCountryCode + "_")
        ReplaceTextInFile $f.fullname ("\." + $oldCountryCode) ("." + $newCountryCode)
        ReplaceTextInFile $f.fullname ($oldCountryCode + "\.") ($newCountryCode + ".")
    }

    # instructions.txt
    foreach ($f in Get-ChildItem $destFolder -Include instructions.txt  -Recurse)
    {
        ReplaceTextInFile $f.fullname ($oldCountryCode.toUpper() + "_") ($newCountryCode.toUpper() + "_")
        ReplaceTextInFile $f.fullname $oldCountryName $newCountryName
        ReplaceTextInFile $f.fullname "HoSel 1.1:" ($newCountryName + " - first release:")
    }

    # customize_all.xml
    foreach ($f in Get-ChildItem $destFolder -Include customize_all.xml  -Recurse)
    {
        ReplaceTextInFile $f.fullname "\-id" "-ph"
    }

    # *.properties
    foreach ($f in Get-ChildItem $destFolder -Include "*.properties"  -Recurse)
    {
        ReplaceTextInFile $f.fullname ("\." + $oldCountryCode) ("." + $newCountryCode)
    }

    # dokumentace
    foreach ($f in Get-ChildItem $destFolder -Include *.xml -File -Recurse)
    {
        ReplaceTextInFile $f.fullname "<since>[a-zA-Z_.0-9( )]*</since>" "<since>$releaseCode</since>"
    }
    
    Debug "*********************************************************"
}

# ************************** Begin SVN ******************************
function CheckOutFromSVN()
{
    Debug "CheckOutFromSvn"
    
    svn checkout $sourceSVN $destFolder --ignore-externals
    
    Debug "*********************************************************"
}

function RemoveSVNMess()
{
    Debug "RemoveSVNMess"

    Get-ChildItem $destFolder -Include .svn -Directory -Force -Recurse | Remove-Item -Recurse -Force

    Debug "*********************************************************"
}
# ************************** End SVN ******************************


# Start

cls

Debug "Start"
Debug "Configuration:"
Debug "  sourceSVN = $sourceSVN"
Debug "  destFolder = $destFolder"

DeleteDestFolder
CheckOutFromSVN
RemoveSVNMess
ChangeCountryCodes
CreateUpdates


Debug "Done!!!"