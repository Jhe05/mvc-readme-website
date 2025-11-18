<#
Non-interactive transfer script. Intended to be run from the project root.
Will attempt to detect the SQLite DB from appsettings.json and run the transfer tool.
Example: powershell -ExecutionPolicy Bypass -File .\scripts\transfer_noninteractive.ps1 -MysqlPassword ""
#>

param(
    [string]$MysqlPassword = ""
)

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Push-Location $root\..\

# locate sqlite path
$ajs = Join-Path (Get-Location) 'appsettings.json'
if (-not (Test-Path $ajs)) { Write-Host "appsettings.json not found"; Pop-Location; exit 2 }
$j = Get-Content $ajs -Raw | ConvertFrom-Json
$ds = $j.ConnectionStrings.MvcReadMe_Group4Context
if ($ds -match 'Data Source=(.+)') { $sqlitePath = $matches[1] } else { Write-Host "Could not parse Data Source"; Pop-Location; exit 3 }
if (-not (Test-Path $sqlitePath)) { Write-Host "SQLite file not found: $sqlitePath"; Pop-Location; exit 4 }

Write-Host "Detected SQLite: $sqlitePath"

# Wait for MySQL port
Write-Host "Waiting for MySQL on 127.0.0.1:3306 (10s timeout)..."
$start = Get-Date
while (-not (Test-NetConnection -ComputerName 127.0.0.1 -Port 3306 -InformationLevel Quiet)) {
    Start-Sleep -Seconds 1
    if ((Get-Date) - $start -gt [TimeSpan]::FromSeconds(10)) { Write-Host "MySQL not responding on 3306"; break }
}

if ($MysqlPassword -ne '') { $pwdPart = "Password=$MysqlPassword;" } else { $pwdPart = "" }
$mysqlConn = "Server=127.0.0.1;Port=3306;User ID=root;${pwdPart}Database=mvcreadme_db;SslMode=None;"

Write-Host "Running transfer..."
dotnet run --project tools/sqlite_to_mysql -- "$sqlitePath" "$mysqlConn"

Pop-Location
