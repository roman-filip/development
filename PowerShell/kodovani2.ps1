function ReCodeFile($fileName)
{
    write "$fileName will be re-coded to utf8"
    $input = [String]::Join([Environment]::NewLine, (Get-Content $fileName))                
    $input | Out-File -Encoding UTF8 $fileName      
}

Get-Childitem -Filter "*.Designer.cs" -Recurse | Foreach-Object -Process { ReCodeFile($_.FullName) }
