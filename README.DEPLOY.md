Deployment guide for LibroVerse (MvcReadMe_Group4)

This repository contains a `Dockerfile` and a `fly.toml` configured for Fly.io deployments, plus a GitHub Actions workflow that will run `flyctl deploy` on push to `main`.

What I prepared for you
- `MvcReadMe_Group4/Dockerfile` (multi-stage build)
- `fly.toml` (app name `libroverse`, ports set to 80/443 -> internal 8080)
- `.github/workflows/deploy-fly.yml` (runs `flyctl deploy` using `FLY_API_TOKEN` secret)
- Startup safety: `Program.cs` attempts to create `Favorites` and `Comments` tables at startup if missing.

What you need to do (one-time)
1. Create a Fly account and generate an API token:
   - Visit https://fly.io and sign up
   - `flyctl auth login` locally to confirm access
   - Create a token: https://fly.io/account/personal_tokens

2. Add secrets in your GitHub repository settings (Settings → Secrets → Actions):
   - `FLY_API_TOKEN`: the token you generated
   - Optionally: `CONNECTION_STRINGS__MvcReadMe_Group4Context` (your production DB connection string). Example for MySQL:
     `server=HOST;port=3306;user=USER;password=PASSWORD;database=mvcreadme_db;SslMode=None`

3. Push your code to the `main` branch. The workflow will call `flyctl deploy` and Fly will build the Docker image and create the release.

Manual deploy (local)
```powershell
# Install flyctl (PowerShell)
iwr https://fly.io/install.ps1 -UseBasicParsing | iex

# Login and create app
flyctl auth login
flyctl apps create libroverse

# Set production DB connection string (run locally to avoid sharing secrets here):
flyctl secrets set ConnectionStrings__MvcReadMe_Group4Context="server=...;user=...;password=...;database=mvcreadme_db;SslMode=None"

# Deploy
flyctl deploy --config fly.toml --dockerfile MvcReadMe_Group4/Dockerfile

# Check status
flyctl status libroverse
```

Security notes
- Do not put production DB passwords in plaintext in your repo.
- Use Fly secrets and GitHub Actions secrets for credentials.

If you want, I can help verify the deployment and run smoke checks once you paste the Fly app URL or the GitHub Actions run link in this chat.
