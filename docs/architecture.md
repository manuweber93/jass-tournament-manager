# System Architecture: Jass Tournament Manager

## Overview

The Jass Tournament Manager application follows a classic 3-tier architecture with a clear separation between the presentation layer, application/business logic, and data storage.

# Technology Stack

## Frontend

- .NET MAUI (CommunityToolkit.Maui & Uranium UI)
- MVVM Pattern
- REST API communication
- Cross-platform support

## Backend

- .NET 10 / ASP.NET Core Web API
- Swagger / OpenAPI
- Entity Framework Core
- FluentValidation (planned)

## Database

- PostgreSQL 16
- EF Core migrations with Npgsql provider

## DevOps

- Docker
- Docker Compose
- GitHub
- GitHub Actions
- Reverse proxy (Caddy or Nginx or Traefik)
- TLS via Let's Encrypt

---

## Component Description

### Frontend (Presentation Layer)

Main responsibilities:
- Tournament management: create, edit, configure tournaments
- Player management: registration, Excel import, participant management
- Pairing management: manual entry, automated pairing, player self-signup
- Score entry: point entry with automatic calculations (157-point system)
- Leaderboards: live standings, statistics, reports

User roles:
- SYSADMIN: full access for support
- ORGANIZER: manage own tournaments
- PLAYER: enter scores, select partners, view leaderboards

Features:
- Cross-platform UI for desktop and mobile via .NET MAUI
- QR-code generation for tournament check-in
- Configurable score visibility
- Excel import for historical data (see backend service)

---

### Backend (Application Layer)

Services and responsibilities:

1) Authentication Service
- User registration & login
- JWT issuance and validation
- Role-based access control (RBAC)
- Password hashing via ASP.NET Core Identity (secure PBKDF2-based hasher)

2) Tournament Service
- CRUD operations for tournaments
- Tournament configuration (rounds, games, match-bonus, etc.)
- QR-code generation for check-in
- Tournament status management

3) Participant Service
- Participant management and registration
- Excel import handling (server-side parsing)
- Email-based matching and lookups
- Player statistics

4) Game Service
- Round and game management
- Pairing logic (manual and automatic)
- Player self-signup and partner selection
- Game status tracking

5) Score Service
- Score entry and validation
- Automatic calculations (157-point system)
- Match-bonus logic (+100 when applicable)

6) Leaderboard Service
- Compute standings and leaderboards
- Aggregated statistics per organizer and tournament

7) Excel Import Service
- Parse Excel files using .NET libraries (e.g. ClosedXML or EPPlus)
- Validate and transform incoming data
- Bulk insert with transactions

API design:
- RESTful endpoints, JSON request/response
- Standardized error model
- Input validation using DataAnnotations and/or FluentValidation
- OpenAPI/Swagger documentation

---

### Data Layer

Business rules:
- Organizer isolation (each organizer sees only their own tournaments, SYSADMIN exception)
- Automatic score calculation where applicable

Performance considerations:
- Strategic indexes on frequently queried columns (organizerId, tournamentId, userId, status)
- Query optimization for organizer-specific views and leaderboard calculations

---

## Security Considerations

Authentication & authorization:
- JWT-based auth (tokens sent via Authorization header or HTTP-only cookies)
- Role-based access control (SYSADMIN, ORGANIZER, PLAYER)
- Password hashing via ASP.NET Core Identity (recommended) or a vetted hashing algorithm

API security:
- HTTPS/TLS enforced
- Proper CORS configuration
- Input validation on both client and server

Data protection / privacy:
- Organizer isolation and least-privilege access
- SYSADMIN access limited to support needs
- Players see only tournaments they participate in

---

Backup strategy:
- Regular PostgreSQL backups (pg_dump or base backups)
- Daily retention/rotation (e.g. 7 days) and offsite copies
- Persistent volumes for database containers

---

## Scalability

Horizontal scaling:
- Backend: multiple API instances behind a load balancer
- Database: PostgreSQL replication and read replicas for heavy read workloads
- Caching: Redis for sessions, rate-limiting and hot data

Vertical scaling:
- Increase CPU/RAM / use faster storage (SSD) for DB when needed

Performance optimizations:
- Database indexes (see database-schema.md)
- API response caching for non-critical endpoints
- Efficient leaderboard aggregation (pre-aggregation where necessary)
- Lazy loading and prudent resource usage in MAUI client

---
