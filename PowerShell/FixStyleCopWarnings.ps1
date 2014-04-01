$newLineConst = [Environment]::NewLine + [Environment]::NewLine
$openCurlyConst = '{' + [Environment]::NewLine
$closeCurlyConst = [Environment]::NewLine + '}'

$items = Get-ChildItem -Path .\ -Recurse *.cs | Select-Object -Property FullName
$cntFixedFiles = 0
$cntAllFiles = $items.Count
foreach($item in $items)
{
      $path = ($item | Get-Member 'FullName') -replace 'System.String FullName=', ''
      $arr = Get-Content $path;
      if($arr -eq $null)
      {
            continue
      }
      
      $input = [String]::Join([Environment]::NewLine, $arr)
      $cleared = $input -replace '([ \t]*\r\n){3,}', $newLineConst
      $cleared = $cleared -replace '(\{(?:\r\n){2,})', $openCurlyConst
      $cleared = $cleared -replace '(\r\n[\t ]*){2,}\}', $closeCurlyConst
      if($cleared -ne $input)
      {
            write "$path has been modified"
            $cleared | Out-File -Encoding UTF8 $path
            $cntFixedFiles++
      }
}
write "Fixed files: $cntFixedFiles / $cntAllFiles"
