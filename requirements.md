# Jass Tournament Manager - Anforderungen & Kontext

## Übersicht
Web-Applikation für Jass-Turnier-Organisatoren zur Verwaltung von Schieber-Turnieren.

## Benutzerrollen

### System-Administrator (SYSADMIN)
- Kann alle Turniere aller Organisatoren einsehen und verwalten
- Für Support-Zwecke
- Vollzugriff auf alle Daten und Funktionen
- Kann Organisatoren und Spieler verwalten

### Organisatoren
- Können sich selbst registrieren
- Erstellen und verwalten eigene Turniere
- Sehen nur ihre eigenen Turniere (logische Abgrenzung, außer SYSADMIN)
- Können Teilnehmer manuell erfassen oder via QR-Code einladen
- Können Paarungen manuell festlegen oder automatisch auslosen
- Haben erweiterte Rechte (Turnierverwaltung, Konfiguration)
- Können Auswertungen über alle ihre Turniere machen
- Können vergangene Turnierdaten via Excel importieren

### Spieler
- Können sich einloggen
- Können Punkte von Runden eintragen
- Können Punkte einsehen (falls vom Organisator freigegeben)
- Sehen alle Turniere, an denen sie teilgenommen haben
- Können Ranglisten von Turnieren eines Organisators einsehen
- Können via QR-Code an Turnieren teilnehmen

## Jass-Variante: Schieber

- **Teams**: 2 Personen pro Team (2vs2)
- **Ansagen**: Jeweils ein Team sagt Trumpf an
- **Ansagen pro Person**: Standardmäßig 2x pro Partie (konfigurierbar)

## Turnier-Struktur

### Hierarchie
```
Turnier
  └── Runden (Standard: 5 Runden, konfigurierbar)
        └── Spiele (Standard: 8 Spiele pro Runde, konfigurierbar)
              └── Paarungen (4 Spieler pro Spiel: 2vs2)
```

### Turniere
- Gehören zu einem Organisator
- Wiederkehrende Veranstaltungen möglich
- Auswertungen über alle Turniere eines Organisators
- Anzahl Runden konfigurierbar (Standard: 5)
- Anzahl Spiele pro Runde konfigurierbar (Standard: 8)
- QR-Code für Teilnahme-Link generierbar
- Match-Bonus konfigurierbar (an/aus)

### Runden
- Eine Runde besteht aus mehreren Spielen (Standard: 8)
- Neue Paarungen können pro Runde definiert werden

### Spiele
- Ein Spiel = eine Partie zwischen zwei Zweier-Paarungen (2vs2)
- Total 157 Punkte pro Spiel
- Match-Bonus: +100 Punkte wenn ein Team alle Punkte macht (konfigurierbar)
- Nur ein Team muss Punkte eintragen, das andere wird automatisch berechnet (157 - Punkte)

### Paarungen
- **Normalfall**: Spieler spielen für sich selbst, Paarungen wechseln pro Runde
- **Alternative**: Feste Teams über gesamtes Turnier (seltener)
- **Eingabe**: Manuelle Festlegung oder automatische Auslosung

### Paarungen
- **Option 1**: Feste Teams über gesamtes Turnier
- **Option 2**: Neue Paarungen pro Runde
- **Eingabe**: Manuelle Festlegung oder automatische Auslosung

### Teilnehmer
- Organisatoren erfassen Teilnehmer manuell ODER
- Spieler fügen sich via QR-Code selbst hinzu
- Gleiche Spieler können an verschiedenen Turnieren teilnehmen
- Spieler-Identifikation via E-Mail-Adresse (für Excel-Export)

## Funktionen

### Paarungsverwaltung
- Organisatoren können Paarungen manuell festlegen
- Organisatoren können Paarungen automatisch auslosen
- **Spieler können ihren zugelosten Partner selbst eintragen**

### Punkteverwaltung
- Spieler können Punkte eintragen
- Punkte können während Turnier ausgeblendet werden (Spannungsfaktor)
- Punkte-Einsicht kann vom Organisator freigegeben/gesperrt werden

### Ranglisten
- Spieler können Ranglisten von Turnieren einsehen
- Auswertungen über alle Turniere eines Organisators

### Excel-Import
- **Primärer Zweck**: Import vergangener Turnierdaten
- **Verfügbar für**: Organisatoren (für ihre eigenen Turniere)
- Import von:
  - Turnierinformationen
  - Teilnehmerdaten
  - Spielergebnissen
  - Paarungen
- Spieler-Identifikation via E-Mail-Adresse (für Matching bestehender Spieler)
- Details werden später spezifiziert
- Ermöglicht Migration historischer Daten in das System

## Datenschutz & Isolation
- Turniere verschiedener Organisatoren sind logisch abgegrenzt
- Organisator sieht nur eigene Turniere
- Spieler sieht nur Turniere, an denen er teilgenommen hat

## Konfigurierbare Parameter (pro Turnier)
- Anzahl Ansagen pro Person (Standard: 2)
- Anzahl Runden (Standard: 5)
- Anzahl Spiele pro Runde (Standard: 8)
- Match-Bonus aktiviert/deaktiviert (Standard: aktiviert, +100 Punkte)
- Paarungs-Modus (wechselnde Paarungen vs. feste Teams, Standard: wechselnd)
- Punkte-Sichtbarkeit für Spieler während Turnier
- Punkte-Einsicht für Spieler nach Turnier

## Technische Anforderungen
- QR-Code-Generierung für Turnier-Teilnahme
- Excel-Import-Funktionalität für Spielerdaten
- Automatische Auslosung von Paarungen
- Manuelle Paarungs-Eingabe
- Automatische Punkteberechnung (157 - eingetragene Punkte)
- Match-Bonus-Berechnung (+100 bei allen Punkten)
