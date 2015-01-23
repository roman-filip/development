# This script creates backup of tabs open in the Firefox

$fileForBackup = "c:\Users\filip\AppData\Roaming\Mozilla\Firefox\Profiles\w60eo11w.default\sessionstore.js"

$dateExtension = Get-Date -Format yyyyMMdd

$backupFolder = "c:\Temp\Firefox backup"
$backupFileName = "sessionstore_" + $dateExtension
$backupFullPath = join-path -path $backupFolder -childpath $backupFileName


zip -j $backupFullPath $fileForBackup
