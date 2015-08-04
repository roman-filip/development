$sourceSVNs = 
    @(
      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs10_sp31_d1/",
      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs10_sp31_d2/",
      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs10_sp31_d3/",
      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs10_sp31_d4/",

      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs11_sp32_d1/",
      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs11_sp32_d2/",
      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs11_sp32_d3/",
      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs11_sp32_d4/",

      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs09",
      "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/branches/cs09_fix")

$targetSVN = "https://subversion.homecredit.net/repos/dotnet/dotnetclient/cs/trunk"

$highlightLogin = "filip"

foreach ($sourceSVN in $sourceSVNs)
{
    Write-Host $sourceSVN -ForegroundColor Magenta
    
    $unmergedRevisions = svn mergeinfo --show-revs eligible $sourceSVN $targetSVN
    if ($unmergedRevisions -ne $null)
    {
        [array]::Reverse($unmergedRevisions)
        
        foreach ($unmergedRevision in $unmergedRevisions)
        {
            $revisionDetail = svn log -r $unmergedRevision $sourceSVN
            
            $splittedRevDetail = $revisionDetail[1].Split("|")
            $login = $splittedRevDetail[1].Trim()
            
            $splittedDateTime = $splittedRevDetail[2].Split("")
            $date = $splittedDateTime[1].Trim()
            $time = $splittedDateTime[2].Trim()
            
            $comment = $revisionDetail[3]
            
            $msg = "$unmergedRevision : $login ($date $time) - $comment"
            
            if ($login -eq $highlightLogin)
            {
                Write-Host $msg -ForegroundColor Green
            }
            else
            {
                Write-Host $msg
            }
        }
    }
    
    Write-Output "**************************************************"
}