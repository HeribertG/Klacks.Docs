# UI Element Map — Klacks.Ui

Vollständige Übersicht aller Seiten, Routen und wichtiger Element-IDs für den Chatbot.

---

## Navigation & Layout

| Route | Komponente | Guard |
|-------|-----------|-------|
| `/login` | LoginComponent | - |
| `/workplace` | HomeComponent | AuthGuard |
| `/workplace/dashboard` | DashboardHomeComponent | Lazy |
| `/workplace/client` | AllAddressHomeComponent | Lazy |
| `/workplace/edit-address/:id` | EditAddressHomeComponent | Lazy, CanDeactivate |
| `/workplace/schedule` | ScheduleHomeComponent | Lazy |
| `/workplace/absence` | AbsenceGanttHomeComponent | Lazy |
| `/workplace/client-availability` | ClientAvailabilityHomeComponent | Lazy |
| `/workplace/settings` | SettingsHomeComponent | Lazy, AdminGuard |
| `/workplace/group` | AllGroupHomeComponent | Lazy, AdminGuard |
| `/workplace/edit-group/:id` | EditGroupHomeComponent | Lazy, AdminGuard |
| `/workplace/group-structure` | GroupScopeComponent | Lazy, AdminGuard |
| `/workplace/shift` | AllShiftHomeComponent | Lazy |
| `/workplace/new-shift` | EditShiftHomeComponent | Lazy, AdminGuard |
| `/workplace/edit-shift/:id` | EditShiftHomeComponent | Lazy |
| `/workplace/cut-shift/:id` | CutShiftHomeComponent | Lazy |
| `/workplace/container-template/:id` | ContainerTemplateComponent | Lazy |
| `/workplace/inbox` | InboxHomeComponent | Lazy, InboxGuard |
| `/workplace/floor-plan` | FloorPlanHomeComponent | Lazy |
| `/workplace/profile` | ProfileHomeComponent | Lazy |

---

## Aside Panel (Chat)

```
aside-title                          — Panel-Titel
assistant-chat-clear-btn             — Chat leeren
aside-close-btn                      — Panel schliessen
assistant-chat-messages              — Nachrichten-Container
assistant-chat-input                 — Textarea Eingabe
assistant-chat-send-btn              — Senden-Button
assistant-chat-model-dropdown        — Modell-Auswahl Dropdown
assistant-chat-voice-btn             — Spracheingabe-Button
```

---

## Dashboard (`/workplace/dashboard`)

```
dashboard-expand-all-btn             — Alle Sektionen aufklappen
dashboard-collapse-all-btn           — Alle Sektionen zuklappen
dashboard-section-overview           — Sektion: Übersicht
dashboard-section-coverage           — Sektion: Abdeckung
dashboard-section-locations          — Sektion: Standorte
clients-overview-chart               — Klienten-Übersicht Donut-Chart
coverage-chart                       — Abdeckungs-Chart
sealed-chart                         — Versiegelt-Chart
shifts-overview-chart                — Schicht-Übersicht-Chart
address-search                       — Standort-Suche Input
address-search-btn                   — Standort-Suche Button
map-container                        — Karten-Container
map-style-select                     — Kartenstil-Auswahl
```

---

## Klienten-Liste (`/workplace/client`)

```
address-home-container               — Seiten-Container
address-home-headline                — Überschrift
allAddressForm                       — Hauptformular
new-address-button                   — Neuer Klient Button
address-nav                          — Filter-Navigation
myAddressTable                       — Klienten-Tabelle
client-row-{i}                       — Tabellenzeile (dynamisch)
client-firstname-{i}                 — Vorname-Zelle (dynamisch)
client-lastname-{i}                  — Nachname-Zelle (dynamisch)
client-edit-button-{i}               — Bearbeiten-Button (dynamisch)
client-delete-button-{i}             — Löschen-Button (dynamisch)

Filter-Navigation (all-address-nav):
  navClientForm                      — Filter-Formular
  filter-female                      — Filter: Weiblich
  filter-male                        — Filter: Männlich
  filter-intersexuality              — Filter: Intersexuell
  filter-legal-entity                — Filter: Juristische Person
  dropdownForm-0                     — Typ-Dropdown
  filter-type-employee               — Filter: Mitarbeiter
  filter-type-extern-emp             — Filter: Extern
  filter-type-customer               — Filter: Kunde
  dropdownForm-1                     — Gültigkeit-Dropdown
  filter-active-membership           — Filter: Aktive Mitgliedschaft
  filter-former-membership           — Filter: Ehemalige
  filter-future-membership           — Filter: Zukünftige
  dropdownForm-2                     — Länder-Dropdown
  dropdownForm1                      — Kantone-Dropdown
  dropdownForm4                      — Zeitraum-Dropdown
  filter-show-deleted                — Gelöschte anzeigen
  filter-reset-button                — Filter zurücksetzen
```

---

## Klient bearbeiten (`/workplace/edit-address/:id`)

```
edit-address-home-container          — Seiten-Container
edit-address-home-headline           — Überschrift
membership-form                      — Mitgliedschaft-Formular
client-contracts-form                — Verträge-Formular
client-groups-form                   — Gruppen-Formular
note-form                            — Notizen-Formular
client-image-form                    — Bild-Formular

Persona:
  company                            — Firma Input
  gender                             — Geschlecht Select
  legalEntity                        — Rechtsform Select
  title                              — Titel Input
  firstname                          — Vorname Input
  profile-name                       — Nachname Input
  street                             — Strasse Input
  zip                                — PLZ Input
  city                               — Ort Input
  state                              — Kanton Select
  country                            — Land Select
  profile-birthday                   — Geburtstag
  add-phone-button                   — Telefon hinzufügen
  add-email-button                   — Email hinzufügen
  phoneValue-{i}                     — Telefonnummer (dynamisch)
  emailValue-{i}                     — Email-Adresse (dynamisch)

Mitgliedschaft:
  client-type                        — Mitgliedschafts-Typ
  membership-entry-date              — Eintrittsdatum
  membership-until-date              — Austrittsdatum

Verträge:
  add-contract-button                — Vertrag hinzufügen
  contract-{i}                       — Vertrag Select (dynamisch)
  from-date-{i}                      — Von-Datum (dynamisch)
  until-date-{i}                     — Bis-Datum (dynamisch)
  active-{i}                         — Aktiv-Checkbox (dynamisch)
  delete-contract-{i}                — Vertrag löschen (dynamisch)

Gruppen:
  add-group-button                   — Gruppe hinzufügen
  groupId-{i}                        — Gruppe Select (dynamisch)
  valid-from-{i}                     — Gültig von (dynamisch)
  valid-until-{i}                    — Gültig bis (dynamisch)
  delete-group-{i}                   — Gruppe entfernen (dynamisch)

Notizen:
  new-note-button                    — Neue Notiz
  note-{i}                           — Notiz-Eintrag (dynamisch)

Bild:
  upload-client-image-area           — Bild-Upload Dropzone
  clientImage                        — Datei-Input
  delete-client-image-button         — Bild löschen
```

---

## Dienstplan (`/workplace/schedule`)

```
schedule-main-header                 — Header-Bereich
schedule-body                        — Body-Bereich
schedule-grid-surface                — Grid-Surface Komponente
shift-section-container              — Schicht-Sektion Container
shift-tabs                           — Schicht/Fehler Tabs

Header:
  schedule-prev-btn                  — Vorherige Periode
  schedule-next-btn                  — Nächste Periode
  dropdownSetting                    — Einstellungen-Dropdown
  schedule-wizard-btn                — Wizard starten
  schedule-pdf-export-btn            — PDF-Export
  schedule-send-email-btn            — Email senden
  schedule-break-placeholder-toggle  — Pause-Platzhalter ein/aus
  schedule-availability-check-btn    — Verfügbarkeit prüfen
  schedule-recalculate-btn           — Neuberechnung

Grid:
  schedule-row-header-filter         — Zeilen-Header Filter
  scheduleRowCanvas                  — Schedule Canvas
  shiftRowHeaderCanvas               — Shift Row Canvas

Filter:
  shift-filter-search                — Schicht-Suche Input
  shift-filter-clear-btn             — Filter leeren
  error-filter-btn                   — Fehler-Filter
  warning-filter-btn                 — Warnung-Filter
  info-filter-btn                    — Info-Filter

Dialoge:
  description                        — Beschreibung Textarea (in mehreren Dialogen)
  replaceClient                      — Ersatz-Mitarbeiter Suche
```

---

## Schicht-Verwaltung (`/workplace/shift`)

```
all-shift-home-container             — Seiten-Container
all-shift-header                     — Header-Bereich
allShiftForm                         — Hauptformular
shift-create-btn                     — Neue Schicht erstellen
navShiftForm                         — Filter-Formular
shift-filter-original                — Filter: Original
shift-filter-shift                   — Filter: Schicht
shift-filter-container               — Filter: Container
shift-filter-absence                 — Filter: Abwesenheit

original-shift-table                 — Original-Schichten Tabelle
shift-detail-table                   — Schicht-Detail Tabelle

Edit-Shift:
  edit-shift-home-container          — Seiten-Container
  edit-shift-headline                — Überschrift
  mainShiftForm                      — Hauptformular
  abbreviation                       — Kürzel Input
  name                               — Name Input
  shift-lock-btn                     — Sperren-Button
  validFrom                          — Gültig von
  validUntil                         — Gültig bis
  isContainer                        — Container-Checkbox
  shift-description-editor           — Beschreibung Rich-Text

  Wochentage:
    isMonday ... isSunday            — Wochentag-Checkboxen
    isHoliday                        — Feiertag-Checkbox

  Spezial:
    isSporadic                       — Sporadisch-Checkbox
    sumEmployees                     — Anzahl Mitarbeiter
    sumQuantity                      — Anzahl Menge

  Gruppen:
    shift-group-header               — Gruppen-Header
    groupShiftForm                   — Gruppen-Formular
    myGroupTable                     — Gruppen-Tabelle

  Mitarbeiter:
    shift-address-header             — Mitarbeiter-Header
    shift-address-select-btn         — Mitarbeiter auswählen Button
    addressShiftForm                 — Mitarbeiter-Formular
    member                           — Mitarbeiter-Suche Input

  Makro:
    shift-macro-header               — Makro-Header
    shift-macro-list-select          — Makro-Auswahl

  Spezial-Header:
    shift-special-feature-header     — Spezial-Header
    shift-weekday-header             — Wochentag-Header

Cut-Shift:
  cut-shift-home-container           — Seiten-Container
  cutShiftForm                       — Cut-Formular
  cutDateInput                       — Schnittdatum
  cut-date-btn                       — Datum schneiden
  cut-time-btn                       — Zeit schneiden
  cut-weekdays-btn                   — Wochentage schneiden
  cut-staff-btn                      — Personal schneiden
  cut-task-btn                       — Aufgabe schneiden
  reset-cuts-btn                     — Schnitte zurücksetzen
  shift-cut-btn-{id}                 — Schnitt-Button (dynamisch)
  shift-container-btn-{id}           — Container-Button (dynamisch)
  shift-edit-btn-{id}                — Bearbeiten-Button (dynamisch)

Container-Template:
  container-template-wrapper         — Seiten-Container
  container-template-header          — Header
  start-base                         — Start-Basis Select
  end-base                           — End-Basis Select
  selected-tasks-list                — Ausgewählte Aufgaben (cdkDropList)
  selected-tasks-table               — Ausgewählte Aufgaben Tabelle
  available-tasks-list               — Verfügbare Aufgaben (cdkDropList)
  available-tasks-table              — Verfügbare Aufgaben Tabelle
```

---

## Gruppen-Verwaltung (`/workplace/group`)

```
Listen-Ansicht:
  all-group-list-new-button          — Neue Gruppe
  all-group-list-tree-toggle         — Zur Baumansicht
  all-group-list-table               — Gruppen-Tabelle
  all-group-list-row-{i}             — Tabellenzeile (dynamisch)
  all-group-list-edit-{i}            — Bearbeiten (dynamisch)
  all-group-list-copy-{i}            — Kopieren (dynamisch)
  all-group-list-delete-{i}          — Löschen (dynamisch)
  all-group-list-pagination          — Paginierung

  Filter:
    all-group-validity-dropdown-toggle — Gültigkeit-Dropdown
    all-group-filter-active          — Filter: Aktiv
    all-group-filter-former          — Filter: Ehemalig
    all-group-filter-future          — Filter: Zukünftig

Baum-Ansicht:
  tree-group-expand-button           — Alle aufklappen
  tree-group-collapse-button         — Alle zuklappen
  tree-group-refresh-button          — Aktualisieren
  tree-group-add-root-button         — Wurzel-Gruppe hinzufügen
  tree-group-grid-toggle             — Zur Listenansicht
  tree-group-node-{id}               — Baum-Knoten (dynamisch)
  tree-node-edit-btn-{id}            — Knoten bearbeiten (dynamisch)
  tree-node-add-btn-{id}             — Kind hinzufügen (dynamisch)
  tree-node-delete-btn-{id}          — Knoten löschen (dynamisch)

Gruppe bearbeiten:
  edit-group-item-name               — Gruppenname Input
  edit-group-item-from               — Gültig von
  edit-group-item-until              — Gültig bis
  group-payment-interval-dropdown    — Zahlungsintervall
  group-calendar-dropdown            — Kalender
  group-description-editor           — Beschreibung
  edit-group-parent-select           — Übergeordnete Gruppe
  member                             — Mitglieder-Suche
```

---

## Einstellungen (`/workplace/settings`)

```
Haupt-Container:
  settings-home-container            — Einstellungen Container
  settings-home-headline             — Überschrift

Sektionen (expandierbar):
  settings-general                   — Allgemein
  settings-owner-address             — Firmendaten
  settings-user-administration       — Benutzerverwaltung
  settings-group-scope               — Gruppenstruktur
  settings-identity-providers        — Identity Provider
  settings-absence                   — Abwesenheitstypen
  settings-calendar-selection        — Kalender

Allgemein (settings-general):
  setting-general-name               — App-Name
  setting-general-icon-file-input    — Icon-Upload
  setting-general-logo-file-input    — Logo-Upload
  setting-general-delete-icon-btn    — Icon löschen
  setting-general-delete-logo-btn    — Logo löschen

Firmendaten (owner-address):
  setting-owner-address-name         — Firmenname
  setting-owner-address-street       — Strasse
  setting-owner-address-zip          — PLZ
  setting-owner-address-city         — Ort
  setting-owner-address-country      — Land
  setting-owner-address-state        — Kanton
  setting-owner-address-tel          — Telefon
  setting-owner-address-email        — Email
  setting-owner-address-calendar     — Kalender

Email (email-setting):
  outgoingServer                     — SMTP-Server
  outgoingServerPort                 — Port
  outgoingServerAuthUser             — Benutzer
  outgoingServerAuthKey              — Passwort
  enabledSSL                         — SSL aktiv
  setting-email-test-btn             — Test senden

IMAP (imap-setting):
  imapServer                         — IMAP-Server
  imapPort                           — Port
  imapUsername                       — Benutzer
  imapPassword                       — Passwort
  imapFolder                         — Ordner
  imapEnableSSL                      — SSL aktiv
  setting-imap-test-btn              — Test-Button

LLM-Modelle (llm-models):
  llm-models-modal-model-id          — Modell-ID
  llm-models-modal-model-name        — Modell-Name
  llm-models-modal-provider          — Provider
  llm-models-modal-api-model-id      — API Modell-ID
  llm-models-modal-api-key           — API-Key
  llm-models-modal-is-enabled        — Aktiviert
  llm-models-modal-is-default        — Standard
  llm-models-modal-save-btn          — Speichern

LLM-Provider (llm-providers):
  llm-providers-modal-provider-name  — Provider-Name
  llm-providers-modal-base-url       — Base-URL
  llm-providers-modal-is-enabled     — Aktiviert
  llm-providers-modal-save-btn       — Speichern

Verträge (contracts):
  contractName                       — Vertragsname
  contract-modal-guaranteed-hours    — Garantierte Stunden
  contract-modal-fulltime-input      — Vollzeit-Stunden
  nightRate / holidayRate / saRate / soRate — Zuschläge
  contract-modal-save-btn            — Speichern

Filialen (branches):
  branches-modal-name                — Filialname
  branches-modal-address             — Adresse
  branches-modal-phone               — Telefon
  branches-modal-email               — Email
  branches-modal-save-btn            — Speichern

Abwesenheiten (absence):
  absence-modal-name                 — Abwesenheitsname
  absence-modal-abbreviation         — Kürzel
  absence-modal-color-picker         — Farbe
  absence-modal-save-btn             — Speichern
  absence-add-btn                    — Hinzufügen

DeepL:
  deepl-apikey                       — API-Key
  deepl-apikey-toggle                — Sichtbarkeit

OpenRoute:
  openroute-apikey                   — API-Key
  openroute-apikey-toggle            — Sichtbarkeit
```

---

## Profil (`/workplace/profile`)

```
profile-home-container               — Profil-Container
profile-picture                      — Profilbild
profile-data-edit                    — Daten bearbeiten
profile-custom-setting               — Benutzerdefinierte Einstellungen
profile-microphone-test              — Mikrofon-Test
```

---

## Shared Components

```
Grid:
  template-canvas{id}                — Grid Canvas (dynamisch)
  cell-input (data-testid)           — Zellen-Input

Pagination:
  pagination                         — Seitennavigation
  pagination-total-count             — Gesamtanzahl
  selection                          — Seitenauswahl

Client-Filter:
  client-filter-container            — Filter-Container
  client-filter-firstname-button     — Vorname-Filter
  client-filter-name-button          — Nachname-Filter
  client-filter-company-button       — Firma-Filter
  client-filter-hours-button         — Stunden-Filter

Gruppe-Select:
  group-select-dropdown-toggle{suffix} — Gruppen-Dropdown
  group-node-{id}                    — Gruppen-Knoten (dynamisch)
  group-option-{id}                  — Gruppen-Option (dynamisch)

Kalender:
  select-calendar                    — Kalender-Auswahl
  calendar-dropdown-menu             — Kalender-Dropdown
  calendar-dropdown-country-select   — Land-Auswahl

Context-Menu:
  context-menu                       — Kontextmenü
  {menuItem.key}                     — Menü-Eintrag (dynamisch)
```
