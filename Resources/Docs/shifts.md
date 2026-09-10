# Schichtplanung

## Übersicht
Die Schichtplanung ermöglicht die Zuweisung von Arbeitszeiten an Mitarbeiter — manuell oder automatisch über den Wizard/Autofill (siehe `klacks://docs/autofill`).

## Wie ein Dienst entsteht: Bestellung → Dienst

Ein planbarer Dienst lässt sich in Klacks **nicht direkt anlegen** — jeder Dienst entsteht ausschliesslich durch das Versiegeln einer Bestellung, und das Versiegeln ist unumkehrbar. Es gibt vier Stufen:

| Stufe | Bedeutung |
|-------|-----------|
| OriginalOrder | Entwurf einer Bestellung, bearbeitbar, noch kein planbarer Dienst |
| SealedOrder | Versiegelte Bestellung — unveränderlicher Vertrag mit dem Kunden |
| OriginalShift | Aktive Planungs-Kopie der versiegelten Bestellung — hier werden Mitarbeiter effektiv eingeteilt |
| SplitShift | Zeit-/Tages-Zuschnitt eines OriginalShift (z.B. wenn eine Schicht geteilt wird) |

Beim Anlegen entscheidet nur **wann versiegelt wird**, nicht "Bestellung oder Dienst":
- Als Entwurf speichern → bleibt OriginalOrder, bearbeitbar, noch kein Dienst
- Direkt speichern (Standard) → sofort versiegelt, direkt planbarer Dienst
- Ein Import aus einem ERP-System erzeugt immer nur Entwürfe, nie automatisch versiegelte Dienste

## Kundenlose Dienste

Nicht jeder Dienst gehört zu einem Kunden. Ein Dienst ohne Kunde ist kein unvollständiger Datensatz, sondern eine eigene, gültige Kategorie — die Unterscheidung ist, ob die Arbeitsstunden einem bestimmten Auftraggeber zurechenbar sind:

- **Mit Kunde:** Kunde → Bestellung → Dienst → Arbeitsstunden sind diesem Auftraggeber zurechenbar.
- **Ohne Kunde:** die Arbeitsstunden gehören zum Betrieb selbst, nicht zu einem Auftraggeber.

Drei typische Fälle für kundenlose Dienste:
1. **Versteckter Teil der Dienstleistung** — bezahlte Arbeitszeit, die keinem Kunden verrechnet werden kann (z.B. Tanken, Fahrzeugpflege, Reinigung).
2. **Innendienst** — Büroarbeit, Verwaltung, Disposition.
3. **Ganze Berufsbilder** — z.B. Pflegepersonal auf Station, Küche, Coiffeur-Salon: hier ist der kundenlose Dienst der **Regelfall**, nicht die Ausnahme, weil es entweder keinen abrechenbaren Kunden gibt oder viele wechselnde (Patienten, Gäste).

Alle kundenlosen Dienste zählen trotzdem normal als Arbeitszeit ins Stundensoll des Mitarbeiters — der fehlende Kunde wirkt sich nur auf Abrechnung/Auswertung pro Kunde aus, nicht auf die Zeiterfassung.

**Anlegen:** Über die Ansicht "Planbare Dienste" gibt es einen eigenen Button für kundenlose Dienste. Ein kundenloser Dienst wird sofort versiegelt angelegt (kein Entwurfsstadium) und muss deshalb schon beim Anlegen vollständig sein (Kürzel, Name, Startdatum, Wochentag, Gruppe, Anzahl Mitarbeiter). Eine Bestellung mit Kunde ohne Kunden-Angabe wird dagegen abgelehnt — der Kunde ist nur bei kundenlosen Diensten optional, nicht generell.

## Schicht-Typen

### Reguläre Schichten
- Feste Start- und Endzeit
- Wiederkehrend (täglich, wöchentlich)
- Mit oder ohne Pause

### Bereitschaft
- On-Call Dienste
- Pikettdienst
- Rufbereitschaft

### Abwesenheiten
- Ferien
- Krankheit
- Weiterbildung
- Sonderurlaub

## Schicht anlegen

### Pflichtfelder
| Feld | Beschreibung |
|------|-------------|
| Mitarbeiter | Zugewiesener Mitarbeiter |
| Datum | Tag der Schicht |
| Startzeit | Beginn der Schicht |
| Endzeit | Ende der Schicht |
| Schichttyp | Art der Schicht |

### Optionale Felder
- Pause (Dauer in Minuten)
- Bemerkung
- Standort
- Projekt/Kostenstelle

## Ansichten

### Tagesansicht
- Alle Schichten eines Tages
- Sortiert nach Startzeit
- Überschneidungen sichtbar

### Wochenansicht
- Montag bis Sonntag
- Mitarbeiter als Zeilen
- Schichten als Blöcke

### Monatsansicht
- Kalenderübersicht
- Aggregierte Stunden
- Feiertage markiert

## Regeln und Validierung

### Arbeitszeit-Regeln
- Maximale Tagesstunden
- Ruhezeit zwischen Schichten
- Wochenarbeitszeit-Limite

### Konflikte
- Überlappende Schichten werden markiert
- Doppelbuchungen werden verhindert
- Warnungen bei Regelverstoß

## Vorlagen

### Schicht-Vorlagen
Häufig verwendete Schichten als Vorlage speichern:
- Frühschicht: 06:00 - 14:00
- Spätschicht: 14:00 - 22:00
- Nachtschicht: 22:00 - 06:00

### Wochen-Vorlagen
Ganze Wochenpläne als Vorlage speichern und wiederverwenden.

## Export & Periodenabschluss

Bevor exportiert werden kann, muss die Periode über den Periodenabschluss (`/workplace/period-closing`) versiegelt werden — nur Einträge mit `LockLevel == Closed` werden exportiert. Der Export selbst ist bestellungsbasiert (nicht datumsbasiert) und liefert einen Leistungsnachweis, keine Rechnung.

### Formate
- CSV, JSON, XML (immer verfügbar)
- DATEV, BMD (Buchhaltung, Admin-aktivierbar)
- Länderspezifische Lohn-Exportformate (Admin-konfigurierbar), z.B. AbaConnect (CH), PAXml/SIE4B (SE), POHODA (CZ), BrightPay (IE/UK) u.v.m.

Siehe: `klacks://docs/exports` für Details.
