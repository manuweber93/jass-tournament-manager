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

## Run Backend

```bash
dotnet run --project src/JassTournamentManager.Api
```

Swagger UI:

```text
http://localhost:5272/swagger
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