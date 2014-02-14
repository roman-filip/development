function ReCodeFile($fileName)
{
                $stream = New-Object System.IO.FileStream($fileName, [System.IO.FileMode]::Open)
                $reader = New-Object System.IO.BinaryReader($stream)
                $firstByte = $reader.ReadByte()
                $secondByte = $reader.ReadByte()       
                $stream.Dispose()
                
                if($firstByte -eq 0xFF -and $secondByte -eq 0xFE)
                {
                               write "$fileName will be re-coded to utf8"
                               $input = [String]::Join([Environment]::NewLine, (Get-Content -Encoding BigEndianUnicode $fileName))                
                               
                               $input | Out-File -Encoding UTF8 $fileName      
                }
}

Get-Childitem -Filter "*.cs" -Recurse | Foreach-Object -Process { ReCodeFile($_.FullName) }
