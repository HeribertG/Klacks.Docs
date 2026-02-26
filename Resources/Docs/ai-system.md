# AI-System - Soul, Memory, Guidelines & Agent Framework

## Übersicht

Das AI-System basiert auf zwei Schichten:

1. **Legacy-Entities** (`ai_souls`, `ai_memories`, `ai_guidelines`) — einfache Key/Value-Tabellen, per Migration geseeded
2. **Agent Framework** (`agents`, `agent_soul_sections`, `agent_memories`, `agent_sessions`, `agent_skills`, ...) — voll ausgebautes, multi-agent-fähiges System mit pgvector, Session-Management und Skill-Ausführung

Die Legacy-Entities dienen als **Datenquelle und Fallback**. Das Agent Framework ist die **Runtime**, die den System-Prompt zusammenbaut und Konversationen verwaltet.

### Die 3 Säulen

| Säule | Zweck | Legacy Entity | Agent Entity |
|-------|-------|---------------|-------------|
| **Soul** | Wer bin ich? — Persönlichkeit und Identität | `AiSoul` | `AgentSoulSection` |
| **Memory** | Was weiss ich? — Persistentes Wissen | `AiMemory` | `AgentMemory` |
| **Guidelines** | Wie verhalte ich mich? — Verhaltensregeln | `AiGuidelines` | (direkt aus DB geladen) |

---

## 1. Soul (Persönlichkeit)

Die Soul definiert die Identität des Assistenten — Werte, Grenzen, Kommunikationsstil.

### Legacy Entity: AiSoul

| Feld | Typ | Beschreibung |
|------|-----|-------------|
| Name | string | Kurzname (z.B. "Klacks Assistant") |
| Content | string | Vollständiger Soul-Text (alle Sektionen als Markdown) |
| IsActive | bool | Nur 1 Soul gleichzeitig aktiv |
| Source | string? | Herkunft ("chat", "seed", etc.) |

### Agent Entity: AgentSoulSection

Die Soul wird pro Agent in **typisierte Sektionen** aufgeteilt:

| Feld | Typ | Beschreibung |
|------|-----|-------------|
| AgentId | Guid | Zuordnung zum Agent |
| SectionType | string | Sektionstyp (identity, personality, boundaries, ...) |
| Content | string | Inhalt dieser Sektion |
| SortOrder | int | Reihenfolge im Prompt |
| IsActive | bool | Aktiv-Flag |
| Version | int | Automatisch hochgezählt bei Änderungen |
| Source | string? | Herkunft ("seed", "chat", etc.) |

### Sektionstypen

| SectionType | Inhalt | SortOrder |
|-------------|--------|-----------|
| `identity` | Wer ist der Agent? Rolle und Zweck | 0 |
| `personality` | Core Truths — Grundwerte und Arbeitsweise | 1 |
| `boundaries` | Was der Agent nicht tun darf | 2 |
| `communication_style` | Vibe — Tonalität und Sprachstil | 3 |
| `values` | Continuity — Verhalten über Sessions hinweg | 4 |
| `tone` | Formalität und Länge (optional) | 5 |
| `domain_expertise` | Fachgebiet und Branchenwissen (optional) | 6 |
| `error_handling` | Umgang mit Fehlern und Unwissen (optional) | 7 |

### Seed-Verhalten

Beim App-Start prüft `AgentSoulSectionSeedService`:
1. Default Agent laden via `IAgentRepository.GetDefaultAgentAsync()`
2. Existieren bereits Sektionen? → Skip
3. Wenn leer → 5 Default-Sektionen aus dem AiSoul-Content seeden

### History-Tracking

Jede Änderung an einer Soul-Sektion wird in `agent_soul_histories` protokolliert:

| Feld | Beschreibung |
|------|-------------|
| ContentBefore | Alter Inhalt |
| ContentAfter | Neuer Inhalt |
| Version | Versionsnummer |
| ChangeType | Create / Update / Deactivate |
| ChangedBy | Wer hat geändert |

### Skills

| Skill | Typ | Berechtigung |
|-------|-----|-------------|
| `get_ai_soul` | Query | CanViewSettings |
| `update_ai_soul` | CRUD | CanEditSettings |

---

## 2. Memory (Persistentes Wissen)

Memories sind Wissenseinträge, die über alle Konversationen hinweg bestehen bleiben.

### Legacy Entity: AiMemory

| Feld | Typ | Beschreibung |
|------|-----|-------------|
| Category | string | Kategorie des Eintrags |
| Key | string | Kurzer Titel/Identifier |
| Content | string | Der eigentliche Inhalt |
| Importance | int | Wichtigkeit 1-10 (Default: 5) |
| Source | string? | Herkunft ("chat", "seed", etc.) |

### Agent Entity: AgentMemory

Erweitert die Legacy-Entity um semantische Suche, Pinning und Ablauf:

| Feld | Typ | Beschreibung |
|------|-----|-------------|
| AgentId | Guid | Zuordnung zum Agent |
| Category | string | Kategorie (fact, preference, decision, ...) |
| Key | string | Kurzer Identifier |
| Content | string | Inhalt |
| Importance | int | Wichtigkeit 1-10 |
| Embedding | float[]? | pgvector-Embedding für semantische Suche |
| IsPinned | bool | Gepinnte Memories: IMMER im Context |
| ExpiresAt | DateTime? | NULL = permanent, Set = temporär |
| SupersedesId | Guid? | Referenz auf ersetzte Memory |
| AccessCount | int | Zugriffszähler (für Relevanz-Scoring) |
| LastAccessedAt | DateTime? | Letzter Zugriff |
| Source | string | Herkunft (conversation, user_explicit, compaction_flush, ...) |
| SourceRef | string? | Referenz auf Session/Message |
| Metadata | JsonDocument | Flexible Key-Value-Daten |

### Kategorien

| Kategorie | Zweck |
|-----------|-------|
| `fact` | Verifiziertes Wissen |
| `preference` | Benutzer-Vorlieben |
| `decision` | Getroffene Entscheidungen |
| `user_info` | Persönliche Info über den User |
| `project_context` | Projektspezifischer Kontext |
| `learned_behavior` | Gelerntes Verhalten aus Feedback |
| `correction` | Korrektur einer früheren Annahme |
| `temporal` | Zeitgebundene Info (Meeting, Deadline) |

Konstanten: `Domain/Constants/MemoryCategories.cs`

### Hybrid Search

Die Memory-Suche kombiniert mehrere Signale:

| Signal | Gewicht | Beschreibung |
|--------|---------|-------------|
| Vector Similarity | 50% | Semantische Nähe via pgvector (cosine distance) |
| Full-Text Search | 20% | Keyword-Match via tsvector (German) |
| Importance | 15% | Manuelles Scoring (1-10) |
| Recency | 10% | Exponential Decay (neuere = relevanter) |
| Access Frequency | 5% | Häufig abgerufene Memories höher gerankt |

**Ablauf:** Pre-Filter (Top 50 via Vector) → Scoring → Top N zurückgeben.

### Background Services

| Service | Intervall | Aufgabe |
|---------|-----------|---------|
| `EmbeddingBackgroundService` | 60 Sekunden | Generiert Embeddings für neue Memories |
| `MemoryCleanupBackgroundService` | 6 Stunden | Löscht abgelaufene Memories, archiviert inaktive Sessions |

### Skills

| Skill | Typ | Berechtigung | Parameter |
|-------|-----|-------------|-----------|
| `add_ai_memory` | CRUD | Admin | key, content, category?, importance? |
| `get_ai_memories` | Query | Admin | category?, searchTerm? |
| `update_ai_memory` | CRUD | Admin | memoryId, key?, content?, category?, importance? |
| `delete_ai_memory` | CRUD | Admin | memoryId |

---

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
- Wird direkt in den System-Prompt injiziert als `=== GUIDELINES ===` Block
- Template-Variablen ({{LANGUAGE}}, {{LANGUAGE_CODE}}) werden aufgelöst
- Ohne aktive Guidelines: Kein Guidelines-Block im Prompt

### Skills

| Skill | Typ | Berechtigung |
|-------|-----|-------------|
| `get_ai_guidelines` | Query | CanViewSettings |
| `update_ai_guidelines` | CRUD | CanEditSettings |

---

## 4. Agent Framework — Architektur

### Tabellen-Übersicht

```
┌─────────────────────────────────────────────────────────────┐
│                      agents                                  │
│  (Kern-Definition: Name, Config, is_default)                 │
│                                                              │
│  ┌──────────────────┐  ┌──────────────────┐                 │
│  │ agent_soul_      │  │ agent_memories   │                 │
│  │ sections         │  │ + agent_memory_  │                 │
│  │ + agent_soul_    │  │   tags           │                 │
│  │   histories      │  │ + pgvector       │                 │
│  └──────────────────┘  └──────────────────┘                 │
│                                                              │
│  ┌──────────────────┐  ┌──────────────────┐                 │
│  │ agent_skills     │  │ agent_sessions   │                 │
│  │ + agent_skill_   │  │ + agent_session_ │                 │
│  │   executions     │  │   messages       │                 │
│  └──────────────────┘  └──────────────────┘                 │
│                                                              │
│  ┌──────────────────┐  ┌──────────────────┐                 │
│  │ agent_links      │  │ global_agent_    │                 │
│  │ (Multi-Agent)    │  │ rules            │                 │
│  │ ⚠️ noch nicht    │  │ + global_agent_  │                 │
│  │   implementiert  │  │   rule_histories │                 │
│  └──────────────────┘  └──────────────────┘                 │
└─────────────────────────────────────────────────────────────┘
```

---

### agents

Kern-Definition eines Agent.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| name | varchar | Interner Name (z.B. "klacks-default") |
| display_name | varchar | Anzeigename |
| description | text? | Beschreibung |
| is_active | boolean | Aktiv-Flag |
| is_default | boolean | Default-Agent für das System |
| template_id | uuid? | FK auf agent_templates (optional) |

**Repository:** `IAgentRepository` → `AgentRepository`
**Controller:** `AgentsController` (GET, POST, PUT)
**Seed:** `AgentSkillSeedService` erstellt den Default-Agent (`klacks-default`) beim Start.

---

### agent_soul_sections

Persönlichkeits-Sektionen pro Agent. Werden als `=== IDENTITY ===` in den System-Prompt injiziert.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| agent_id | uuid | FK → agents |
| section_type | varchar | Typ (identity, personality, boundaries, ...) |
| content | text | Sektionsinhalt |
| sort_order | int | Reihenfolge im Prompt |
| is_active | boolean | Aktiv-Flag |
| version | int | Automatisch hochgezählt |
| source | varchar? | Herkunft (seed, chat) |

**Repository:** `IAgentSoulRepository` → `AgentSoulRepository`
**Seed:** `AgentSoulSectionSeedService` seedet 5 Default-Sektionen beim Start.
**History:** Jede Änderung → `agent_soul_histories`

---

### agent_soul_histories

Audit-Trail für Soul-Änderungen.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| agent_id | uuid | FK → agents |
| soul_section_id | uuid | FK → agent_soul_sections |
| section_type | varchar | Welche Sektion geändert wurde |
| content_before | text? | Alter Inhalt (NULL bei Create) |
| content_after | text | Neuer Inhalt |
| version | int | Versionsnummer |
| change_type | varchar | Create / Update / Deactivate |
| changed_by | varchar? | Wer hat geändert |
| change_reason | text? | Begründung |

---

### agent_memories

Langzeitgedächtnis mit pgvector-Embeddings und Hybrid-Suche.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| agent_id | uuid | FK → agents |
| category | varchar | Kategorie (fact, preference, decision, ...) |
| key | varchar | Kurzer Identifier |
| content | text | Inhalt |
| importance | int | Wichtigkeit 1-10 |
| embedding | vector(1536)? | pgvector-Embedding (NULL wenn ausstehend) |
| is_pinned | boolean | Gepinnt = IMMER im Context |
| expires_at | timestamp? | NULL = permanent |
| supersedes_id | uuid? | FK → agent_memories (Vorgänger) |
| access_count | int | Zugriffszähler |
| last_accessed_at | timestamp? | Letzter Zugriff |
| source | varchar | Herkunft |
| source_ref | varchar? | Referenz auf Session/Message |
| metadata | jsonb | Flexible Metadaten |

**Repository:** `IAgentMemoryRepository` → `AgentMemoryRepository`
**Indizes:** HNSW (vector_cosine_ops), GIN (tsvector), GIN (trgm)

---

### agent_memory_tags

Flexible Verschlagwortung für Memories.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| memory_id | uuid | PK, FK → agent_memories |
| tag | varchar | PK, Tag-Text |

---

### agent_sessions

Konversations-Sessions pro Agent+User.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| agent_id | uuid | FK → agents |
| session_id | varchar | Externe Session-ID |
| user_id | varchar? | User-Identifier |
| title | varchar? | Session-Titel |
| summary | text? | Zusammenfassung (nach Compaction) |
| status | varchar | Active / Archived |
| message_count | int | Anzahl Messages |
| token_count_est | int | Geschätzte Token-Anzahl |
| compaction_count | int | Wie oft kompaktiert |
| active_categories | text[]? | Aktive Skill-Kategorien für Session-Context |
| channel | varchar | Kanal (web, slack, ...) |
| last_message_at | timestamp? | Letzter Message-Zeitpunkt |
| last_model_id | varchar? | Zuletzt verwendetes LLM-Modell |
| is_archived | boolean | Archiviert-Flag |

**Repository:** `IAgentSessionRepository` → `AgentSessionRepository`
**Compaction:** Wenn Token-Count zu hoch → alte Messages zusammenfassen → Summary speichern.
**Archivierung:** Sessions > 30 Tage inaktiv → automatisch archiviert via Background Service.

---

### agent_session_messages

Chat-Verlauf innerhalb einer Session.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| session_id | uuid | FK → agent_sessions |
| role | varchar | system / user / assistant / tool |
| content | text? | Nachrichteninhalt (NULL bei reinen Tool-Calls) |
| token_count | int? | Token-Schätzung |
| model_id | varchar? | Verwendetes Modell |
| function_calls | jsonb? | Tool-Call-Daten |
| is_compacted | boolean | Wurde diese Message kompaktiert? |
| compacted_into_id | uuid? | FK → agent_session_messages (Summary) |

---

### agent_skills

Registrierte Skills pro Agent.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| agent_id | uuid | FK → agents |
| name | varchar | Skill-Name (z.B. "create_employee") |
| description | text | Beschreibung für LLM |
| parameters_json | jsonb? | Parameter-Schema |
| required_permission | varchar? | Benötigte Berechtigung |
| execution_type | varchar | Backend / Frontend / UiAction |
| category | varchar | Query / Crud / Navigation / ... |
| is_enabled | boolean | Aktiv-Flag |
| sort_order | int | Reihenfolge |
| handler_type | varchar? | internal / http / mcp |
| handler_config | jsonb? | Handler-Konfiguration |
| trigger_keywords | text[]? | Keywords für automatische Aktivierung |
| allowed_channels | text[]? | Kanal-Einschränkung |
| max_calls_per_session | int? | Rate Limit pro Session |
| always_on | boolean | Immer aktiv (kein Keyword-Match nötig) |
| version | int | Versionsnummer |

**Repository:** `IAgentSkillRepository` → `AgentSkillRepository`
**Seed:** `AgentSkillSeedService` synct Skills aus `ISkillRegistry` in die DB.

---

### agent_skill_executions

Audit-Log für Skill-Ausführungen.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| agent_id | uuid | FK → agents |
| skill_id | uuid | FK → agent_skills |
| session_id | uuid? | FK → agent_sessions |
| user_id | varchar? | User-Identifier |
| tool_name | varchar | Aufgerufene Funktion |
| parameters_json | jsonb? | Bereinigte Parameter (keine Secrets) |
| success | boolean | Erfolg/Fehler |
| result_message | text? | Ergebnis-Text |
| error_message | text? | Fehlermeldung |
| duration_ms | int? | Ausführungsdauer |
| triggered_by | varchar | agent / user / cron |

**Verwendung:** Audit-Trail + Rate-Limiting (`GetSessionCallCountAsync()`).

---

### agent_links ⚠️ Zukunfts-Feature

Agent-zu-Agent Kommunikation. Schema vorhanden, **kein Repository implementiert**.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| source_agent_id | uuid | FK → agents (Sender) |
| target_agent_id | uuid | FK → agents (Empfänger) |
| link_type | varchar | can_delegate / can_read_memory / can_notify / parent_child |
| config | jsonb | Link-Konfiguration (z.B. allowed_categories) |
| is_active | boolean | Aktiv-Flag |

**Status:** Entity-Klasse und DB-Schema existieren, aber keine Business-Logik implementiert.

---

### global_agent_rules

System-weite Regeln (gelten für **alle** Agents).

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| name | varchar | Regel-Name (z.B. "ADDRESS_VALIDATION") |
| content | text | Regel-Inhalt (unterstützt {{LANGUAGE}} Templates) |
| sort_order | int | Reihenfolge |
| is_active | boolean | Aktiv-Flag |
| version | int | Versionsnummer |
| source | varchar? | Herkunft (seed, chat) |

**Repository:** `IGlobalAgentRuleRepository` → `GlobalAgentRuleRepository`
**Controller:** `GlobalRulesController`
**Seed:** `GlobalAgentRuleSeedService` seedet 3 Default-Regeln (ADDRESS_VALIDATION, ADDRESS_COMPLETENESS, RESPONSE_LANGUAGE).
**History:** Jede Änderung → `global_agent_rule_histories`

---

### global_agent_rule_histories

Audit-Trail für globale Regeln.

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| global_agent_rule_id | uuid | FK → global_agent_rules |
| name | varchar | Regel-Name |
| content_before | text? | Alter Inhalt |
| content_after | text | Neuer Inhalt |
| version | int | Versionsnummer |
| change_type | varchar | Create / Update / Deactivate |
| changed_by | varchar? | Wer hat geändert |
| change_reason | text? | Begründung |

---

## System-Prompt Aufbau (ContextAssemblyPipeline)

Der System-Prompt wird von `ContextAssemblyPipeline` + `LLMSystemPromptBuilder` zusammengebaut:

```
┌─────────────────────────────────────────────────┐
│ === GLOBAL RULES ===                             │  ← global_agent_rules
│ [ADDRESS_VALIDATION]                             │
│ Every address must be validated...               │
│ [RESPONSE_LANGUAGE]                              │
│ Always respond in German (Deutsch).              │
│ ====================                             │
├─────────────────────────────────────────────────┤
│ === GUIDELINES ===                               │  ← ai_guidelines
│ - Be polite and professional                     │
│ - Use available functions when users ask...      │
│ ==================                               │
├─────────────────────────────────────────────────┤
│ === IDENTITY ===                                 │  ← agent_soul_sections
│ [IDENTITY]                                       │
│ You are the Klacks Assistant...                  │
│ [PERSONALITY]                                    │
│ ## Core Truths ...                               │
│ [BOUNDARIES]                                     │
│ ## Boundaries ...                                │
│ [COMMUNICATION_STYLE]                            │
│ ## Vibe ...                                      │
│ [VALUES]                                         │
│ ## Continuity ...                                │
│ ================                                 │
├─────────────────────────────────────────────────┤
│ Du bist ein hilfreicher KI-Assistent...          │  ← Basis-Instruktion
├─────────────────────────────────────────────────┤
│ Benutzer-Kontext:                                │  ← User-Info
│ - User ID: ...                                   │
│ - Berechtigungen: ...                            │
├─────────────────────────────────────────────────┤
│ Verfügbare Funktionen:                           │  ← agent_skills
│ - function_name: description                     │
├─────────────────────────────────────────────────┤
│ === PERSISTENT KNOWLEDGE ===                     │  ← agent_memories
│ [PINNED]                                         │
│ - [fact] company_name: Acme Corp                 │
│ [RELEVANT]                                       │
│ - [preference] ui_preference: Dark mode          │
│ ============================                     │
└─────────────────────────────────────────────────┘
```

### Template-Variablen

In `GLOBAL RULES` und `GUIDELINES` werden Template-Variablen aufgelöst:

| Variable | Beispiel (de) | Beispiel (en) |
|----------|---------------|---------------|
| `{{LANGUAGE}}` | German (Deutsch) | English (English) |
| `{{LANGUAGE_CODE}}` | de | en |

---

## Startup-Pipeline

Beim App-Start werden folgende Seed-Services in dieser Reihenfolge ausgeführt:

```
app.InitializeSkills()              → SkillRegistrationService registriert alle Skills im Registry
await app.SeedAgentSkillsAsync()    → AgentSkillSeedService synct Skills in die DB
await app.SeedGlobalAgentRulesAsync() → GlobalAgentRuleSeedService seedet 3 Default-Regeln
await app.SeedAgentSoulSectionsAsync() → AgentSoulSectionSeedService seedet 5 Soul-Sektionen
```

Alle Seeds sind **idempotent** — beim 2. Start wird nichts dupliziert.

---

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
    ├── AgentSessionRepository.GetOrCreateAsync()    → Session laden/erstellen
    │
    ├── ContextAssemblyPipeline.AssembleSoulAndMemoryPromptAsync()
    │       ├── GlobalAgentRuleRepository.GetActiveRulesAsync()   → === GLOBAL RULES ===
    │       ├── AiGuidelinesRepository.GetActiveAsync()           → === GUIDELINES ===
    │       ├── AgentSoulRepository.GetActiveSectionsAsync()      → === IDENTITY ===
    │       ├── AgentMemoryRepository.GetPinnedAsync()            → [PINNED]
    │       └── AgentMemoryRepository.HybridSearchAsync()         → [RELEVANT]
    │
    ├── LLMSystemPromptBuilder.BuildSystemPromptAsync()
    │       ├── PromptTranslationProvider.GetTranslationsAsync()
    │       └── Fügt User-Context, Funktionen, Memories zusammen
    │
    ▼
LLMProviderRequest { SystemPrompt = ..., Tools = ... }
    │
    ▼
ILLMProvider.ProcessAsync()  → OpenAI / DeepSeek / Anthropic / etc.
    │
    ├── Tool Calls? → SkillExecutorService.ExecuteAsync()
    │                      └── AgentSkillRepository.LogExecutionAsync()
    │
    ▼
LLMResponse → AgentSessionRepository.SaveMessageAsync() → User
```

---

## Skills-Referenz

Alle 10 AI-System-Skills im Überblick:

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
| `configure_heartbeat` | CRUD | CanEditSettings | Heartbeat-Konfiguration |
| `set_user_group_scope` | CRUD | — | Benutzer-Gruppenfilter setzen |

---

## Berechtigungen

| Rolle | Soul | Memory | Guidelines |
|-------|------|--------|-----------|
| Admin | Lesen + Schreiben | Lesen + Schreiben | Lesen + Schreiben |
| CanViewSettings | Lesen | — | Lesen |
| CanEditSettings | Lesen + Schreiben | — | Lesen + Schreiben |
| Andere | — | — | — |

Memory-Skills erfordern explizit **Admin**-Berechtigung, da sie das Verhalten des Assistenten für alle Benutzer beeinflussen.

---

## Background Services

| Service | Intervall | Aufgabe |
|---------|-----------|---------|
| `EmbeddingBackgroundService` | 60 Sekunden | Generiert pgvector-Embeddings für neue agent_memories |
| `MemoryCleanupBackgroundService` | 6 Stunden | Löscht abgelaufene Memories, archiviert Sessions > 30 Tage |

---

## Legacy Datenbank-Tabellen

Diese Tabellen werden per Migration geseeded und dienen als Datenquelle:

### ai_souls

| Spalte | Typ | Beschreibung |
|--------|-----|-------------|
| id | uuid | Primary Key |
| name | varchar | Kurzname |
| content | text | Soul-Inhalt (alle Sektionen als Markdown) |
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
