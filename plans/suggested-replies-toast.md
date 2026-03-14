# Plan: Suggested Replies als Toast

## Ziel

Der Chatbot soll dem User Rückfragen stellen können, wobei die Antwort-Optionen als schwebendes Toast-Element über dem Chat-Input angezeigt werden. Zusätzlich soll der Chatbot die gesamte UI per Element-IDs kennen und bedienen können.

---

## Ist-Zustand

Das System hat bereits einen einfachen Suggestion-Mechanismus:
- **Backend**: `LLMResponseBuilder` parst `[SUGGESTIONS: "opt1" | "opt2" | "opt3"]` aus LLM-Antworten in `List<string>` auf `LLMResponse.Suggestions`
- **Frontend**: `IAssistantChatResponse.suggestions` ist ein `string[]`, dargestellt als klickbare Chip-Buttons (`.suggestion-btn`) **inline in der Chat-Bubble**
- **Limitierungen**: Keine Unterscheidung Single/Multi-Select, kein Label/Value-Trennung, Chips bleiben im Chat-Verlauf statt als temporäres Element

---

## Teil A: Suggested Replies als Toast

### Schritt 1: Backend — Neue Models

**Neue Datei**: `Domain/Models/Assistant/SuggestedReply.cs`

```csharp
public class SuggestedReply
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
```

**Neue Datei**: `Domain/Models/Assistant/SuggestedRepliesConfig.cs`

```csharp
public class SuggestedRepliesConfig
{
    public string SelectionMode { get; set; } = SuggestedReplySelectionModes.Single;
    public string? Prompt { get; set; }
    public List<SuggestedReply> Options { get; set; } = new();
}
```

**Neue Datei**: `Domain/Constants/SuggestedReplySelectionModes.cs`

```csharp
public static class SuggestedReplySelectionModes
{
    public const string Single = "single";
    public const string Multi = "multi";
}
```

### Schritt 2: Backend — `LLMResponse` erweitern

**Ändern**: `Domain/Models/Assistant/LLMResponse.cs`

```csharp
public SuggestedRepliesConfig? SuggestedReplies { get; set; }
```

Bestehende `Suggestions` Property bleibt für Rückwärtskompatibilität.

### Schritt 3: Backend — `LLMResponseBuilder` Parsing erweitern

**Ändern**: `Domain/Services/Assistant/LLMResponseBuilder.cs`

Neues Format das der LLM ausgeben soll:

```
[REPLIES:single "Bern" | "Zürich" | "St. Gallen"]
[REPLIES:multi:Welche Schichten? "Frühdienst=FD" | "Spätdienst=SD" | "Nachtdienst=ND"]
```

Neue Methode `ExtractSuggestedReplies()`:
1. Regex matcht `[REPLIES:...]` Block
2. Parst Selection-Mode (`single`/`multi`)
3. Parst optionalen Prompt-Text (nach `multi:`)
4. Parst Optionen als `Label=Value` oder einfach `Label` (wo Value = Label)
5. Baut `SuggestedRepliesConfig`
6. Entfernt Block aus dem Response-Content

Falls nur `[SUGGESTIONS:...]` vorhanden → Auto-Konvertierung zu `SuggestedRepliesConfig` mit `SelectionMode = "single"`.

### Schritt 4: Backend — System Prompt erweitern

**Ändern**: `Infrastructure/Services/Assistant/PromptTranslationProvider.cs`

Neues Segment `SuggestedRepliesFormat`:

```
ANTWORT-VORSCHLÄGE: Wenn du dem Benutzer Optionen zur Auswahl geben möchtest,
verwende dieses Format am Ende deiner Nachricht:
- Einfache Auswahl: [REPLIES:single "Option1" | "Option2" | "Option3"]
- Mehrfachauswahl: [REPLIES:multi:Überschrift "Label1=Wert1" | "Label2=Wert2"]
- Maximal 6 Optionen
- Verwende REPLIES nur wenn es sinnvoll ist (Rückfragen, Auswahl)
```

**Ändern**: `Domain/Services/Assistant/LLMSystemPromptBuilder.cs`

Segment `t["SuggestedRepliesFormat"]` an System-Prompt anhängen.

### Schritt 5: Frontend — Neue Models

**Neue Datei**: `domain/models/assistant/suggested-reply.interface.ts`

```typescript
export interface ISuggestedReply {
  label: string;
  value: string;
}

export interface ISuggestedRepliesConfig {
  selectionMode: 'single' | 'multi';
  prompt?: string;
  options: ISuggestedReply[];
}
```

### Schritt 6: Frontend — API Response erweitern

**Ändern**: `infrastructure/api/assistant/data-assistant.service.ts`

`IAssistantChatResponse` um `suggestedReplies?: ISuggestedRepliesConfig` erweitern.

### Schritt 7: Frontend — ChatMessage erweitern

**Ändern**: `presentation/aside/assistant-chat/assistant-chat.component.ts`

`ChatMessage` Interface um `suggestedReplies?: ISuggestedRepliesConfig` erweitern.

In `sendMessage()` Mapping:

```typescript
suggestedReplies: response?.suggestedReplies
  ?? this.convertLegacySuggestions(response?.suggestions)
```

### Schritt 8: Frontend — Neue Overlay-Komponente

**Neue Dateien**:
- `presentation/aside/assistant-chat/suggested-replies-overlay/suggested-replies-overlay.component.ts`
- `presentation/aside/assistant-chat/suggested-replies-overlay/suggested-replies-overlay.component.html`
- `presentation/aside/assistant-chat/suggested-replies-overlay/suggested-replies-overlay.component.scss`

**Inputs**: `config: InputSignal<ISuggestedRepliesConfig | null>`
**Outputs**: `replySelected: OutputEmitterRef<string>`

#### Single-Select: Chips

```
┌──────────────────────────────────────────────────┐
│  Für welchen Standort?                           │
│  [ Bern ]  [ Zürich ]  [ St. Gallen ]  [ Genf ] │
└──────────────────────────────────────────────────┘
═══════════════════════════════════════════════════
│  Nachricht eingeben...              [ Senden ] │
═══════════════════════════════════════════════════
```

#### Multi-Select: Card mit Checkboxen

```
┌──────────────────────────────────────────────────┐
│  Welche Schichten sollen erstellt werden?         │
│  ☑ Frühdienst (07:00-15:00)                     │
│  ☑ Spätdienst (15:00-23:00)                     │
│  ☐ Nachtdienst (23:00-07:00)                    │
│                            [ Bestätigen ]         │
└──────────────────────────────────────────────────┘
═══════════════════════════════════════════════════
│  Nachricht eingeben...              [ Senden ] │
═══════════════════════════════════════════════════
```

**Styling**:
- Position: `absolute; bottom: 0` innerhalb eines `position: relative` Wrappers
- Background: `var(--backgroundColorCard)` mit Border und Box-Shadow
- Animation: slide-up + fade-in
- Chips: gleicher Style wie bestehende `.suggestion-btn`
- z-index: zwischen Chat-Messages und Modals

**Verhalten**:
- Single-Select: Klick → Value wird sofort als User-Message gesendet, Overlay verschwindet
- Multi-Select: Checkboxen anklicken → "Bestätigen" → Werte komma-separiert senden
- Overlay verschwindet bei: Auswahl, neuer User-Eingabe, neuer Bot-Antwort

### Schritt 9: Frontend — Integration in Chat-Komponente

**Ändern**: `assistant-chat.component.ts`

```typescript
activeSuggestedReplies = signal<ISuggestedRepliesConfig | null>(null);

onSuggestedReplySelected(value: string): void {
  this.activeSuggestedReplies.set(null);
  this.inputText = value;
  this.sendMessage();
}
```

**Ändern**: `assistant-chat.component.html`

```html
<div class="input-area-wrapper" style="position: relative;">
  <app-suggested-replies-overlay
    [config]="activeSuggestedReplies()"
    (replySelected)="onSuggestedReplySelected($event)"
  />
  <div class="input-container">
    ...existing input...
  </div>
</div>
```

### Schritt 10: i18n

Neue Keys in `de.json`, `en.json`, `fr.json`, `it.json`:

```json
"assistant-chat.replies.confirm": "Bestätigen"
```

---

## Teil B: UI-Kenntnis des Chatbots

### Schritt 11: UI-Element-Map als GlobalAgentRule

Statt einer eigenen Constants-Klasse wird die UI-Map als **GlobalAgentRule** in die DB geseedet. So fliesst sie automatisch über die `ContextAssemblyPipeline` in den System-Prompt.

**Ändern**: `Infrastructure/Persistence/Seed/GlobalAgentRuleSeedService.cs`

Neuer Seed-Eintrag:

```csharp
new GlobalAgentRule
{
    Name = "ui_element_map",
    Content = @"
=== UI ELEMENT MAP ===
Dashboard: /dashboard
Settings: /workplace/settings
  - General: settings-general-card
  - Owner Address: settings-owner-card
  - Email: settings-email-card
Client List: /workplace/client
  - Search: search-include-client
Schedule: /workplace/schedule
Absence Gantt: /workplace/absence-gantt
Shift Management: /workplace/shift
Group Management: /workplace/group
=== END UI MAP ==="
}
```

**Vorteil**: Kann in der DB aktualisiert werden ohne Code-Deployment.

### Schritt 12: Element-IDs im Frontend sicherstellen

Prüfen und ggf. ergänzen, dass alle wichtigen UI-Elemente stabile `id`-Attribute haben. Bestehende UiAction-Skills nutzen bereits `HandlerConfig` mit Element-Referenzen — diese IDs müssen mit der UI-Map übereinstimmen.

---

## Datenfluss

```
1. User fragt etwas → Backend LLM verarbeitet
2. LLM gibt [REPLIES:single "Ja" | "Nein"] am Ende der Antwort aus
3. LLMResponseBuilder.ExtractSuggestedReplies() parst → SuggestedRepliesConfig
4. LLMResponse.SuggestedReplies wird gesetzt, Text bereinigt
5. Frontend empfängt IAssistantChatResponse mit suggestedReplies
6. assistant-chat.component setzt activeSuggestedReplies Signal
7. SuggestedRepliesOverlayComponent rendert Chips/Card über dem Input
8. User klickt Chip → Value emitted → onSuggestedReplySelected(value)
   → inputText = value → sendMessage() → Overlay verschwindet
9. Multi-Select: User checkt Optionen → "Bestätigen" → Werte komma-separiert gesendet
```

---

## Implementierungs-Reihenfolge

| Phase | Beschreibung | Dateien |
|-------|-------------|---------|
| **1** | Backend Models & Parsing | `SuggestedReply.cs`, `SuggestedRepliesConfig.cs`, `SuggestedReplySelectionModes.cs`, `LLMResponse.cs`, `LLMResponseBuilder.cs` |
| **2** | System Prompt Update | `PromptTranslationProvider.cs`, `LLMSystemPromptBuilder.cs` |
| **3** | Frontend Models & Transport | `suggested-reply.interface.ts`, `data-assistant.service.ts`, `assistant-chat.component.ts` |
| **4** | Frontend Overlay-Komponente | `suggested-replies-overlay/` (3 Dateien), i18n JSONs, `assistant-chat.component.html/scss` |
| **5** | UI-Map (parallel möglich) | `GlobalAgentRuleSeedService.cs`, HTML id-Attribute prüfen |

---

## Risiken & Mitigierungen

| Risiko | Mitigation |
|--------|-----------|
| LLM hält `[REPLIES:...]` Format nicht ein | Robustes Parsing mit Fallback auf `[SUGGESTIONS:...]`, klare Prompt-Anweisungen mit Beispielen |
| Rückwärtskompatibilität | Bestehende `suggestions` Property bleibt, Auto-Konvertierung zu neuem Format |
| Overlay-Positionierung im Aside-Panel | `position: absolute` innerhalb `position: relative` Wrapper, kein Overflow |
| Multi-Select Werte-Zusammenführung | Komma-separierter natürlicher Text (z.B. "Frühdienst, Spätdienst") |
| Overlay bei User-Eingabe | `(input)` Listener auf Textarea löscht `activeSuggestedReplies` |

---

## Verifikation

1. `dotnet build --no-restore` — Backend baut erfolgreich
2. `npx ng build` — Frontend baut erfolgreich
3. `npm run test:no-report` — Bestehende Tests grün
4. Manuell: Chat öffnen, Frage stellen die Rückfrage auslöst, Chips erscheinen, Klick sendet Antwort
5. Manuell: Multi-Select testen (falls LLM multi-Format nutzt)
