Backend API (ASP.NET Core, .NET 8)

Quick start (local):
- Ensure .NET 8 SDK is installed.
- From repository root run:
  - `dotnet restore ./backend`
  - `dotnet build ./backend`
- To run with Docker Compose:
  - `docker compose up --build`

Next steps:
- Create initial EF Core migration:
  - `dotnet ef migrations add InitialCreate -p ./backend -s ./backend`
- Update database:
  - `dotnet ef database update -p ./backend -s ./backend`

The project includes ASP.NET Identity configured with EF Core and PostgreSQL provider.
Replace the JWT secret in `backend/appsettings.json` before production.
