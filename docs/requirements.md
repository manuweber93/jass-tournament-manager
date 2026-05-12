# Jass Tournament Manager - Requirements & Context

## Overview
Web application for Jass tournament organizers to manage Schieber tournaments.

## Types of Users

### System Administrator (SYSADMIN)
- Can view and manage all tournaments from all organizers
- For support purposes
- Full access to all data and functions
- Can manage organizers and players

### Organizers
- Can register themselves
- Create and manage their own tournaments
- See only their own tournaments (logical separation, except SYSADMIN)
- Can register participants manually or invite via QR code
- Can set pairings manually or run automatic draws
- Have extended permissions (tournament management, configuration)
- Can run reports across all their tournaments
- Can import past tournament data via Excel

### Players
- Can log in
- Can enter scores for rounds
- Can view scores (if allowed by the organizer)
- See all tournaments they participated in
- Can view leaderboards for an organizer's tournaments
- Can join tournaments via code
- Can select an existing user during first login/registration, so that they can reuse a user that was created during an import

## Jass Variant: Schieber

- Teams: 2 players per team (2 vs 2)
- Declarations: a team declares trump
- Declarations per player: typically 2 per game (configurable)

## Tournament Structure

### Hierarchy

Turnament
  └── Rounds (default: 5 rounds, configurable)
        └── Pairings (4 players per pairing: 2 vs 2)
                └── Games (default: 8 games per pairing/round, configurable)
              

### Tournaments
- Belong to an organizer
- Can be recurring events
- Reports across all tournaments of an organizer
- Number of rounds configurable (default: 5)
- Number of games per round configurable (default: 8)
- Code for participation can be generated (maybe QR code to scan)
- Match bonus configurable (on/off)

### Rounds
- A round consists of multiple games (default: 8)
- New pairings can be defined per round

### Games
- A game = one match between two pairs (2 vs 2)
- Total 157 points per game
- Match bonus: +100 points when a team takes all points (configurable)
- Only one team needs to enter points; the other is calculated as (157 - entered points)

### Pairings
- Normal mode: players play individually and pairings change per round
- Alternative: fixed teams for the whole tournament
- Input: manual assignment or automatic draw

### Participants
- Organizers can register participants manually OR
- Players can add themselves via code
- Same players can participate in different tournaments
- Player identification via email address (for Excel export/matching)

## Features

### Pairing Management
- Organizers can assign pairings manually
- Organizers can run automatic pairing draws
- Players can confirm or enter their assigned partner themselves

### Score Management
- Players can enter scores
- Scores can be hidden during the tournament (for suspense)
- Score visibility can be toggled by the organizer

### Leaderboards
- Players can view leaderboards for tournaments
- Reports across all tournaments of an organizer

### Excel Import
- Primary purpose: import historical tournament data
- Available for: Organizers (for their own tournaments)
- Import includes:
  - Tournament metadata
  - Participant data
  - Game results
  - Pairings
- Player matching via email address
- Details to be specified later
- Enables migration of historical data into the system

## Privacy & Isolation
- Tournaments of different organizers are logically separated
- Organizer sees only their own tournaments
- Player sees only tournaments they participated in

## Configurable Parameters (per tournament)
- Number of rounds (default: 5)
- Number of games per round (default: 8)
- Match bonus enabled/disabled (default: enabled, +100 points)
- Pairing mode (rotating pairings vs fixed teams, default: rotating)
- Score visibility to players during the tournament
- Score visibility to players after the tournament

## Technical Requirements
- Code generation for tournament participation
- Excel import functionality for historical player and result data
- Manual pairing entry
- Automatic score calculation (157 - entered points)
- Match bonus calculation (+100 when applicable)
- (Automatic pairing draw for games)
