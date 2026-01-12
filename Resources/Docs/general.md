# Klacks - Allgemeine Hilfe

## Was ist Klacks?
Klacks ist ein Planungssystem für Personalverwaltung und Schichtplanung.

## MCP Server - Deine Schnittstelle
Du (als LLM-Agent) hast über diesen MCP-Server Zugriff auf:
- **Dokumentation** - Verstehe wie Klacks funktioniert (`klacks://docs/*`)
- **Tools** - Führe Aktionen aus (Mitarbeiter erstellen, suchen, etc.)

## Hauptfunktionen

### Mitarbeiterverwaltung
- Mitarbeiter anlegen und verwalten
- Gruppen und Abteilungen
- Verträge und Pensen
- Siehe: `klacks://docs/clients`

### Schichtplanung
- Schichten erstellen und zuweisen
- Tages-, Wochen-, Monatsansicht
- Vorlagen und Wiederholungen
- Siehe: `klacks://docs/shifts`

### Identity Provider
- Single Sign-On (SSO)
- LDAP/Active Directory Integration
- Automatische Mitarbeiter-Synchronisation
- Siehe: `klacks://docs/identity-providers`

### Makros
- BASIC-ähnliche Skriptsprache für Berechnungen
- Zuschläge, Stunden, Ferien berechnen
- Siehe: `klacks://docs/macros`

## Navigation

### Hauptbereiche
| Bereich | Pfad | Beschreibung |
|---------|------|-------------|
| Dashboard | `/workplace/dashboard` | Übersicht |
| Mitarbeiter | `/workplace/client` | Mitarbeiterliste |
| Planung | `/workplace/planning` | Schichtplanung |
| Einstellungen | `/workplace/settings` | Konfiguration |

### Einstellungen
| Bereich | Beschreibung |
|---------|-------------|
| Gruppen | Abteilungen verwalten |
| Vertragstypen | Vertragskonfiguration |
| Identity Provider | SSO-Konfiguration |
| Makros | Textvorlagen |
| LLM-Modelle | KI-Assistenten |

## Sprachen
Klacks unterstützt:
- Deutsch (de)
- Englisch (en)
- Französisch (fr)
- Italienisch (it)

## Berechtigungen

### Rollen
| Rolle | Beschreibung |
|-------|-------------|
| Admin | Vollzugriff |
| Manager | Planung und Mitarbeiter |
| Planer | Nur Schichtplanung |
| Viewer | Nur Lesezugriff |

### Berechtigungen prüfen
Nicht alle Funktionen sind für alle Benutzer verfügbar.
Fehlende Berechtigungen werden angezeigt.

## Hilfe erhalten
- Im System: Tab "Erklärungen" in den Einstellungen
- MCP-Ressourcen: `klacks://docs/*`
- KI-Assistent: Chat im seitlichen Panel
