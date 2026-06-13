# Jass Tournament Manager

Cross-platform application for managing Schieber Jass tournaments.

The system helps tournament organizers plan, run and evaluate Jass tournaments, including participant registration, pairing generation, score entry and leaderboards.

## Tech Stack

- Frontend: .NET MAUI
- Backend: ASP.NET Core Web API
- Database: PostgreSQL
- ORM/Migrations: Entity Framework Core
- API Documentation: Swagger / OpenAPI

## Main Features

- Tournament management
- Participant registration
- Tournament-code based joining
- Manual and automatic pairings
- Score entry with automatic 157-point calculation
- Optional match bonus
- Leaderboards and reports
- Excel import for historical tournament data

## User Roles

- SYSADMIN: Can access and manage all data for support purposes
- ORGANIZER: Can create and manage own tournaments
- PLAYER: Can join tournaments, enter scores and view allowed results

Users can see tournaments if they are either the organizer of the tournament or registered as a participant in that tournament. SYSADMIN users can see all tournaments.

## Project Structure

```text
JassTournamentManager
├─ src
│  ├─ JassTournamentManager.Api
│  ├─ JassTournamentManager.Application
│  ├─ JassTournamentManager.Domain
│  ├─ JassTournamentManager.Infrastructure
│  ├─ JassTournamentManager.Contracts
│  └─ JassTournamentManager.App
├─ tests
├─ docs
│  ├─ requirements.md
│  ├─ architecture.md
│  └─ database-schema.md
└─ JassTournamentManager.sln
```

## Backend Commands

All `dotnet` commands target the backend solution file:

```bash
# Restore dependencies
dotnet restore JassTournamentManager.Backend.slnx

# Build
dotnet build JassTournamentManager.Backend.slnx --configuration Release

# Run all tests
dotnet test JassTournamentManager.Backend.slnx

# Run tests for a single project
dotnet test tests/JassTournamentManager.Domain.Tests

# Run a single test by name filter
dotnet test tests/JassTournamentManager.Domain.Tests --filter "FullyQualifiedName~Tournament.Constructor"

# Start the API locally (requires a running PostgreSQL on port 5433)
dotnet run --project src/JassTournamentManager.Api
```

Swagger UI (local): `http://localhost:5272/swagger`

## Docker

`docker-compose.yml` starts PostgreSQL (host port 5433) and the API (host port 8080). `POSTGRES_PASSWORD` must be provided via a `.env` file:

```bash
# One-time setup
echo "POSTGRES_PASSWORD=your_password" > .env

# Start all services
docker compose up -d

# Stop all services
docker compose down

# Stop and remove volumes (resets the database)
docker compose down -v
```

To run only the database in Docker and the API locally:

```bash
docker compose up postgres -d
dotnet run --project src/JassTournamentManager.Api
```

The connection string is managed via [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) and is not committed to the repository. Set it once per developer:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Host=localhost;Port=5433;Database=jass_tournament_manager;Username=jass_app;Password=your_password" \
  --project src/JassTournamentManager.Api
```

## Database Migrations

Create a new EF Core migration from the repository root in PowerShell:

```powershell
dotnet ef migrations add NameOfMigration `
  --project src\JassTournamentManager.Infrastructure\JassTournamentManager.Infrastructure.csproj `
  --startup-project src\JassTournamentManager.Api\JassTournamentManager.Api.csproj `
  --context JtmDbContext `
  --output-dir Persistence/Migrations
```

Apply pending migrations to the local database:

```powershell
docker compose up postgres -d

dotnet ef database update `
  --project src\JassTournamentManager.Infrastructure\JassTournamentManager.Infrastructure.csproj `
  --startup-project src\JassTournamentManager.Api\JassTournamentManager.Api.csproj `
  --context JtmDbContext
```

Remove the latest migration if it has not been applied to the database yet:

```powershell
dotnet ef migrations remove `
  --project src\JassTournamentManager.Infrastructure\JassTournamentManager.Infrastructure.csproj `
  --startup-project src\JassTournamentManager.Api\JassTournamentManager.Api.csproj `
  --context JtmDbContext
```

Downgrade the local database to an earlier migration. `TargetMigrationName` is the migration that should remain applied after the downgrade:

```powershell
dotnet ef database update TargetMigrationName `
  --project src\JassTournamentManager.Infrastructure\JassTournamentManager.Infrastructure.csproj `
  --startup-project src\JassTournamentManager.Api\JassTournamentManager.Api.csproj `
  --context JtmDbContext
```

## Run Frontend

Open the solution in Visual Studio, set `JassTournamentManager.App` as startup project and run it with the desired target platform.

## Documentation

Detailed documentation is maintained in:

- `docs/requirements.md` — functional requirements and business rules
- `docs/architecture.md` — technical architecture and deployment concepts
- `docs/database-schema.md` — data model, entities and constraints

## Development Principles

- Layered architecture
- Domain-driven design concepts
- Clear separation of business logic and infrastructure
- REST-based communication
- Dependency Injection
- Testable domain and application logic

## License

MIT
