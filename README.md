# Jass Tournament Manager

Jass Tournament Manager is a cross-platform application designed to manage Schieber Jass tournaments.

The system supports tournament organizers in planning, running, and evaluating tournaments, including automatic pairings, score entry, and leaderboard generation.

The application consists of:

- A backend REST API built with ASP.NET Core
- A cross-platform frontend built with .NET MAUI
- A PostgreSQL database for persistent storage

---

# Overview

The system supports multiple user roles with clearly separated responsibilities.

## User Roles

### SYSADMIN

System-wide administrator with full access.

Responsibilities:

- View and manage all tournaments
- Manage organizers
- Manage players
- Support troubleshooting

---

### ORGANIZER

Main operational role.

Responsibilities:

- Create and manage tournaments
- Configure tournament settings
- Register participants
- Generate pairings
- Enter or validate scores
- Run reports
- Import historical tournament data via Excel

Each organizer can only access their own tournaments.

---

### PLAYER

Participant role.

Responsibilities:

- Join tournaments
- Enter scores
- View leaderboards
- Participate via QR-code registration

Players only see tournaments they participate in.

---

# Architecture

The system follows a layered architecture with clear separation of concerns.

```text
┌─────────────────────────────┐
│ Frontend (.NET MAUI)        │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ Backend (.NET Web API)      │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ Application Layer           │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ Domain Layer                │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ Infrastructure Layer        │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│ PostgreSQL Database         │
└─────────────────────────────┘
```


This architecture ensures:

- Maintainability
- Scalability
- Testability
- Separation of business logic and infrastructure

---

# Technology Stack

## Frontend

- .NET MAUI
- MVVM Pattern
- REST API communication
- Cross-platform support

## Backend

- .NET 8 / ASP.NET Core Web API
- Swagger / OpenAPI
- Entity Framework Core
- FluentValidation (planned)

## Database

- PostgreSQL 16
- EF Core migrations
- Optional Redis caching

## DevOps

- Docker
- Docker Compose
- GitHub
- GitHub Actions
- Reverse proxy (Nginx or Traefik)
- TLS via Let's Encrypt

---

# Project Structure

```text
JassTournamentManager
├─ src
│  ├─ JassTournamentManager.Api
│  │   Backend entry point
│  │   Controllers, middleware, Swagger configuration
│  │
│  ├─ JassTournamentManager.Application
│  │   Application logic
│  │   Use cases
│  │   Business services
│  │
│  ├─ JassTournamentManager.Domain
│  │   Core business model
│  │   Entities
│  │   Business rules
│  │
│  ├─ JassTournamentManager.Infrastructure
│  │   Technical implementations
│  │   Database access (EF Core)
│  │   External services
│  │
│  ├─ JassTournamentManager.Contracts
│  │   DTOs
│  │   API request and response models
│  │
│  └─ JassTournamentManager.App
│      Cross-platform frontend (.NET MAUI)
│
├─ tests
│   Unit and integration tests
│
├─ docs
│  ├─ requirements.md
│  ├─ architecture.md
│  ├─ database-schema.md
│
└─ JassTournamentManager.sln
```

---

# Domain Structure

The core tournament hierarchy is structured as follows:

```text
Tournament
  └── Round
        └── Game
              ├── GameParticipant
              └── GameScore
```

## Game Rules

Each game consists of:

- 4 players
- 2 teams
- 157 total points per game
- Optional match bonus (+100 points)

Scores are automatically calculated when only one team enters points.

---

# Core Features

## Tournament Management

- Create tournaments
- Configure tournament settings
- Manage tournament lifecycle

## Participant Management

- Register participants
- QR-code-based participation
- Import historical data via Excel

## Pairing Management

- Manual pairings
- Automatic pairing generation
- Player partner confirmation

## Score Management

- Score entry
- Automatic score validation
- Match bonus calculation

## Leaderboards

- Live rankings
- Tournament statistics
- Organizer-level reports

---

# Data Model Overview

Core entities include:

User  
Tournament  
TournamentConfigTemplate  
TournamentConfig  
TournamentParticipant  
Round  
Game  
GameParticipant  
GameScore  
Table  

Relationship overview:
```text
User  
 └── Tournament  
       └── Round  
             └── Game  
                   ├── GameParticipant  
                   └── GameScore 
```		   

---

# Prerequisites

Required software:

- .NET SDK
- Visual Studio 2022 or newer
- .NET MAUI workload (Windows)

Optional:

- PostgreSQL
- Docker

---

# Running the Project

## Start Backend

dotnet run --project src/JassTournamentManager.Api

Swagger UI:

http://localhost:5272/swagger

---

## Start Frontend

In Visual Studio:

1. Set `JassTournamentManager.App` as Startup Project  
2. Select target platform (e.g. Windows Machine)  
3. Start debugging (F5)  

---

# Development Guidelines

## Architectural Principles

- Layered architecture
- Domain-driven design concepts
- Dependency Injection
- Clear separation of responsibilities
- REST-based communication

Dependency direction:

```text
Api → Application → Domain  
             ↓  
       Infrastructure  
```

The Domain layer must not depend on technical frameworks.

---

# Security

- JWT-based authentication
- Role-based authorization
- HTTPS enforcement
- Organizer-level isolation
- Input validation on client and server

---

# Scalability Strategy

Planned scaling mechanisms:

- Multiple backend instances
- PostgreSQL replication
- Redis caching
- Containerized deployments

---

# Roadmap

## Phase 1

- User management
- Tournament management
- Participant management

## Phase 2

- Automatic pairings
- Score entry
- Leaderboards

## Phase 3

- Excel import
- Advanced reporting
- Performance optimizations

## Phase 4

- Multi-organizer support
- Scaling improvements
- Monitoring and logging

---

# Documentation

Additional documentation is available in:

- docs/requirements.md
- docs/architecture.md
- docs/database-schema.md

These documents provide detailed functional and technical specifications.

---

# License

This project is licensed under the MIT License — see the LICENSE file for details.