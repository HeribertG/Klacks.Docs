# Periodenabschluss & Export

## Übersicht
Bevor Arbeitszeiten exportiert werden können (für Lohnbuchhaltung, ERP oder Kundenabrechnung), muss die betroffene Periode über den **Periodenabschluss** (`/workplace/period-closing`) versiegelt werden. Erst danach ist ein Eintrag exportierbar (`LockLevel == Closed`).

Der Export ist ein **Leistungsnachweis** (Proof-of-Service), keine Rechnung — die Preisbildung übernimmt die nachgelagerte Buchhaltungssoftware.

## Zwei Export-Wege

### Bestellungs-Export ("Bestellungen")
Bezieht sich auf eine oder mehrere versiegelte Bestellungen (SealedOrder). Der Export sammelt automatisch alle zugehörigen Einträge, auch wenn die Bestellung zwischenzeitlich umbenannt oder in mehrere Teile aufgesplittet wurde.

| Format | Verwendung |
|--------|-----------|
| CSV | Flache Tabelle, ein Eintrag pro Zeile |
| JSON | Hierarchisch, für Systemintegration |
| XML | Strukturiert, für Systemintegration |
| DATEV | Deutsche Buchhaltung (Buchungsstapel) |
| BMD | Österreichische Buchhaltung |

### Mitarbeiterstunden-Export ("Employee")
Stunden/Spesen/Pausen pro Mitarbeiter für einen Zeitraum, unabhängig von einzelnen Bestellungen. Für Lohn-Exporte zusätzlich länderspezifische Formate wählbar (z.B. AbaConnect für die Schweiz, PAXml/SIE4B für Schweden, POHODA für Tschechien, BrightPay für Irland/UK, DATEV-LuG für Deutschland, und weitere).

## Format-Verwaltung
Welche zusätzlichen Formate sichtbar sind, legt ein Administrator zentral fest (Settings). CSV/JSON/XML für Bestellungen sind immer verfügbar; alle anderen Formate müssen freigeschaltet werden.

## Voraussetzung: Periodenabschluss
Der Export greift ausschliesslich auf abgeschlossene (versiegelte) Perioden zu. Eine offene Periode liefert keine oder unvollständige Export-Daten — zuerst im Periodenabschluss-Bereich die betroffene Periode versiegeln.

## Siehe auch
- `klacks://docs/shifts` — Schichtplanung
- `klacks://docs/general` — Navigation und Rollen
