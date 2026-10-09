## Felrapport

### Fel 1: krasch när items.txt saknas
**Vad hände:** Programmet kraschade direkt vid start med `FileNotFoundException`.
**Varför:** `Load()` läste filen utan att kolla att den fanns. Filen saknas första gången programmet körs.
**Lösning:** Jag kontrollerar med `File.Exists` först i `Load()`. Om filen saknas börjar programmet med en tom lista.

### Fel 2: krasch vid start och sökning hittade inte varan
**Vad hände:** Programmet kraschade med `IndexOutOfRangeException`. Dessutom hittades inte varor vid sökning.
**Varför:** Filen slutar med en radbrytning, så sista raden är tom och `parts[1]` finns inte. `Save()` skrev `\r\n` men `Load()` delade bara på `\n`, så `\r` hängde kvar i namnet ("Mjölk\r") och sökningen matchade inte. `int.Parse` kunde också krascha på ett ogiltigt pris.
**Lösning:** Jag använder `File.ReadAllLines`, som hanterar båda radbrytningarna. Jag hoppar över rader med färre än två delar och använder `int.TryParse` för priset.

### Fel 3: krasch vid felaktig inmatning
**Vad hände:** Om jag skrev bokstäver vid menyval, pris eller nummer kraschade programmet med `FormatException`.
**Varför:** `Program.cs` använde `int.Parse`, som kastar undantag när texten inte är ett heltal.
**Lösning:** Jag bytte till `int.TryParse`. Vid `false` skriver programmet ett meddelande och går tillbaka till menyn.

### Fel 4: krasch när man tar bort en vara som inte finns
**Vad hände:** Om jag skrev ett nummer utanför listan, till exempel 0 eller 99, kraschade programmet med `ArgumentOutOfRangeException`.
**Varför:** `RemoveAt` anropade `items.RemoveAt(number - 1)` utan att kontrollera numret.
**Lösning:** Jag kontrollerar att numret är mellan 1 och `items.Count`. Metoden returnerar nu `bool`, och `Program.cs` skriver ett meddelande om det blev `false`.

### Fel 5: fel totalsumma
**Vad hände:** Programmet visade 121 kr i stället för 136 kr, utan att krascha.
**Varför:** Loopen i `Total()` började på `i = 1`, så första varan på listan räknades aldrig med.
**Lösning:** Jag ändrade loopen så att den börjar på `i = 0`.

### Fel 6: programmet dolde att sparningen misslyckades
**Vad hände:** Programmet skrev "Listan är sparad." även när sparningen misslyckades.
**Varför:** `Save()` hade en tom `catch` som svalde felet, och meddelandet låg utanför `try`.
**Lösning:** Jag flyttade meddelandet "Listan är sparad." in i `try`, direkt efter `WriteAllText`, så att det bara skrivs om sparningen lyckades. Jag ersatte den tomma `catch` med `catch (UnauthorizedAccessException)` och `catch (IOException)`. Båda skriver ett felmeddelande, så användaren får veta att sparningen misslyckades.