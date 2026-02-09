# AI-System - Soul, Memory & Guidelines

## Übersicht

Das AI-System basiert auf 3 Säulen, die das Verhalten des KI-Assistenten steuern:

| Säule | Zweck | Entity |
|-------|-------|--------|
| **Soul** | Wer bin ich? — Persönlichkeit und Identität | `AiSoul` |
| **Memory** | Was weiss ich? — Persistentes Wissen | `AiMemory` |
| **Guidelines** | Wie verhalte ich mich? — Verhaltensregeln | `AiGuidelines` |

Alle drei werden beim Start jeder Konversation geladen und in den System-Prompt eingebaut.

## 1. Soul (Persönlichkeit)

Die Soul definiert die Identität des Assistenten — Werte, Grenzen, Kommunikationsstil.

### Entity: AiSoul

| Feld | Typ | Beschreibung |
|------|-----|-------------|
| Name | string | Kurzname (z.B. "Klacks Planungsassistent") |
| Content | string | Vollständiger Soul-Text |
| IsActive | bool | Nur 1 Soul gleichzeitig aktiv |
| Source | string? | Herkunft ("chat", "seed", etc.) |

### Verhalten

- Nur **eine** Soul kann gleichzeitig aktiv sein
- Beim Update wird die alte deaktiviert, eine neue erstellt und aktiviert
- Im System-Prompt als `=== IDENTITY ===` Block eingebettet
- Ohne aktive Soul: Kein Identity-Block im Prompt

### Seed-Default

Die Standard-Soul enthält:
- **Core Truths** — Grundlegende Identitätsaussagen
- **Boundaries** — Was der Assistent nicht tut
- **Vibe** — Kommunikationsstil und Tonalität
- **Continuity** — Verhalten über Konversationen hinweg

### Skills

| Skill | Typ | Berechtigung |
|-------|-----|-------------|
| `get_ai_soul` | Query | CanViewSettings |
| `update_ai_soul` | CRUD | CanEditSettings |

**Parameter für `update_ai_soul`:**
- `soul` (required) — Der komplette Soul-Text
- `name` (optional) — Kurzname, Default: "AI Soul"

## 2. Memory (Persistentes Wissen)

Memories sind Wissenseinträge, die über alle Konversationen hinweg bestehen bleiben.

### Entity: AiMemory

| Feld | Typ | Beschreibung |
|------|-----|-------------|
| Category | string | Kategorie des Eintrags |
| Key | string | Kurzer Titel/Identifier |
| Content | string | Der eigentliche Inhalt |
| Importance | int | Wichtigkeit 1-10 (Default: 5) |
| Source | string? | Herkunft ("chat", "seed", etc.) |

### Kategorien

| Kategorie | Zweck |
|-----------|-------|
| `user_preference` | Benutzer-Vorlieben |
| `system_knowledge` | System-Wissen |
| `learned_fact` | Gelernte Fakten |
| `workflow` | Arbeitsabläufe |
| `context` | Kontext-Informationen |

Konstanten: `Application\Constants\AiMemoryCategories.cs`

### Verhalten

- Alle Memories werden geladen, nach **Importance absteigend** sortiert
- **Top 20** werden in den System-Prompt aufgenommen
- Format im Prompt: `- [category] key: content`
- Sprachspezifischer Header: "Persistentes Wissen" (de), "Persistent Knowledge" (en), etc.

### Skills

| Skill | Typ | Berechtigung | Parameter |
|-------|-----|-------------|-----------|
| `add_ai_memory` | CRUD | Admin | key, content, category?, importance? |
| `get_ai_memories` | Query | Admin | category?, searchTerm? |
| `update_ai_memory` | CRUD | Admin | memoryId, key?, content?, category?, importance? |
| `delete_ai_memory` | CRUD | Admin | memoryId |

## 3. Guidelines (Richtlinien)

Guidelines definieren Verhaltensregeln — wie der Assistent mit Funktionen umgehen soll, Berechtigungen prüft, etc.

### Entity: AiGuidelines

| Feld | Typ | Beschreibung |
|------|-----|-------------|
| Name | string | Kurzname (z.B. "Default Guidelines") |
| Content | string | Vollständiger Guidelines-Text |
| IsActive | bool | Nur 1 Guidelines-Set gleichzeitig aktiv |
| Source | string? | Herkunft ("chat", "seed", etc.) |

### Verhalten

- Nur **ein** Guidelines-Set kann gleichzeitig aktiv sein
- Beim Update wird das alte deaktiviert, ein neues erstellt und aktiviert
- **Fallback**: Ohne aktive Guidelines werden Default-Regeln verwendet:
  - Be polite and professional
  - Use available functions when users ask for them
  - Give clear and precise instructions
  - Always check permissions before executing functions
  - For missing permissions: explain that the user needs to contact an administrator
- Sprachspezifischer Header: "Richtlinien" (de), "Guidelines" (en), "Directives" (fr), "Linee guida" (it)

### Skills

| Skill | Typ | Berechtigung |
|-------|-----|-------------|
| `get_ai_guidelines` | Query | CanViewSettings |
| `update_ai_guidelines` | CRUD | CanEditSettings |

**Parameter für `update_ai_guidelines`:**
- `guidelines` (required) — Der komplette Guidelines-Text
- `name` (optional) — Kurzname, Default: "AI Guidelines"

## System-Prompt Aufbau

Der System-Prompt wird von `LLMSystemPromptBuilder` zusammengebaut. Reihenfolge:

```
┌─────────────────────────────────────────┐
│ === IDENTITY ===                        │  ← Soul (wenn aktiv)
│ [Soul Content]                          │
│ ================                        │
├─────────────────────────────────────────┤
│ Du bist ein hilfreicher KI-Assistent... │  ← Basis-Instruktion
├─────────────────────────────────────────┤
│ Benutzer-Kontext:                       │  ← User-Info
│ - User ID: ...                          │
│ - Berechtigungen: ...                   │
├─────────────────────────────────────────┤
│ Verfügbare Funktionen:                  │  ← Funktionsliste
│ - function_name: description            │
├─────────────────────────────────────────┤
│ Richtlinien:                            │  ← Guidelines (oder Defaults)
│ [Guidelines Content]                    │
├─────────────────────────────────────────┤
│ Persistentes Wissen:                    │  ← Top 20 Memories
│ - [category] key: content               │
└─────────────────────────────────────────┘
```

Der Prompt wird **mehrsprachig** generiert (de, en, fr, it) basierend auf `context.Language`.

## Skills-Referenz

Alle 8 AI-Skills im Überblick:

| Skill | Typ | Berechtigung | Beschreibung |
|-------|-----|-------------|-------------|
| `get_ai_soul` | Query | CanViewSettings | Soul-Definition abrufen |
| `update_ai_soul` | CRUD | CanEditSettings | Soul-Definition aktualisieren |
| `get_ai_memories` | Query | Admin | Memory-Einträge abrufen/suchen |
| `add_ai_memory` | CRUD | Admin | Neuen Memory-Eintrag erstellen |
| `update_ai_memory` | CRUD | Admin | Memory-Eintrag aktualisieren |
| `delete_ai_memory` | CRUD | Admin | Memory-Eintrag löschen |
| `get_ai_guidelines` | Query | CanViewSettings | Guidelines abrufen |
| `update_ai_guidelines` | CRUD | CanEditSettings | Guidelines aktualisieren |

## Berechtigungen

| Rolle | Soul | Memory | Guidelines |
|-------|------|--------|-----------|
| Admin | Lesen + Schreiben | Lesen + Schreiben | Lesen + Schreiben |
| CanViewSettings | Lesen | — | Lesen |
| CanEditSettings | Lesen + Schreiben | — | Lesen + Schreiben |
| Andere | — | — | — |

Memory-Skills erfordern explizit **Admin**-Berechtigung, da sie das Verhalten des Assistenten für alle Benutzer beeinflussen.

## Datenfluss

```
User-Nachricht
    │
    ▼
ChatController
    │
    ▼
LLMService.ProcessAsync()
    │
    ├── LoadSoulAsync()         → IAiSoulRepository.GetActiveAsync()
    ├── GetAllAsync()           → IAiMemoryRepository.GetAllAsync()
    ├── LoadGuidelinesAsync()   → IAiGuidelinesRepository.GetActiveAsync()
    │
    ▼
LLMSystemPromptBuilder.BuildSystemPrompt(context, soul, memories, guidelines)
    │
    ▼
LLMProviderRequest { SystemPrompt = ... }
    │
    ▼
ILLMProvider.ProcessAsync()  → OpenAI / DeepSeek / etc.
    │
    ▼
LLMResponse → User
```

## Datenbank-Tabellen

### ai_souls

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| name | varchar | Kurzname |
| content | text | Soul-Inhalt |
| is_active | boolean | Aktiv-Flag |
| source | varchar? | Herkunft |
| create_time | timestamp | Erstellungszeitpunkt |

### ai_memories

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| category | varchar | Kategorie |
| key | varchar | Titel/Identifier |
| content | text | Inhalt |
| importance | integer | Wichtigkeit 1-10 |
| source | varchar? | Herkunft |
| create_time | timestamp | Erstellungszeitpunkt |

### ai_guidelines

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| name | varchar | Kurzname |
| content | text | Guidelines-Inhalt |
| is_active | boolean | Aktiv-Flag |
| source | varchar? | Herkunft |
| create_time | timestamp | Erstellungszeitpunkt |
