# Sqlite -> MySQL Transfer Tool

This small console tool copies schema and data from an existing SQLite database into a MySQL/MariaDB database.

Usage examples:

1) Provide the paths explicitly:

```powershell
dotnet run --project tools/sqlite_to_mysql -- "C:\path\to\MvcReadMe_Group4Context.db" "Server=127.0.0.1;User ID=root;Password=;Database=mvcreadme_db;SslMode=None;"
```

2) Run without args from the solution folder: the tool will search up the directory tree for `appsettings.json`, extract the connection string `MvcReadMe_Group4Context` and use that SQLite file.

Notes:
- The tool makes a best-effort mapping between SQLite types and MySQL types.
- It drops and recreates target tables, then inserts the rows found in SQLite.
- You must run this locally — the tool does not modify your XAMPP installation. Always backup `C:\xampp\mysql\data` before making changes to MySQL.

If you prefer a one-click script, look at `../../scripts/transfer_one_click.ps1` and `../../scripts/transfer_noninteractive.ps1`.
