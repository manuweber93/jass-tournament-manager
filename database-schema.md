# Datenmodell: Jass Tournament Manager

## Entitäten und Beziehungen

### 1. User (Benutzer)
**Beschreibung**: Authentifizierte Benutzer des Systems (Organisatoren und Spieler)

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| email | String | E-Mail-Adresse | UNIQUE, NOT NULL |
| passwordHash | String | Gehashtes Passwort | NOT NULL |
| firstName | String | Vorname | NOT NULL |
| lastName | String | Nachname | NOT NULL |
| role | Enum | Benutzerrolle | NOT NULL |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |
| updatedAt | DateTime | Aktualisierungszeitpunkt | NOT NULL |

**Rollen**: `SYSADMIN`, `ORGANIZER`, `PLAYER`

**Beziehungen**:
- Hat viele `Tournament` als Organisator (1:n)
- Hat viele `TournamentConfigTemplate` (1:n)
- Hat viele `TournamentParticipant` als Spieler (1:n)

---

### 2. Tournament (Turnier)
**Beschreibung**: Jass-Turnier eines Organisators

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| organizerId | UUID | Organisator | FK, NOT NULL |
| name | String | Turniername | NOT NULL |
| location | String | Austragungsort | |
| date | Date | Turnierdatum | NOT NULL |
| status | Enum | Turnierstatus | NOT NULL, DEFAULT 'PLANNED' |
| qrCode | String | QR-Code für Teilnahme | UNIQUE |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |
| updatedAt | DateTime | Aktualisierungszeitpunkt | NOT NULL |

**Enums**:
- `status`: `OPEN`, `COMPLETED`, `CANCELLED`

**Beziehungen**:
- `organizerId` → `User.id` (n:1)
- Hat ein `TournamentConfig` (1:1)
- Hat viele `TournamentParticipant` (1:n)
- Hat viele `Round` (1:n)

---

### 3. TournamentConfigTemplate (Turnier-Konfigurations-Vorlage)
**Beschreibung**: Wiederverwendbare Konfigurationsvorlagen für Turniere

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| organizerId | UUID | Organisator | FK, NOT NULL |
| name | String | Template-Name | NOT NULL |
| numberOfRounds | Integer | Anzahl Runden | NOT NULL, DEFAULT 5 |
| gamesPerRound | Integer | Spiele pro Runde | NOT NULL, DEFAULT 8 |
| matchBonusEnabled | Boolean | Match-Bonus aktiviert | NOT NULL, DEFAULT true |
| fixedTeams | Boolean | Feste Teams | NOT NULL, DEFAULT false |
| scoreVisibility | Enum | Punkte-Sichtbarkeit | NOT NULL, DEFAULT 'ALWAYS_VISIBLE' |
| isDefault | Boolean | Standard-Template | NOT NULL, DEFAULT false |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |
| updatedAt | DateTime | Aktualisierungszeitpunkt | NOT NULL |

**Enums**:
- `scoreVisibility`: `ALWAYS_VISIBLE`, `HIDDEN_DURING_TOURNAMENT`, `ORGANIZER_ONLY`

**Beziehungen**:
- `organizerId` → `User.id` (n:1)
- Wird kopiert zu `TournamentConfig` beim Turnier-Erstellen

**Constraints**: UNIQUE(organizerId, name)

---

### 4. TournamentConfig (Turnier-Konfiguration)
**Beschreibung**: Konkrete Konfiguration eines Turniers (Kopie vom Template)

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| tournamentId | UUID | Turnier | FK, UNIQUE, NOT NULL |
| templateId | UUID | Ursprüngliches Template | FK (optional) |
| numberOfRounds | Integer | Anzahl Runden | NOT NULL, DEFAULT 5 |
| gamesPerRound | Integer | Spiele pro Runde | NOT NULL, DEFAULT 8 |
| matchBonusEnabled | Boolean | Match-Bonus aktiviert | NOT NULL, DEFAULT true |
| fixedTeams | Boolean | Feste Teams | NOT NULL, DEFAULT false |
| scoreVisibility | Enum | Punkte-Sichtbarkeit | NOT NULL, DEFAULT 'ALWAYS_VISIBLE' |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |
| updatedAt | DateTime | Aktualisierungszeitpunkt | NOT NULL |

**Enums**:
- `scoreVisibility`: `ALWAYS_VISIBLE`, `HIDDEN_DURING_TOURNAMENT`, `ORGANIZER_ONLY`

**Beziehungen**:
- `tournamentId` → `Tournament.id` (1:1)
- `templateId` → `TournamentConfigTemplate.id` (n:1, optional)

---

### 5. TournamentParticipant (Turnier-Teilnehmer)
**Beschreibung**: Spieler, die an einem Turnier teilnehmen

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| tournamentId | UUID | Turnier | FK, NOT NULL |
| userId | UUID | Benutzer | FK, NOT NULL |
| registrationMethod | Enum | Registrierungsmethode | NOT NULL |
| registeredAt | DateTime | Anmeldezeitpunkt | NOT NULL |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |
| updatedAt | DateTime | Aktualisierungszeitpunkt | NOT NULL |

**Enums**:
- `registrationMethod`: `MANUAL`, `QR_CODE`, `EXCEL_IMPORT`

**Beziehungen**:
- `tournamentId` → `Tournament.id` (n:1)
- `userId` → `User.id` (n:1)
- Hat viele `GameParticipant` (1:n)

**Constraints**: UNIQUE(tournamentId, userId)

**Hinweis**: Email, firstName, lastName kommen von `User`-Tabelle

---

### 6. Round (Runde)
**Beschreibung**: Eine Runde innerhalb eines Turniers

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| tournamentId | UUID | Turnier | FK, NOT NULL |
| roundNumber | Integer | Rundennummer (1-basiert) | NOT NULL |
| status | Enum | Rundenstatus | NOT NULL, DEFAULT 'PENDING' |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |
| updatedAt | DateTime | Aktualisierungszeitpunkt | NOT NULL |

**Enums**:
- `status`: `PENDING`, `IN_PROGRESS`, `COMPLETED`

**Beziehungen**:
- `tournamentId` → `Tournament.id` (n:1)
- Hat viele `Game` (1:n)

**Constraints**: UNIQUE(tournamentId, roundNumber)

---

### 7. Table (Tisch)
**Beschreibung**: Vordefinierte Tische für ein Turnier

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| tournamentId | UUID | Turnier | FK, NOT NULL |
| name | String | Tisch-Bezeichnung | NOT NULL |
| displayOrder | Integer | Sortierreihenfolge | NOT NULL |
| isActive | Boolean | Tisch aktiv | NOT NULL, DEFAULT true |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |
| updatedAt | DateTime | Aktualisierungszeitpunkt | NOT NULL |

**Beziehungen**:
- `tournamentId` → `Tournament.id` (n:1)
- Hat viele `Game` (1:n)

**Constraints**: UNIQUE(tournamentId, name)

---

### 8. Game (Spiel)
**Beschreibung**: Ein einzelnes Spiel (2vs2) innerhalb einer Runde

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| roundId | UUID | Runde | FK, NOT NULL |
| gameNumber | Integer | Spielnummer in Runde | NOT NULL |
| tableId | UUID | Tisch | FK (optional) |
| status | Enum | Spielstatus | NOT NULL, DEFAULT 'PENDING' |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |
| updatedAt | DateTime | Aktualisierungszeitpunkt | NOT NULL |

**Enums**:
- `status`: `PENDING`, `IN_PROGRESS`, `COMPLETED`, `CANCELLED`

**Beziehungen**:
- `roundId` → `Round.id` (n:1)
- `tableId` → `Table.id` (n:1, optional)
- Hat viele `GameParticipant` (1:n, genau 4)
- Hat ein `GameScore` (1:1, optional)

**Constraints**: UNIQUE(roundId, gameNumber)

---

### 9. GameParticipant (Spiel-Teilnehmer)
**Beschreibung**: Paarungen für ein Spiel (4 Spieler: 2vs2)

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| gameId | UUID | Spiel | FK, NOT NULL |
| participantId | UUID | Teilnehmer | FK, NOT NULL |
| team | Enum | Team (A oder B) | NOT NULL |
| enteredBy | UUID | Eingetragen von | FK (optional) |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |

**Enums**:
- `team`: `TEAM_A`, `TEAM_B`

**Beziehungen**:
- `gameId` → `Game.id` (n:1)
- `participantId` → `TournamentParticipant.id` (n:1)
- `enteredBy` → `User.id` (n:1, optional)

**Constraints**: 
- UNIQUE(gameId, participantId)
- UNIQUE(gameId, team, participantId)

**Geschäftsregel**: Genau 4 Teilnehmer pro Spiel (2 pro Team)

---

### 10. GameScore (Spielergebnis)
**Beschreibung**: Punktestand eines Spiels

| Feld | Typ | Beschreibung | Constraints |
|------|-----|--------------|-------------|
| id | UUID | Primärschlüssel | PK, NOT NULL |
| gameId | UUID | Spiel | FK, UNIQUE, NOT NULL |
| teamAPoints | Integer | Punkte Team A | NOT NULL |
| teamBPoints | Integer | Punkte Team B | NOT NULL |
| teamAMatchBonus | Boolean | Team A hat Match-Bonus | NOT NULL, DEFAULT false |
| teamBMatchBonus | Boolean | Team B hat Match-Bonus | NOT NULL, DEFAULT false |
| enteredBy | UUID | Erfasst von (User) | FK, NOT NULL |
| enteredAt | DateTime | Erfassungszeitpunkt | NOT NULL |
| createdAt | DateTime | Erstellungszeitpunkt | NOT NULL |
| updatedAt | DateTime | Aktualisierungszeitpunkt | NOT NULL |

**Beziehungen**:
- `gameId` → `Game.id` (1:1)
- `enteredBy` → `User.id` (n:1)

**Geschäftsregeln**:
- `teamAPoints + teamBPoints = 157` (ohne Match-Bonus)
- Match-Bonus: +100 Punkte wenn ein Team 157 Punkte hat
- Nur ein Team kann Match-Bonus haben

---

## Entity Relationship Diagram (ERD)

```mermaid
erDiagram
    User ||--o{ Tournament : "organizes"
    User ||--o{ TournamentConfigTemplate : "creates"
    User ||--o{ TournamentParticipant : "registers as"
    User ||--o{ GameScore : "enters"
    User ||--o{ GameParticipant : "enters pairing"
    
    Tournament ||--|| TournamentConfig : "has"
    Tournament ||--o{ TournamentParticipant : "has"
    Tournament ||--o{ Round : "has"
    Tournament ||--o{ Table : "has"
    
    TournamentConfigTemplate ||--o{ TournamentConfig : "copied to"
    
    Round ||--o{ Game : "contains"
    
    Table ||--o{ Game : "assigned to"
    
    Game ||--o{ GameParticipant : "has"
    Game ||--o| GameScore : "has result"
    
    TournamentParticipant ||--o{ GameParticipant : "plays in"
    
    User {
        uuid id PK
        string email UK
        string passwordHash
        string firstName
        string lastName
        enum role
        datetime createdAt
        datetime updatedAt
    }
    
    Tournament {
        uuid id PK
        uuid organizerId FK
        string name
        string location
        date date
        enum status
        string qrCode UK
        datetime createdAt
        datetime updatedAt
    }
    
    TournamentConfigTemplate {
        uuid id PK
        uuid organizerId FK
        string name
        int numberOfRounds
        int gamesPerRound
        bool matchBonusEnabled
        bool fixedTeams
        enum scoreVisibility
        bool isDefault
        datetime createdAt
        datetime updatedAt
    }
    
    TournamentConfig {
        uuid id PK
        uuid tournamentId FK
        uuid templateId FK
        int numberOfRounds
        int gamesPerRound
        bool matchBonusEnabled
        bool fixedTeams
        enum scoreVisibility
        datetime createdAt
        datetime updatedAt
    }
    
    TournamentParticipant {
        uuid id PK
        uuid tournamentId FK
        uuid userId FK
        enum registrationMethod
        datetime registeredAt
        datetime createdAt
        datetime updatedAt
    }
    
    Round {
        uuid id PK
        uuid tournamentId FK
        int roundNumber
        enum status
        datetime createdAt
        datetime updatedAt
    }
    
    Table {
        uuid id PK
        uuid tournamentId FK
        string name
        int displayOrder
        bool isActive
        datetime createdAt
        datetime updatedAt
    }
    
    Game {
        uuid id PK
        uuid roundId FK
        int gameNumber
        uuid tableId FK
        enum status
        datetime createdAt
        datetime updatedAt
    }
    
    GameParticipant {
        uuid id PK
        uuid gameId FK
        uuid participantId FK
        uuid enteredBy FK
        enum team
        datetime createdAt
    }
    
    GameScore {
        uuid id PK
        uuid gameId FK
        int teamAPoints
        int teamBPoints
        bool teamAMatchBonus
        bool teamBMatchBonus
        uuid enteredBy FK
        datetime enteredAt
        datetime createdAt
        datetime updatedAt
    }
```

## Geschäftsregeln

### Turnier-Regeln
1. **Organisator-Isolation**: Organisatoren sehen nur ihre eigenen Turniere
2. **SYSADMIN-Zugriff**: System-Administratoren können alle Turniere aller Organisatoren einsehen und verwalten
3. **Spieler-Sicht**: Spieler sehen nur Turniere, an denen sie teilnehmen
4. **QR-Code**: Jedes Turnier hat einen eindeutigen QR-Code für Teilnahme
5. **Ein-Tages-Turniere**: Alle Turniere dauern einen Tag
6. **Turnier-Status**: Einfache Status (OPEN, COMPLETED, CANCELLED) für einfache Bedienung

### Config-Template-Regeln
1. **Wiederverwendbarkeit**: Templates können für mehrere Turniere verwendet werden
2. **Kopie beim Erstellen**: Beim Turnier-Erstellen wird Config vom Template kopiert
3. **Unabhängigkeit**: Änderungen am Template betreffen nur neue Turniere
4. **Default-Template**: Organisator kann ein Standard-Template markieren
5. **Template-Referenz**: TournamentConfig behält Referenz zum ursprünglichen Template

### Runden-Regeln
1. **Anzahl Runden**: Konfigurierbar, Standard 5
2. **Spiele pro Runde**: Konfigurierbar, Standard 8
3. **Rundennummern**: Fortlaufend, 1-basiert

### Spiel-Regeln
1. **Teilnehmer**: Genau 4 Spieler pro Spiel (2 Teams à 2 Spieler)
2. **Punkte-Total**: 157 Punkte pro Spiel
3. **Match-Bonus**: +100 Punkte wenn ein Team alle 157 Punkte macht (konfigurierbar)
4. **Automatische Berechnung**: Wenn Team A Punkte einträgt, werden Team B Punkte automatisch berechnet (157 - Team A)
5. **Tischzuweisung**: Beim Auslosen werden Tische automatisch vergeben

### Paarungs-Regeln
1. **Normalfall**: Wechselnde Paarungen pro Runde (Spieler spielen für sich)
2. **Alternative**: Feste Teams über gesamtes Turnier (konfigurierbar)
3. **Paarungs-Eingabe**:
   - Organisator: Manuelle Eingabe oder automatische Zufallsauslosung
   - Spieler: Können ihren zugelosten Partner selbst eintragen
4. **Tracking**: `enteredBy` in `GameParticipant` zeigt, wer die Paarung erfasst hat

### Sichtbarkeits-Regeln
1. **ALWAYS_VISIBLE**: Spieler sehen immer alle Punkte
2. **HIDDEN_DURING_TOURNAMENT**: Punkte während Turnier ausgeblendet, danach sichtbar
3. **ORGANIZER_ONLY**: Nur Organisator sieht Punkte

### Teilnehmer-Regeln
1. **Nur registrierte Benutzer**: Alle Teilnehmer müssen User-Account haben
2. **Excel-Import**: Erstellt neue User-Accounts oder verknüpft bestehende
3. **Email als Identifier**: Matching via Email-Adresse

### Tisch-Regeln
1. **Vordefinierte Tische**: Organisator definiert verfügbare Tische pro Turnier
2. **Flexible Benennung**: Tische können beliebig benannt werden (z.B. "Tisch 1", "VIP", "Stammtisch")
3. **Automatische Zuweisung**: Beim Auslosen werden Spiele automatisch auf Tische verteilt
4. **Sortierung**: Tische haben displayOrder für konsistente Anzeige
5. **Deaktivierung**: Tische können deaktiviert werden (isActive = false)
6. **Löschung**: Tisch kann nur gelöscht werden, wenn keine Spiele zugewiesen

## Indizes (Performance-Optimierung)

```sql
-- Häufige Abfragen
CREATE INDEX idx_tournament_organizer ON Tournament(organizerId);
CREATE INDEX idx_tournament_status ON Tournament(status);
CREATE INDEX idx_tournament_date ON Tournament(date);

CREATE INDEX idx_config_template_organizer ON TournamentConfigTemplate(organizerId);
CREATE INDEX idx_config_template_default ON TournamentConfigTemplate(organizerId, isDefault);

CREATE INDEX idx_participant_tournament ON TournamentParticipant(tournamentId);
CREATE INDEX idx_participant_user ON TournamentParticipant(userId);

CREATE INDEX idx_round_tournament ON Round(tournamentId);
CREATE INDEX idx_round_status ON Round(status);

CREATE INDEX idx_table_tournament ON Table(tournamentId);
CREATE INDEX idx_table_active ON Table(tournamentId, isActive);

CREATE INDEX idx_game_round ON Game(roundId);
CREATE INDEX idx_game_status ON Game(status);

CREATE INDEX idx_game_participant_game ON GameParticipant(gameId);
CREATE INDEX idx_game_participant_participant ON GameParticipant(participantId);

CREATE INDEX idx_game_score_game ON GameScore(gameId);
```

## Datenmigration & Import

### Excel-Import (für Organisatoren)
- **Primärer Zweck**: Import vergangener Turnierdaten
- Import von kompletten Turnieren inkl.:
  - Turnierinformationen (Name, Datum, Ort)
  - Teilnehmerdaten → User-Accounts erstellen/verknüpfen
  - Runden und Spiele
  - Spielergebnisse und Paarungen
- Matching bestehender Spieler via E-Mail-Adresse
- Automatische Erstellung aller relevanten Entitäten
- Validierung der importierten Daten

## Änderungen gegenüber vorheriger Version

### Entfernt
- ❌ `Tournament.description`
- ❌ `Tournament.startDate/endDate` → nur `date`
- ❌ `TournamentConfig.announcementsPerPlayer`
- ❌ `TournamentConfig.hideScoresDuringTournament`
- ❌ `TournamentConfig.allowPlayerScoreView`
- ❌ `TournamentParticipant.email/firstName/lastName`
- ❌ `Round.startTime/endTime`
- ❌ `Game.startTime/endTime`
- ❌ `GameParticipant.position`

### Hinzugefügt
- ✅ `TournamentConfigTemplate` (neue Tabelle)
- ✅ `TournamentConfig.templateId`
- ✅ `TournamentConfig.scoreVisibility` (ENUM)
- ✅ `TournamentConfigTemplate.isDefault`

### Geändert
- 🔄 `TournamentParticipant.userId` → NOT NULL (immer Account erforderlich)
- 🔄 `Tournament` → nur ein `date` statt `startDate/endDate`
- 🔄 Score Visibility → vereinfacht zu einem ENUM

## Nächste Schritte

1. ✅ Datenmodell überarbeitet
2. ⏭️ Prisma Schema aktualisieren
3. ⏭️ Migrations erstellen
4. ⏭️ Seed-Daten für Entwicklung
