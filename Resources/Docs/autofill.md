# Wizard / Autofill — Automatische Schichtplanung

## Übersicht
Der Wizard füllt einen Schichtplan automatisch mit einem evolutionären Algorithmus (genetischer Algorithmus), statt dass jede Schicht manuell zugewiesen wird. Er ersetzt die frühere TypeScript-Engine vollständig.

## Wie es funktioniert
1. Der Wizard sammelt alle offenen Slots (Schicht × Datum) im gewählten Zeitraum sowie die verfügbaren Mitarbeiter mit ihren Vertragsdaten.
2. Bereits gesperrte Einträge (`LockLevel > 0`) werden nie verändert.
3. Ein genetischer Algorithmus optimiert über mehrere Generationen, bis eine gute Lösung gefunden ist oder das Generationenlimit erreicht ist.
4. Das Ergebnis kann entweder direkt übernommen oder zuerst als **Szenario** (What-If, isoliert von den echten Daten) geprüft werden, bevor es angewendet wird.

## Priorisierung (was der Algorithmus zuerst erfüllt)
1. **Harte Regeln** (müssen immer erfüllt sein): keine doppelte Buchung, Mindestpause zwischen Schichten, maximale Tagesstunden, maximale Anzahl aufeinanderfolgender Arbeitstage, vollständige Slot-Abdeckung wo möglich.
2. **Garantierte Stunden** pro Mitarbeiter (aus dem Vertrag).
3. **Vollzeit-Abdeckung** mit abnehmender Priorität für bereits gut ausgelastete Mitarbeiter.
4. **Weiche Regeln**: bevorzugte Reihenfolge von Schichtblöcken, Präferenzen/Sperrlisten, Standort-Kontinuität.
5. **Kosmetik**: Fairness zwischen Mitarbeitern, symmetrische Blöcke.

## Szenario-Modus (What-If)
Ein Wizard-Lauf kann als isoliertes Szenario ausgeführt werden, ohne die produktiven Daten zu verändern. Erst wenn das Ergebnis übernommen wird, werden die Einträge real angelegt (inkl. automatischer Neuberechnung von Periodenstunden, Kollisionsprüfung und Benachrichtigungen).

## Aktueller Stand
Produktiv im Einsatz, umfassend getestet (1400+ Unit-Tests, End-to-End-Tests gegen echte Datenbank). Bekannte offene Punkte: doppelt besetzte Slots (Overstaffing) werden noch nicht bestraft, bei hoher Zufalls-Einstellung können daher gelegentlich überzählige Einträge entstehen.

## Siehe auch
- `klacks://docs/shifts` — manuelle Schichtplanung
- `klacks://docs/general` — Rollen/Berechtigungen
