This README explains how to build and run the `MvcReadMe_Group4` Docker image locally.

Build locally
1. From the repository root run (PowerShell):
   ```powershell
   docker build -t libroverse/mvcreadme:local -f MvcReadMe_Group4/Dockerfile .
   ```

Run locally
1. Start a container and map port 8080:
   ```powershell
   docker run --rm -e ASPNETCORE_ENVIRONMENT=Production -e ConnectionStrings__MvcReadMe_Group4Context='Data Source=app.db' -p 8080:8080 libroverse/mvcreadme:local
   ```

Notes on connection string / environment variables
- For production you'll want a MySQL database. Example environment variables for MySQL:
  - `ConnectionStrings__MvcReadMe_Group4Context`: `server=host;port=3306;user=root;password=secret;database=mvcreadme_db;SslMode=None` 
- The app reads `appsettings.json` by default. When running in Docker pass a connection string via environment variables as above.
