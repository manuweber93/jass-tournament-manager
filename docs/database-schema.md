
# Data Model: Jass Tournament Manager

## Entity Relationship Diagram

```mermaid
erDiagram
    User ||--o{ Tournament : "organizes"
    User ||--|| TournamentConfigTemplate : "manages"
    User ||--o{ TournamentParticipant : "participates as"
    User ||--o{ JassTable : "owns"
    User ||--o{ Game : "enters result of"
    User ||--o{ PairingParticipant : "enters"
    User o|--o{ User : "is merged into"

    Tournament ||--|| TournamentConfig : "has"
    TournamentConfigTemplate ||--o{ TournamentConfig : "serves as template for"

    Tournament ||--o{ TournamentParticipant : "has"
    Tournament ||--o{ Round : "consists of"

    Round ||--o{ Pairing : "has"

    JassTable ||--o{ Pairing : "hosts"

    Pairing ||--o{ PairingParticipant : "has"
    TournamentParticipant ||--o{ PairingParticipant : "appears as"

    Pairing ||--o{ Game : "consists of"
```

## Entities

### 1. User
**Description**: Authenticated users of the system (organizers and players)

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| email | String | Email address | UNIQUE, NOT NULL |
| passwordHash | String | Hashed password | NOT NULL |
| firstName | String | First name | NOT NULL |
| lastName | String | Last name | NOT NULL |
| isSysAdmin | Boolean | Whether user is sysAdmin | NOT NULL, DEFAULT false |
| sourceType | Enum | How was the user created? | NOT NULL|
| mergedIntoUserId | UUID | merge target user id | |
| mergedAt | DateTime | merge time | |
| mergedBy | UUID | user id of merging user | FK |
| createdAt | DateTime | Creation timestamp | NOT NULL |
| updatedAt | DateTime | Update timestamp | NOT NULL |

**Enums**:
- `sourceTypes`: `MANUAL`, `EXCEL_IMPORT`, `SELF_REGISTERED`

---

### 2. Tournament
**Description**: A Jass tournament created by an organizer

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| organizerId | UUID | Organizer | FK, NOT NULL |
| name | String | Tournament name | NOT NULL |
| location | String | Venue/location | |
| date | Date | Tournament date | NOT NULL |
| status | Enum | Tournament status | NOT NULL, DEFAULT 'ACTIVE' |
| tournamentCode | String | Code which is used to join tournament | UNIQUE |
| createdAt | DateTime | Creation timestamp | NOT NULL |
| updatedAt | DateTime | Update timestamp | NOT NULL |

**Enums**:
- `status`: `ACTIVE`, `COMPLETED`, `CANCELLED`

---

### 3. TournamentConfigTemplate
**Description**: Reusable configuration templates for tournaments

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| organizerId | UUID | Organizer | FK, NOT NULL |
| numberOfRounds | Integer | Number of rounds | NOT NULL, DEFAULT 5 |
| gamesPerRound | Integer | Games per round | NOT NULL, DEFAULT 8 |
| matchBonusEnabled | Boolean | Match bonus enabled | NOT NULL, DEFAULT true |
| fixedTeams | Boolean | Fixed teams | NOT NULL, DEFAULT false |
| scoreVisibility | Enum | Score visibility | NOT NULL, DEFAULT 'ALWAYS_VISIBLE_FOR_EVERYONE' |
| createdAt | DateTime | Creation timestamp | NOT NULL |
| updatedAt | DateTime | Update timestamp | NOT NULL |

**Enums**:
- `scoreVisibility`: `ALWAYS_VISIBLE_FOR_EVERYONE`, `HIDDEN_DURING_ACTIVE_TOURNAMENT`, `ORGANIZER_ONLY`

**Note**: Applied to newly created tournaments (but changes here have no effect on existing tournaments)

---

### 4. TournamentConfig
**Description**: Concrete configuration for a specific tournament (copy of a template)

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| tournamentId | UUID | Tournament | FK, UNIQUE, NOT NULL |
| templateId | UUID | Original template | FK (optional) |
| numberOfRounds | Integer | Number of rounds | NOT NULL, DEFAULT 5 |
| gamesPerRound | Integer | Games per round | NOT NULL, DEFAULT 8 |
| matchBonusEnabled | Boolean | Match bonus enabled | NOT NULL, DEFAULT true |
| fixedTeams | Boolean | Fixed teams | NOT NULL, DEFAULT false |
| scoreVisibility | Enum | Score visibility | NOT NULL, DEFAULT 'ALWAYS_VISIBLE_FOR_EVERYONE' |
| createdAt | DateTime | Creation timestamp | NOT NULL |
| updatedAt | DateTime | Update timestamp | NOT NULL |

**Enums**:
- `scoreVisibility`: `ALWAYS_VISIBLE_FOR_EVERYONE`, `HIDDEN_DURING_ACTIVE_TOURNAMENT`, `ORGANIZER_ONLY`

---

### 5. TournamentParticipant
**Description**: Players participating in a tournament

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| tournamentId | UUID | Tournament | FK, NOT NULL |
| userId | UUID | User | FK, NOT NULL |
| role | Enum | Role | NOT NULL |
| isPlaying | Boolean | Indicates whether the participant plays in the tournament | NOT NULL, DEFAULT true|
| registrationMethod | Enum | Registration method | NOT NULL |
| registeredAt | DateTime | Registration timestamp | NOT NULL |
| createdAt | DateTime | Creation timestamp | NOT NULL |
| updatedAt | DateTime | Update timestamp | NOT NULL |

**Enums**:
- `registrationMethod`: `BY_ORGANIZER`, `VIA_TOURNAMENT_CODE`, `EXCEL_IMPORT`
- `role`: `ORGANIZER`, `PLAYER`

**Constraints**: UNIQUE(tournamentId, userId)

**Note**: Email, firstName, lastName are stored in the `User` table

---

### 6. Round
**Description**: A round within a tournament

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| tournamentId | UUID | Tournament | FK, NOT NULL |
| roundNumber | Integer | Round number (1-based) | NOT NULL |
| status | Enum | Round status | NOT NULL, DEFAULT 'PENDING' |
| createdAt | DateTime | Creation timestamp | NOT NULL |
| updatedAt | DateTime | Update timestamp | NOT NULL |

**Enums**:
- `status`: `PENDING`, `ACTIVE`, `COMPLETED`

**Constraints**: UNIQUE(tournamentId, roundNumber)

---

### 7. JassTable
**Description**: Predefined jass tables for an organizer (reusable)

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| organizerId | UUID | Organizer | FK, NOT NULL |
| name | String | Jass table name/label | NOT NULL |
| displayOrder | Integer | Sort order | NOT NULL |
| isActive | Boolean | Table active | NOT NULL, DEFAULT true |
| createdAt | DateTime | Creation timestamp | NOT NULL |
| updatedAt | DateTime | Update timestamp | NOT NULL |

**Constraints**: UNIQUE(organizerId, name)

---

### 8. Pairing
**Description**: A single pairing consisting of four players within a round which plays a certain amount of games at a table.

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| roundId | UUID | Round | FK, NOT NULL |
| jassTableId | UUID | JassTable | FK (optional) | NOT NULL
| status | Enum | Pairing status | NOT NULL, DEFAULT 'PENDING' |
| createdAt | DateTime | Creation timestamp | NOT NULL |
| updatedAt | DateTime | Update timestamp | NOT NULL |

**Enums**:
- `status`: `PENDING`, `COMPLETED`

**Constraints**: UNIQUE(roundId, jassTableId)

**Business rule**: Exactly 4 participants per pairing (2 per team)

---

### 9. PairingParticipant
**Description**: Participants for a pairing (4 players: 2 vs 2)

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| pairingId | UUID | Game | FK, NOT NULL |
| participantId | UUID | TournamentParticipant | FK, NOT NULL |
| team | Enum | Team (A or B) | NOT NULL |
| enteredBy | UUID | Entered by | FK (optional) |
| createdAt | DateTime | Creation timestamp | NOT NULL |

**Enums**:
- `team`: `TEAM_A`, `TEAM_B`

**Constraints**: 
- UNIQUE(pairingId, participantId)

---

### 10. Game

**Description**: A single game (2 vs 2) within a round

| Field | Type | Description | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primary key | PK, NOT NULL |
| pairingId | UUID | Pairing | FK, NOT NULL |
| gameNumber | Integer | Game number within the round | NOT NULL |
| status | Enum | Game status | NOT NULL, DEFAULT 'PENDING' |
| teamAPoints | Integer | Team A points | |
| teamBPoints | Integer | Team B points | |
| teamAMatchBonus | Boolean | Team A has match bonus | DEFAULT false |
| teamBMatchBonus | Boolean | Team B has match bonus | DEFAULT false |
| enteredBy | UUID | Entered by (User) | FK |
| enteredAt | DateTime | Entry timestamp | |
| createdAt | DateTime | Creation timestamp | NOT NULL |
| updatedAt | DateTime | Update timestamp | NOT NULL |

**Enums**:
- `status`: `PENDING`, `COMPLETED`

**Constraints**: UNIQUE(pairingId, gameNumber)

**Business rules**:
- `teamAPoints + teamBPoints = 157` (without match bonus)
- Match bonus: +100 points when a team takes all tricks
- Only one team can have the match bonus

---

## Business Rules

### Tournament Rules
1. **Tournament Visibility**: Users can see tournaments if they are either the organizer of the tournament or registered as a participant in that tournament.
2. **SYSADMIN Access**: System administrators can view and manage all tournaments of all organizers
3. **Tournament Code / QR Code**: Each tournament has a unique link/code which participants can use to join the tournament
4. **Single-Day Tournaments**: All tournaments last one day

### Config Template Rules
1. **Reusability**: Templates can be used for multiple tournaments
2. **Copy on Creation**: Tournament configuration is copied from the template when creating a tournament
3. **Independence**: Changes to a template only affect newly created tournaments
4. **Template Reference**: `TournamentConfig` keeps a reference to the original template
5. **Default template per organizer (V1)**: In the first version, each organizer has exactly one default configuration template.
Future versions may support multiple named templates per organizer.

### Round Rules
1. **Number of Rounds**: Configurable, default is 5
2. **Games per Round**: Configurable, default is 8
3. **Round Numbers**: Sequential, 1-based

### Game Rules
1. **Participants**: Exactly 4 players per game (2 teams of 2 players)
2. **Total Points**: 157 points per game
3. **Match Bonus**: +100 points if one team scores all 157 points (configurable)
4. **Automatic Calculation**: When Team A enters points, Team B points are calculated automatically (`157 - Team A`)

### Pairing Rules
1. **Default Case**: Pairings change every round (players compete individually)
2. **Alternative**: Fixed teams throughout the entire tournament (configurable)
3. **Pairing Entry**:
   - Organizer: Manual entry (or automatic random draw)
   - Players: Can enter their assigned partner themselves
4. **Tracking**: `enteredBy` in `PairingParticipant` indicates who entered the pairing

### Visibility Rules
The configured score visibility mode determines who can see scores:

1. `ALWAYS_VISIBLE_FOR_EVERYONE`: Players can always see scores.
2. `HIDDEN_DURING_ACTIVE_TOURNAMENT`: Scores are hidden from players while the tournament is active and visible afterwards.
3. `ORGANIZER_ONLY`: Only the organizer can see scores.

### Participant Rules
1. **Registered Users Only**: All participants must have a user account
2. **Excel Import**: Creates new user accounts or links existing ones
3. **Email as Identifier**: Matching is performed via email address

### JassTable Rules
1. **Organizer Ownership**: JassTables belong to the organizer, not to a specific tournament
2. **Reusability**: JassTables can be reused for all tournaments of the organizer
3. **Flexible Naming**: JassTables can be named freely
4. **Sorting**: JassTables have a `displayOrder` for consistent display
5. **Deactivation**: JassTables can be deactivated (`isActive = false`)
6. **Deletion**: A table can only be deleted if no games/pairings are assigned to it

### Player Merge Rules
1. Organizers can merge imported player accounts with manually created or self-registered user accounts.
2. The target user remains active; the source user is marked as merged.
3. Merged users must not be usable for authentication or new tournament registrations.
4. Existing tournament participations remain assigned to the merged user account.

## Indices (Performance optimization)

```sql
-- Tournament
CREATE INDEX idx_tournament_organizer ON Tournament(organizerId);
CREATE INDEX idx_tournament_status ON Tournament(status);
CREATE INDEX idx_tournament_date ON Tournament(date);

-- TournamentConfigTemplate
CREATE INDEX idx_config_template_organizer ON TournamentConfigTemplate(organizerId);

-- TournamentParticipant
CREATE INDEX idx_participant_tournament ON TournamentParticipant(tournamentId);
CREATE INDEX idx_participant_user ON TournamentParticipant(userId);

-- Round
CREATE INDEX idx_round_tournament ON Round(tournamentId);
CREATE INDEX idx_round_tournament_status ON Round(tournamentId, status);

-- JassTable
CREATE INDEX idx_table_organizer ON JassTable(organizerId);
CREATE INDEX idx_table_active ON JassTable(organizerId, isActive);

-- Pairing
CREATE INDEX idx_pairing_round ON Pairing(roundId);
CREATE INDEX idx_pairing_round_status ON Pairing(roundId, status);

-- PairingParticipant
CREATE INDEX idx_pairing_participant_pairing ON PairingParticipant(pairingId);
CREATE INDEX idx_pairing_participant_participant ON PairingParticipant(participantId);
CREATE INDEX idx_pairing_participant_team ON PairingParticipant(pairingId, team);

-- Game
CREATE INDEX idx_game_pairing ON Game(pairingId);
CREATE INDEX idx_game_pairing_status ON Game(pairingId, status);

```

## Data Migration & Import

### Excel Import (for Organizers)
- **Primary purpose**: Import historical tournament data
- Import of complete tournaments including:
  - Tournament information (name, date, location)
  - Participant data → create or link user accounts
  - Rounds and games
  - Game results and pairings
- Matching existing players via email address
- Automatic creation of all relevant entities
- Validation of imported data

