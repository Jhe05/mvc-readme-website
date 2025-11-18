<#
Interactive script: builds and runs the transfer tool and prompts for MySQL password.
Run from project root (MvcReadMe_Group4) with PowerShell as administrator or use ExecutionPolicy bypass.
#>

param(
    [string]$MysqlPassword = "",
    [string]$SqlitePath = ""
)

if (-not (Read-Host "Type YES to continue and run the transfer (this will create/overwrite tables in mvcreadme_db)") -eq 'YES') {
    Write-Host "Aborted by user."; exit 1
}

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
Push-Location $root\..\

if (-not $SqlitePath) {
    # try to read appsettings.json
    $ajs = Join-Path (Get-Location) 'appsettings.json'
    if (Test-Path $ajs) {
        $j = Get-Content $ajs -Raw | ConvertFrom-Json
        $ds = $j.ConnectionStrings.MvcReadMe_Group4Context
        if ($ds -match 'Data Source=(.+)') { $SqlitePath = $matches[1] }
    }
}

if (-not (Test-Path $SqlitePath)) {
    Write-Host "Cannot find SQLite DB at: $SqlitePath"; Pop-Location; exit 2
}

if ($MysqlPassword -ne '') { $pwdPart = "Password=$MysqlPassword;" } else { $pwdPart = "" }
$mysqlConn = "Server=127.0.0.1;Port=3306;User ID=root;${pwdPart}Database=mvcreadme_db;SslMode=None;"

Write-Host "SQLite: $SqlitePath"
Write-Host "MySQL: $mysqlConn"

dotnet run --project tools/sqlite_to_mysql -- "$SqlitePath" "$mysqlConn"

Pop-Location
