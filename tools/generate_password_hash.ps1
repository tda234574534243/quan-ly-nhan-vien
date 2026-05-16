<#
Generate a PBKDF2 password hash and print SQL to update a user password.

Usage (PowerShell Core or Windows PowerShell):
  pwsh .\tools\generate_password_hash.ps1 -password "1" -username "ADMIN"

This prints the pbkdf2$... string and a SQL UPDATE statement. Run the SQL in your database (SSMS).
#>

param(
	[Parameter(Mandatory=$false)]
	[string]$password = "1",
	[Parameter(Mandatory=$false)]
	[string]$username = "ADMIN",
	[int]$iterations = 10000,
	[int]$saltSize = 16,
	[int]$hashSize = 32
)

Add-Type -AssemblyName System.Security

$rng = [System.Security.Cryptography.RNGCryptoServiceProvider]::Create()
$salt = New-Object byte[] $saltSize
$rng.GetBytes($salt)

$pbk = New-Object System.Security.Cryptography.Rfc2898DeriveBytes($password, $salt, $iterations)
$hash = $pbk.GetBytes($hashSize)

$saltB64 = [Convert]::ToBase64String($salt)
$hashB64 = [Convert]::ToBase64String($hash)

$final = 'pbkdf2$' + $iterations + '$' + $saltB64 + '$' + $hashB64

Write-Host "Generated hash:" -ForegroundColor Green
Write-Host $final

Write-Host ""
Write-Host "SQL to run in your database (SSMS):" -ForegroundColor Green
Write-Host "UPDATE TAIKHOAN SET MATKHAU = '$final' WHERE TENDANGNHAP = '$username';"

Write-Host ""
Write-Host "After running the UPDATE, try logging in again."
