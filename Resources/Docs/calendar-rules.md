# Ewigkeitskalender - Feiertagsregeln

## Übersicht
Der Ewigkeitskalender berechnet Feiertage automatisch für jedes Jahr basierend auf definierten Regeln. Er unterstützt sowohl feste Daten als auch bewegliche Feiertage (z.B. Ostern-bezogene).

## Regel-Formate

### Feste Daten
Format: `MM/DD`

| Beispiel | Bedeutung |
|----------|-----------|
| `01/01` | 1. Januar (Neujahr) |
| `12/25` | 25. Dezember (Weihnachten) |
| `08/01` | 1. August (Schweizer Nationalfeiertag) |

### Feste Daten mit Wochentag-Verschiebung
Format: `MM/DD+XXX+WW` oder `MM/DD+XXX-WW`

| Parameter | Beschreibung |
|-----------|--------------|
| `MM/DD` | Monat/Tag als Ausgangspunkt |
| `XXX` | Offset in Tagen (000 bis 999) |
| `+` | Nächster Wochentag nach dem Datum |
| `-` | Vorheriger Wochentag vor dem Datum |
| `WW` | Wochentag-Kürzel |

| Beispiel | Bedeutung |
|----------|-----------|
| `11/01+000+TH` | Erster Donnerstag im November |
| `11/22+000+TH` | Vierter Donnerstag im November (Thanksgiving) |
| `05/25+000-MO` | Letzter Montag im Mai (Memorial Day) |

### Oster-bezogene Daten
Format: `EASTER+XX` oder `EASTER-XX`

Das Osterdatum wird nach der Gaußschen Osterformel berechnet.

| Beispiel | Bedeutung | Offset |
|----------|-----------|--------|
| `EASTER+00` | Ostersonntag | 0 |
| `EASTER-02` | Karfreitag | -2 |
| `EASTER+01` | Ostermontag | +1 |
| `EASTER+39` | Auffahrt/Christi Himmelfahrt | +39 |
| `EASTER+49` | Pfingstsonntag | +49 |
| `EASTER+50` | Pfingstmontag | +50 |
| `EASTER+60` | Fronleichnam | +60 |

## SubRule-Format

SubRules ermöglichen die automatische Verschiebung eines Feiertags, wenn er auf einen bestimmten Wochentag fällt.

Format: `WW+X` oder `WW-X`, mehrere Regeln mit `;` getrennt

| Parameter | Beschreibung |
|-----------|--------------|
| `WW` | Wochentag-Kürzel |
| `+X` | X Tage nach vorne verschieben |
| `-X` | X Tage nach hinten verschieben |

| Beispiel | Bedeutung |
|----------|-----------|
| `SA+2;SU+1` | Samstag → Montag (+2), Sonntag → Montag (+1) |
| `SU+1` | Sonntag → Montag |

**Anwendungsfall:** Feiertage, die auf ein Wochenende fallen, auf den nächsten Werktag verschieben.

## Wochentag-Kürzel

| Kürzel | Wochentag |
|--------|-----------|
| `SU` | Sonntag (Sunday) |
| `MO` | Montag (Monday) |
| `TU` | Dienstag (Tuesday) |
| `WE` | Mittwoch (Wednesday) |
| `TH` | Donnerstag (Thursday) |
| `FR` | Freitag (Friday) |
| `SA` | Samstag (Saturday) |

## Zusätzliche Felder

| Feld | Beschreibung |
|------|--------------|
| Name | Bezeichnung des Feiertags (mehrsprachig) |
| Beschreibung | Optionale Beschreibung (mehrsprachig) |
| Land | Ländercode (z.B. CH, DE, AT) |
| Kanton/Bundesland | Regionale Zuordnung |
| Gesetzlicher Feiertag | Offizieller/gesetzlicher Feiertag |
| Bezahlt | Relevant für Lohnberechnung |

## Beispiele für typische Feiertage

### Schweiz
| Feiertag | Regel | Kanton |
|----------|-------|--------|
| Neujahr | `01/01` | Alle |
| Berchtoldstag | `01/02` | BE, ZH, AG... |
| Karfreitag | `EASTER-02` | Alle (ausser VS, TI) |
| Ostermontag | `EASTER+01` | Alle |
| Auffahrt | `EASTER+39` | Alle |
| Pfingstmontag | `EASTER+50` | Alle |
| Bundesfeiertag | `08/01` | Alle |
| Weihnachten | `12/25` | Alle |

### Deutschland
| Feiertag | Regel | Bundesland |
|----------|-------|------------|
| Neujahr | `01/01` | Alle |
| Karfreitag | `EASTER-02` | Alle |
| Ostermontag | `EASTER+01` | Alle |
| Tag der Arbeit | `05/01` | Alle |
| Christi Himmelfahrt | `EASTER+39` | Alle |
| Pfingstmontag | `EASTER+50` | Alle |
| Tag der Deutschen Einheit | `10/03` | Alle |
| Weihnachten | `12/25` | Alle |
| 2. Weihnachtstag | `12/26` | Alle |

## Technische Details

Die Berechnung erfolgt im Frontend in der Klasse `HolidaysListHelper`:
- Osterdatum wird nach der Gaußschen Osterformel berechnet
- Regelstrings werden zur Laufzeit interpretiert
- SubRules werden nach der Hauptregel angewendet
