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

### Auto-Planung (Wizard/Autofill)
- Automatische Schichtplan-Erstellung per evolutionärem Algorithmus
- Berücksichtigt Vertragsstunden, Verfügbarkeit, harte und weiche Regeln
- Siehe: `klacks://docs/autofill`

### Export & Periodenabschluss
- Perioden sperren (Periodenabschluss) vor dem Export
- Bestellungs-Export in mehreren Formaten (CSV/JSON/XML/DATEV/BMD) plus länderspezifische Lohn-Exportformate
- Siehe: `klacks://docs/exports`

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
Klacks unterstützt 25 Sprachen: die vier Basis-Sprachen Deutsch (de), Englisch (en), Französisch (fr) und Italienisch (it) sowie 21 zusätzliche Sprachen als Language-Plugins (u.a. Arabisch, Chinesisch vereinfacht/traditionell, Japanisch, Koreanisch, Niederländisch, Polnisch, Portugiesisch, Rumänisch, Schwedisch, Spanisch, Thailändisch, Tschechisch, Vietnamesisch). Jedes Language-Plugin liefert Geodaten (Länder/Kantone/Kalenderregeln), UI-Übersetzungen und Skill-Synonyme.

## Berechtigungen

### Rollen
| Rolle | Beschreibung |
|-------|-------------|
| Admin | Vollzugriff, inkl. AI-System (Soul/Memory/Guidelines), Settings, Sensitive Skills |
| Authorised | Standard-Berechtigung für angemeldete Benutzer (granulare Permissions je Feature) |
| User | Basis-Zugriff |

Feingranulare Rechte (z.B. CanViewSettings, CanEditSettings) werden pro Rolle über `Permissions.GetPermissionsForRole()` aufgelöst, nicht über zusätzliche Rollen.

### Berechtigungen prüfen
Nicht alle Funktionen sind für alle Benutzer verfügbar.
Fehlende Berechtigungen werden angezeigt.

## Hilfe erhalten
- Im System: Tab "Erklärungen" in den Einstellungen
- MCP-Ressourcen: `klacks://docs/*`
- KI-Assistent: Chat im seitlichen Panel
