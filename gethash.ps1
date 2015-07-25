$outFile = "files.list"
Remove-Item $outFile
$workdir = pwd
$fileList = Get-FileHash Background\*.* -Algorithm MD5
foreach($line in $fileList)
{
	$path = $line.Path
	$path = $path.replace("$workdir\", "").replace("\", "/")
	$hash = $line.Hash
	"$path, $hash" | Out-File $outFile -Encoding "utf8" -Append
}
