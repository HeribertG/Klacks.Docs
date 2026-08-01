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
| fromhour | String ("HH:MM") | Startzeit, z.B. "08:30" – mit `TimeToHours()` in Dezimalstunden umwandelbar |
| untilhour | String ("HH:MM") | Endzeit, z.B. "17:00" |
| weekday | Integer | Wochentag ISO-8601 (1=Mo, 2=Di, 3=Mi, 4=Do, 5=Fr, 6=Sa, 7=So) |
| holiday | Boolean | Ist aktueller Tag ein offizieller Feiertag |
| holidaynextday | Boolean | Ist Folgetag ein offizieller Feiertag |
| nightrate | Decimal | Nachtzuschlag-Satz aus Contract (z.B. 0.10 = 10%) |
| holidayrate | Decimal | Feiertagszuschlag-Satz aus Contract (z.B. 0.15 = 15%) |
| we1rate | Decimal | Zuschlag-Satz für konfigurierbaren Wochenendtag 1 (z.B. Samstag) |
| we2rate | Decimal | Zuschlag-Satz für konfigurierbaren Wochenendtag 2 (z.B. Sonntag) |
| we3rate | Decimal | Zuschlag-Satz für konfigurierbaren Wochenendtag 3 |
| nightstart | String ("HH:MM") | Beginn des Nachtfensters aus Contract/Settings, z.B. "23:00" |
| nightend | String ("HH:MM") | Ende des Nachtfensters, z.B. "06:00" |
| guaranteedhours | Decimal | Garantierte Monatsstunden aus Contract |
| fulltime | Decimal | Vollzeit-Stunden aus Contract |
| weekendday1 | Integer | ISO-Wochentag des 1. konfigurierten Wochenendtags (0 = nicht konfiguriert) |
| weekendday2 | Integer | ISO-Wochentag des 2. konfigurierten Wochenendtags |
| weekendday3 | Integer | ISO-Wochentag des 3. konfigurierten Wochenendtags |

*Hinweis: die früheren Variablen `sarate`/`sorate` (fixer Samstag/Sonntag-Satz) wurden durch die drei
länderneutralen `we1rate`/`we2rate`/`we3rate` ersetzt, die zusammen mit `weekendday1-3` beliebige
Wochenend-Tage abbilden (z.B. Freitag/Samstag statt Samstag/Sonntag).*

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
    CASE 6, 7
        OUTPUT 1, "Wochenende"    ' Sa=6, So=7 (Standard-Wochenende)
    CASE 1, 2, 3, 4, 5
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

`typ` und `wert` sind zwei durch Komma getrennte Ausdrücke; beide sind optional (Default `0` bzw. `""`,
wenn weggelassen). Ein Macro darf beliebig viele `OUTPUT`-Zeilen enthalten – jede erzeugt einen eigenen
Rückgabewert. Klacks wertet dabei nur die folgenden Typen aus:

| Typ | Bedeutung |
|-----|-----------|
| 1 | Hauptergebnis (Stunden bzw. Gesamtbetrag) – wird auch bei Wert 0 übernommen |
| 10 | Nachtzuschlag |
| 11 | Zuschlag für Wochenendtag 1 (z.B. Samstag) |
| 12 | Zuschlag für Wochenendtag 2 (z.B. Sonntag) |
| 13 | Zuschlag für Wochenendtag 3 (dritter konfigurierbarer Wochenendtag) |
| 14 | Feiertagszuschlag |

Zuschlags-Zeilen (Typ 10-14) mit Wert `0` werden ignoriert — ein Macro muss also nicht für jeden
Zuschlagstyp eine eigene Zeile ausgeben, sondern nur für tatsächlich anfallende Zuschläge. Werte, die
sich nicht als Zahl interpretieren lassen, werden übersprungen.

*Beim Speichern wird das Macro zwingend kompiliert und einmal probeweise mit neutralen Testwerten
ausgeführt. Syntax- und Laufzeitfehler werden dabei zuverlässig angezeigt statt stillschweigend zu
einem funktionslosen Macro zu führen.*

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

## Beispiel: Zuschlagsberechnung ("AllShift")

Das mitgelieferte Standard-Macro der Kategorie "Dienst". Berechnet Zuschläge für Nacht, Feiertag und die
drei konfigurierbaren Wochenendtage mit korrekter Behandlung von Schichten über Mitternacht
(Segment-Splitting) und wendet pro Segment den *höchsten* anwendbaren Satz an (highest-wins):

```basic
IMPORT Hour, FromHour, UntilHour
IMPORT Weekday, Holiday, HolidayNextDay
IMPORT NightRate, HolidayRate, WE1Rate, WE2Rate, WE3Rate
IMPORT NightStart, NightEnd
IMPORT WeekendDay1, WeekendDay2, WeekendDay3

FUNCTION SegBonusForType(StartTime, EndTime, HolidayFlag, WeekdayNum, WantType)
    DIM SegmentHours, NightHours, NonNightHours, Amount
    DIM NRate, DRate, NType, DType
    DIM HasHoliday, IsWE1, IsWE2, IsWE3

    SegmentHours = TimeToHours(EndTime) - TimeToHours(StartTime)
    IF SegmentHours < 0 THEN SegmentHours = SegmentHours + 24 ENDIF

    NightHours = TimeOverlap(NightStart, NightEnd, StartTime, EndTime)
    NonNightHours = SegmentHours - NightHours

    HasHoliday = HolidayFlag = 1
    IsWE1 = WeekdayNum = WeekendDay1
    IsWE2 = WeekdayNum = WeekendDay2
    IsWE3 = WeekdayNum = WeekendDay3

    NRate = 0
    NType = 0
    IF NightHours > 0 THEN
        NRate = NightRate
        NType = 10
    ENDIF
    IF HasHoliday AndAlso HolidayRate > NRate THEN
        NRate = HolidayRate
        NType = 14
    ENDIF
    IF IsWE1 AndAlso WE1Rate > NRate THEN
        NRate = WE1Rate
        NType = 11
    ENDIF
    IF IsWE2 AndAlso WE2Rate > NRate THEN
        NRate = WE2Rate
        NType = 12
    ENDIF
    IF IsWE3 AndAlso WE3Rate > NRate THEN
        NRate = WE3Rate
        NType = 13
    ENDIF

    DRate = 0
    DType = 0
    IF HasHoliday AndAlso HolidayRate > DRate THEN
        DRate = HolidayRate
        DType = 14
    ENDIF
    IF IsWE1 AndAlso WE1Rate > DRate THEN
        DRate = WE1Rate
        DType = 11
    ENDIF
    IF IsWE2 AndAlso WE2Rate > DRate THEN
        DRate = WE2Rate
        DType = 12
    ENDIF
    IF IsWE3 AndAlso WE3Rate > DRate THEN
        DRate = WE3Rate
        DType = 13
    ENDIF

    Amount = 0
    IF NType = WantType THEN Amount = Amount + NightHours * NRate ENDIF
    IF DType = WantType THEN Amount = Amount + NonNightHours * DRate ENDIF

    SegBonusForType = Amount
ENDFUNCTION

DIM TotalBonus, WeekdayNextDay
DIM BonusNight, BonusWeekend1, BonusWeekend2, BonusWeekend3, BonusHoliday

WeekdayNextDay = (Weekday MOD 7) + 1

IF TimeToHours(UntilHour) <= TimeToHours(FromHour) THEN
    BonusNight = SegBonusForType(FromHour, "00:00", Holiday, Weekday, 10) + SegBonusForType("00:00", UntilHour, HolidayNextDay, WeekdayNextDay, 10)
    BonusWeekend1 = SegBonusForType(FromHour, "00:00", Holiday, Weekday, 11) + SegBonusForType("00:00", UntilHour, HolidayNextDay, WeekdayNextDay, 11)
    BonusWeekend2 = SegBonusForType(FromHour, "00:00", Holiday, Weekday, 12) + SegBonusForType("00:00", UntilHour, HolidayNextDay, WeekdayNextDay, 12)
    BonusWeekend3 = SegBonusForType(FromHour, "00:00", Holiday, Weekday, 13) + SegBonusForType("00:00", UntilHour, HolidayNextDay, WeekdayNextDay, 13)
    BonusHoliday = SegBonusForType(FromHour, "00:00", Holiday, Weekday, 14) + SegBonusForType("00:00", UntilHour, HolidayNextDay, WeekdayNextDay, 14)
ELSE
    BonusNight = SegBonusForType(FromHour, UntilHour, Holiday, Weekday, 10)
    BonusWeekend1 = SegBonusForType(FromHour, UntilHour, Holiday, Weekday, 11)
    BonusWeekend2 = SegBonusForType(FromHour, UntilHour, Holiday, Weekday, 12)
    BonusWeekend3 = SegBonusForType(FromHour, UntilHour, Holiday, Weekday, 13)
    BonusHoliday = SegBonusForType(FromHour, UntilHour, Holiday, Weekday, 14)
ENDIF

TotalBonus = BonusNight + BonusWeekend1 + BonusWeekend2 + BonusWeekend3 + BonusHoliday

OUTPUT 1, Round(TotalBonus, 2)
OUTPUT 10, BonusNight
OUTPUT 11, BonusWeekend1
OUTPUT 12, BonusWeekend2
OUTPUT 13, BonusWeekend3
OUTPUT 14, BonusHoliday
```

**Erklärung:**
- **Segment-Splitting:** Bei Schichten über Mitternacht (z.B. 22:00-06:00) wird die Schicht in zwei Segmente aufgeteilt (bis Mitternacht / ab Mitternacht), jedes mit eigenem Wochentag/Feiertags-Flag
- **SegBonusForType:** Berechnet den Zuschlag für ein Segment und einen bestimmten Zuschlagstyp (10-14) unter Berücksichtigung von Nacht-, Feiertags- und Wochenendzuschlägen
- **WE1/WE2/WE3 statt Sa/So:** `weekendday1-3` machen die Wochenend-Tage konfigurierbar (nicht jedes Land hat Samstag/Sonntag als Wochenende)
- **Höchster Zuschlag pro Segment:** `NRate`/`DRate` ermitteln jeweils den höchsten anwendbaren Satz für Nacht- bzw. Tagstunden — highest-wins, kein Stacking
- **HolidayNextDay:** Berücksichtigt, ob der Folgetag ein Feiertag ist (wichtig für Nachtschichten über Mitternacht)
- **Mehrere OUTPUT-Zeilen:** Zeile 1 liefert die Gesamtsumme, Zeilen 10-14 liefern die einzelnen Zuschlagsbeträge (siehe Abschnitt "Rückgabe")

Dies ist das tatsächliche, produktiv hinterlegte Zuschlagsmakro "AllShift" (Migration `AddAllShiftAdditiveMacro`, 2026-07-15).

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

### 01.08.2026 - Dokument mit produktivem Zuschlagsmacro synchronisiert

Import-Tabelle, Rückgabe-Kanäle und das Beispiel-Macro waren auf dem Stand von `SaRate`/`SoRate`
(fixer Samstag/Sonntag-Satz, Migration `GeneralizeWeekendMacrosToConfigurableDays`, 2026-07-06)
eingefroren, obwohl das Backend seither nur noch `WE1Rate`/`WE2Rate`/`WE3Rate` kennt. Beispiel jetzt
1:1 das real hinterlegte "AllShift"-Macro (Migration `AddAllShiftAdditiveMacro`, 2026-07-15); zusätzlich
`fromhour`/`untilhour` korrekt als String statt Decimal dokumentiert, `nightstart`/`nightend` und
`weekendday1-3` ergänzt.

### 22.01.2026 - Surcharges Berechnung Fix

Das Macro-Ergebnis (`Work.Surcharges`) wird jetzt korrekt in der PeriodHours-Berechnung berücksichtigt:

```csharp
// PeriodHoursService.cs + WorkRepository.cs
TotalSurcharges = g.Sum(w => w.Surcharges)  // Summe aller Macro-Ergebnisse
Surcharges = workData.Surcharges + workChangeSurcharges  // + manuelle Korrekturen
```
