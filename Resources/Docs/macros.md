# Macro Scripting Handbuch

## Übersicht
Mit dem Macro Editor lassen sich Skripte schreiben, die als Rechengrundlage für Dienste, Beschäftigungen etc. verwendet werden.

**Anwendungsbeispiele:**
- Exakte Berechnung der Stunden bei Ferien oder Militär gemäss Beschäftigungsgrad
- Berechnung von Zuschlägen für Nachtarbeit, Wochenenden und Feiertage
- Flexible Anpassung an neue Anforderungen

*Der Script Interpreter läuft in einer sicheren Sandbox-Umgebung ohne Zugriff auf das Dateisystem oder Netzwerk.*

## Variablen und Konstanten

Alle Variablen und Konstanten sind typenlos.

**WICHTIG:** `DIM` kann Variablen nur deklarieren, NICHT gleichzeitig initialisieren (wie in VB vor Version 6 / VBA).

```basic
' RICHTIG:
DIM x
x = 10

DIM a, b, c
a = 1
b = 2

' FALSCH (Syntaxfehler!):
' DIM x = 10

CONST PI = 3.1415
IMPORT betrag, rabatt
```

### Importierte Variablen (IMPORT)

Diese Variablen werden aus Work, Contract und CalendarSelection generiert:

| Variable | Typ | Beschreibung |
|----------|-----|--------------|
| hour | Decimal | Arbeitsstunden (aus Work) |
| fromhour | Decimal | Startzeit als Dezimalstunden (8:30 → 8.5) |
| untilhour | Decimal | Endzeit als Dezimalstunden (17:00 → 17.0) |
| weekday | Integer | Wochentag ISO-8601 (1=Mo, 2=Di, 3=Mi, 4=Do, 5=Fr, 6=Sa, 7=So) |
| holiday | Boolean | Ist aktueller Tag ein offizieller Feiertag |
| holidaynextday | Boolean | Ist Folgetag ein offizieller Feiertag |
| nightrate | Decimal | Nachtzuschlag-Satz aus Contract (z.B. 0.10 = 10%) |
| holidayrate | Decimal | Feiertagszuschlag-Satz aus Contract (z.B. 0.15 = 15%) |
| sarate | Decimal | **Sa**mstags-Zuschlag-Satz aus Contract (sa = Samstag/Saturday) |
| sorate | Decimal | **So**nntags-Zuschlag-Satz aus Contract (so = Sonntag/Sunday) |
| guaranteedhours | Decimal | Garantierte Monatsstunden aus Contract |
| fulltime | Decimal | Vollzeit-Stunden aus Contract |

### Weekday-Werte (ISO-8601)

| Wert | Tag |
|------|-----|
| 1 | Montag (Monday) |
| 2 | Dienstag (Tuesday) |
| 3 | Mittwoch (Wednesday) |
| 4 | Donnerstag (Thursday) |
| 5 | Freitag (Friday) |
| 6 | Samstag (Saturday) |
| 7 | Sonntag (Sunday) |

## Kontrollstrukturen

**Wichtig:** Schlüsselwörter wie `ENDIF`, `ENDFUNCTION`, `ENDSUB` müssen zusammengeschrieben werden.

### IF-THEN-ELSE

```basic
IF x > 10 THEN
    OUTPUT 1, "gross"
ELSEIF x > 5 THEN
    OUTPUT 1, "mittel"
ELSE
    OUTPUT 1, "klein"
ENDIF
```

### Einzeiliges IF

```basic
IF x > 10 THEN OUTPUT 1, "gross" ENDIF
```

### SELECT CASE

```basic
SELECT CASE weekday
    CASE 1, 7
        OUTPUT 1, "Wochenende"    ' So=1, Sa=7
    CASE 2, 3, 4, 5, 6
        OUTPUT 1, "Arbeitstag"    ' Mo-Fr
    CASE ELSE
        OUTPUT 1, "Unbekannt"
END SELECT
```

### FOR-NEXT Schleife

```basic
FOR i = 1 TO 10
    summe += i
NEXT

FOR i = 10 TO 1 STEP -1
    IF x > y THEN EXIT FOR
NEXT
```

### DO-LOOP Schleife

```basic
DO WHILE a > 0
    a -= 1
LOOP

DO
    x += 1
LOOP UNTIL x >= 10
```

## Funktionen und Prozeduren

**Wichtig:** Funktionen (FUNCTION) und Prozeduren (SUB) müssen *oberhalb* des ersten Aufrufs definiert werden.

### SUB (Prozedur ohne Rückgabewert)

```basic
SUB berechne(a, b)
    IF a = b THEN EXIT SUB ENDIF
    OUTPUT 1, a + b
ENDSUB
```

### FUNCTION (Funktion mit Rückgabewert)

```basic
FUNCTION verdoppeln(x)
    verdoppeln = x * 2
ENDFUNCTION

DIM ergebnis
ergebnis = verdoppeln(21)
OUTPUT 1, ergebnis
```

## Operatoren

### Mathematische Operatoren

| Operator | Beschreibung | Beispiel |
|----------|-------------|----------|
| + | Addition | 5 + 3 = 8 |
| - | Subtraktion | 5 - 3 = 2 |
| * | Multiplikation | 5 * 3 = 15 |
| / | Division | 10 / 4 = 2.5 |
| \ | Ganzzahldivision | 10 \ 4 = 2 |
| MOD | Modulo (Rest) | 10 MOD 3 = 1 |
| ^ | Potenz | 2 ^ 3 = 8 |

### Vergleichsoperatoren

| Operator | Beschreibung |
|----------|-------------|
| = | Gleich |
| <> | Ungleich |
| < | Kleiner als |
| > | Grösser als |
| <= | Kleiner oder gleich |
| >= | Grösser oder gleich |

### Logische Operatoren

| Operator | Beschreibung |
|----------|-------------|
| AND | Bitweises UND |
| OR | Bitweises ODER |
| NOT | Negation |
| ANDALSO | Logisches UND (Short-Circuit) |
| ORELSE | Logisches ODER (Short-Circuit) |

### Zuweisungsoperatoren

```basic
a = 10      ' Einfache Zuweisung
a += 5      ' a = a + 5
a -= 3      ' a = a - 3
a *= 2      ' a = a * 2
a /= 4      ' a = a / 4
a &= "!"    ' String-Verkettung
```

## Eingebaute Funktionen

### String-Funktionen

| Funktion | Beschreibung | Beispiel |
|----------|-------------|----------|
| Len(s) | Länge eines Strings | Len("Hallo") = 5 |
| Left(s, n) | Linke n Zeichen | Left("Hallo", 2) = "Ha" |
| Right(s, n) | Rechte n Zeichen | Right("Hallo", 2) = "lo" |
| Mid(s, start, len) | Teilstring | Mid("Hallo", 2, 3) = "all" |
| InStr(s, search) | Position suchen | InStr("Hallo", "l") = 3 |
| Replace(s, old, new) | Ersetzen | Replace("Hallo", "l", "x") = "Haxxo" |
| Trim(s) | Leerzeichen entfernen | Trim("  Hi  ") = "Hi" |
| UCase(s) | Grossbuchstaben | UCase("hallo") = "HALLO" |
| LCase(s) | Kleinbuchstaben | LCase("HALLO") = "hallo" |

### Mathematische Funktionen

| Funktion | Beschreibung | Beispiel |
|----------|-------------|----------|
| Abs(x) | Absolutwert | Abs(-5) = 5 |
| Round(x, d) | Runden | Round(3.456, 2) = 3.46 |
| Sqr(x) | Quadratwurzel | Sqr(16) = 4 |
| Log(x) | Natürlicher Logarithmus | Log(2.718) = 1 |
| Exp(x) | Exponential (e^x) | Exp(1) = 2.718 |
| Sgn(x) | Vorzeichen (-1, 0, 1) | Sgn(-5) = -1 |
| Rnd() | Zufallszahl 0-1 | Rnd() = 0.xxxxx |

### Zeit-Funktionen

| Funktion | Beschreibung | Beispiel |
|----------|-------------|----------|
| TimeToHours(s) | Zeit-String zu Dezimalstunden | TimeToHours("08:30") = 8.5 |
| TimeOverlap(s1, e1, s2, e2) | Überlappung zweier Zeiträume in Stunden | TimeOverlap("23:00", "06:00", "22:00", "07:00") = 7 |

*TimeOverlap unterstützt Zeiträume über Mitternacht (z.B. 23:00-06:00).*

### Bedingte Funktionen

| Funktion | Beschreibung | Beispiel |
|----------|-------------|----------|
| IIF(bed, wahr, falsch) | Bedingter Ausdruck | IIF(x > 0, "positiv", "negativ") |

## Rückgabe

```basic
OUTPUT typ, wert    ' Rückgabe an Klacks (typ entspricht MacroType)
```

## Debug-Funktionen

| Funktion | Beschreibung |
|----------|-------------|
| DEBUGPRINT wert | Gibt einen Wert im Test-Fenster aus |
| DEBUGCLEAR | Leert das Test-Fenster |

```basic
DIM x
x = 42
DEBUGPRINT "Der Wert ist: " & x
```

*Die Debug-Ausgaben erscheinen im Tab "Testen" des Macro-Editors.*

## Beispiel: Zuschlagsberechnung

Berechnet Zuschläge für Nacht, Feiertag und Wochenende mit korrekter Behandlung von Schichten über Mitternacht:

```basic
IMPORT hour, fromhour, untilhour
IMPORT weekday, holiday, holidaynextday
IMPORT nightrate, holidayrate, sarate, sorate

FUNCTION CalcSegment(StartHour, EndHour, IsHoliday, WeekdayNum)
    DIM SegmentHours, NightHours, NonNightHours
    DIM NRate, DRate, HasHoliday, HasSaturday, HasSunday

    SegmentHours = EndHour - StartHour
    IF SegmentHours < 0 THEN SegmentHours = SegmentHours + 24 ENDIF

    ' Berechne Nachtstunden (23:00-06:00)
    NightHours = 0
    IF StartHour < 6 THEN NightHours = NightHours + IIF(EndHour < 6, EndHour, 6) - StartHour ENDIF
    IF EndHour > 23 OrElse EndHour < StartHour THEN NightHours = NightHours + IIF(StartHour > 23, 24 - StartHour, 24 - 23) ENDIF
    NonNightHours = SegmentHours - NightHours

    HasHoliday = IsHoliday
    HasSaturday = WeekdayNum = 6    ' Sa = 6 (ISO-8601)
    HasSunday = WeekdayNum = 7      ' So = 7 (ISO-8601)

    ' Höchster Satz für Nachtstunden
    NRate = nightrate
    IF HasHoliday AndAlso holidayrate > NRate THEN NRate = holidayrate ENDIF
    IF HasSaturday AndAlso sarate > NRate THEN NRate = sarate ENDIF
    IF HasSunday AndAlso sorate > NRate THEN NRate = sorate ENDIF

    ' Höchster Satz für Tagstunden
    DRate = 0
    IF HasHoliday AndAlso holidayrate > DRate THEN DRate = holidayrate ENDIF
    IF HasSaturday AndAlso sarate > DRate THEN DRate = sarate ENDIF
    IF HasSunday AndAlso sorate > DRate THEN DRate = sorate ENDIF

    CalcSegment = NightHours * NRate + NonNightHours * DRate
ENDFUNCTION

DIM TotalBonus, WeekdayNextDay

' Berechne Wochentag des Folgetags (VBA-Format)
WeekdayNextDay = (weekday MOD 7) + 1

IF untilhour <= fromhour THEN
    ' Schicht über Mitternacht - 2 Segmente
    TotalBonus = CalcSegment(fromhour, 24, holiday, weekday)
    TotalBonus = TotalBonus + CalcSegment(0, untilhour, holidaynextday, WeekdayNextDay)
ELSE
    ' Normale Tagesschicht
    TotalBonus = CalcSegment(fromhour, untilhour, holiday, weekday)
ENDIF

OUTPUT 1, Round(TotalBonus, 2)
```

**Erklärung:**
- **Segment-Splitting:** Bei Schichten über Mitternacht wird die Schicht in zwei Segmente aufgeteilt
- **CalcSegment:** Berechnet den Zuschlag unter Berücksichtigung von Nacht-, Feiertags- und Wochenendzuschlägen
- **Separate Sa/So-Sätze:** sarate und sorate ermöglichen unterschiedliche Zuschläge für Samstag und Sonntag
- **Höchster Zuschlag:** Es wird immer der höchste anwendbare Zuschlag verwendet
- **holidaynextday:** Berücksichtigt ob der Folgetag ein Feiertag ist (wichtig für Nachtschichten)

## Speicherung der Ergebnisse

Das Macro-Ergebnis wird in `Work.Surcharges` gespeichert und bei der PeriodHours-Berechnung berücksichtigt:

```
Work erstellen
    │
    ▼
WorkMacroService.ProcessWorkMacroAsync()
    │ Macro ausführen
    ▼
work.Surcharges = macroResult
    │
    ▼
PeriodHoursService.RecalculateAndNotifyAsync()
    │ Summiert Work.Surcharges + WorkChange.ChangeTime
    ▼
ClientPeriodHours.Surcharges speichern
    │
    ▼
Frontend: Row-Header Slot 3 anzeigen
```

---

## Changelog

### 22.01.2026 - Surcharges Berechnung Fix

Das Macro-Ergebnis (`Work.Surcharges`) wird jetzt korrekt in der PeriodHours-Berechnung berücksichtigt:

```csharp
// PeriodHoursService.cs + WorkRepository.cs
TotalSurcharges = g.Sum(w => w.Surcharges)  // Summe aller Macro-Ergebnisse
Surcharges = workData.Surcharges + workChangeSurcharges  // + manuelle Korrekturen
```
