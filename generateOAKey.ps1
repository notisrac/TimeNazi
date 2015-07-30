Param(
  [Parameter(Mandatory=$True,Position=1)]
  [string]$OutFile,
  [Parameter(Mandatory=$True)]
  [string]$ClientId,
  [Parameter(Mandatory=$True)]
  [string]$ClientSecret,
  [Parameter(Mandatory=$True)]
  [string]$RedirectUri,
  [string]$Scope = "https://www.googleapis.com/auth/drive.file https://spreadsheets.google.com/feeds",
  [string]$CryptoAssemblyPath = "TimeNazi\bin\Debug\nUtils.Crypto.dll"
)

Write-Host "OAuth key file generator"
Write-Host "noti, 2015"
Write-Host ""

$path = Convert-Path $CryptoAssemblyPath
$bytes = [System.IO.File]::ReadAllBytes($path)
[System.Reflection.Assembly]::Load([byte[]]$bytes) | Out-Null

Write-Host "Assembling..." -NoNewLine
$contents  = "<?xml version=`"1.0`" encoding=`"utf-8`"?>`r`n"
$contents += "<OAuth2Parameters xmlns:xsi=`"http://www.w3.org/2001/XMLSchema-instance`" xmlns:xsd=`"http://www.w3.org/2001/XMLSchema`">`r`n"
$contents += "  <ClientId>$($ClientId)</ClientId>`r`n"
$contents += "  <ClientSecret>$($ClientSecret)</ClientSecret>`r`n"
$contents += "  <RedirectUri>$($RedirectUri)</RedirectUri>`r`n"
$contents += "  <AccessType>offline</AccessType>`r`n"
$contents += "  <ResponseType>code</ResponseType>`r`n"
$contents += "  <ApprovalPrompt>auto</ApprovalPrompt>`r`n"
$contents += "  <Scope>$($Scope)</Scope>`r`n"
$contents += "  <TokenUri>https://accounts.google.com/o/oauth2/token</TokenUri>`r`n"
$contents += "  <AuthUri>https://accounts.google.com/o/oauth2/auth</AuthUri>`r`n"
$contents += "  <AccessCode></AccessCode>`r`n"
$contents += "  <AccessToken></AccessToken>`r`n"
$contents += "  <TokenType>Bearer</TokenType>`r`n"
$contents += "  <RefreshToken></RefreshToken>`r`n"
$contents += "  <TokenExpiry>0001-01-01T00:00:00</TokenExpiry>`r`n"
$contents += "</OAuth2Parameters>"
Write-Host "done"

Write-Host "Encoding..." -NoNewLine
$encodedContents = [nUtils.Crypto.CryptoUtils]::SimpleEncrypt($contents)
Write-Host "done"

$OutFile = "$($OutFile).oakey"

Write-Host "Writing file `"$($OutFile)`"..." -NoNewLine
$encodedContents | Out-File "$($OutFile)"
Write-Host "done"
Write-Host ""
Write-Host ""