# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Key Documentation

Read these before making changes:

- `README.md` — commands (dotnet & docker), project structure, tech stack
- `AGENTS.md` — coding and testing conventions for agents (read first)
- `docs/architecture.md` — layer dependency rules and component responsibilities
- `docs/conventions.md` — test naming format and EF Core configuration rules
- `docs/requirements.md` — functional requirements and business rules
- `docs/database-schema.md` — entity definitions, constraints, and business rules

## Solution Files

There are two solution files. Use `JassTournamentManager.Backend.slnx` for all backend `dotnet` commands (build, test, restore) — this is what CI uses. The full `JassTournamentManager.slnx` also includes the MAUI frontend.

## Architecture

The backend follows a layered architecture with Domain as the innermost layer. For the exact project dependency rules see `docs/architecture.md`.

- **Domain**: Entities, value objects, enums, domain services. No framework dependencies. Enforces all business invariants via constructors and `Guard`.
- **Contracts**: Request/response DTOs shared between Api and the MAUI App. May use Domain enums; must not expose domain entities.
- **Application**: Use cases and application services (interface + implementation). Defines repository interfaces, `IUnitOfWork`, and the `Result<T>` / `Error` pattern (see `docs/conventions.md`).
- **Infrastructure**: EF Core `JtmDbContext`, `*Configuration.cs` per entity, repository implementations, migrations. Entity relationships are configured on the **parent side only** (see `docs/conventions.md`).
- **Api**: ASP.NET Core composition root. Wires DI via `AddApplication()` / `AddInfrastructure()`.
