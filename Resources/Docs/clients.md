# Mitarbeiter- und Kundenverwaltung

## Übersicht
"Client" ist in Klacks eine gemeinsame Entität für drei Personentypen (`EntityTypeEnum`): **Employee** (interner Mitarbeiter), **ExternEmp** (externer Mitarbeiter) und **Customer** (Kunde/Auftraggeber, relevant für den Bestellungs-Export). Die meisten UI-Bereiche zeigen nur Mitarbeiter, aber Kunden nutzen dieselbe Entität und Adressverwaltung.

## Mitarbeiter anlegen

### Pflichtfelder
- Vorname
- Nachname
- Geschlecht (Frau/Herr/Divers/Juristische Person — Juristische Person für Firmenkunden ohne Einzelperson)

### Optionale Felder
- Geburtsdatum
- E-Mail
- Telefon
- Adresse (Strasse, PLZ, Ort, Kanton, Land)
- Gruppe/Abteilung

## Verfügbarkeit

Es gibt kein separates "Mitarbeiter-Status"-Feld. Ob ein Mitarbeiter aktuell planbar ist, ergibt sich aus seinen Verträgen: ein Contract ist über `ValidFrom`/`ValidUntil` zeitlich begrenzt (siehe Abschnitt "Verträge"). Ein Mitarbeiter ohne gültigen Vertrag im betrachteten Zeitraum gilt faktisch als nicht einsatzbereit. Löschung erfolgt als SoftDelete (`IsDeleted`-Flag), nicht als Status-Wechsel.

## Gruppen und Abteilungen

Mitarbeiter können Gruppen zugeordnet werden:
- Hierarchische Struktur möglich
- Mehrfachzuordnung erlaubt
- Berechtigungen auf Gruppenebene

### Gruppen-Pfad
Format: `Firma > Abteilung > Team`
Beispiel: `Klacks AG > IT > Entwicklung`

## Import-Möglichkeiten

### Manueller Import
CSV-Datei mit Spalten:
```
Vorname;Nachname;Email;Geburtsdatum;Strasse;PLZ;Ort
```

### LDAP/AD-Synchronisation
Automatischer Import aus Active Directory oder LDAP.
Siehe: Identity Provider Dokumentation

## Verträge

Jeder Mitarbeiter kann mehrere Verträge haben:
- Verschiedene Pensen über `Percent` (z.B. 80%, 100%)
- Zeitlich begrenzt (`ValidFrom`/`ValidUntil`) oder unbefristet
- Kalenderzuordnung pro Vertrag (`CalendarSelection`), z.B. für kantonale Feiertage

### Vertrags-Parameter (frei konfigurierbar, keine festen Typen)
Es gibt keinen festen Katalog von Vertragstypen ("Vollzeit 160" o.ä.) — jeder Contract wird individuell konfiguriert:

| Feld | Beschreibung |
|------|-------------|
| GuaranteedHours | Garantierte Stunden pro Zahlungsintervall; leer = erbt vom firmenweiten Wert, skaliert mit Percent |
| FullTime | Vollzeit-Stunden-Referenz für die Zuschlagsberechnung |
| Percent | Pensum in Prozent (z.B. 80 = 80%), skaliert die geerbten Stunden |
| PaymentInterval | Weekly / Biweekly / Monthly / MonthlyTargetHours / Individual |
| WorkOnMonday...WorkOnSunday | Arbeitstage-Flags pro Wochentag |
| NightRate/HolidayRate/WE1-3Rate | Zuschlagssätze, siehe Macro-Dokumentation |

Ein `GuaranteedHours` von explizit 0 markiert einen Bereitschafts-/On-Call-Vertrag ohne garantierte Stunden.

## Suche

### Suchfelder
- Name (Vor- und Nachname)
- E-Mail
- Personalnummer
- Gruppe

### Filteroptionen
- Nach Kanton
- Nach Gruppe
- Nach Personentyp (Employee/ExternEmp/Customer)
