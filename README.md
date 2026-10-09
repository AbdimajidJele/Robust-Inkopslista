# Robust inköpslista

Ett konsolprogram i C# där man kan lägga till, ta bort och söka varor. Listan sparas i `items.txt` och har ett budgettak på 500 kr.

## Starta

Öppna terminalen i projektmappen och skriv:

```bash
dotnet run
```

## Filen items.txt

En vara per rad, skrivet som `pris;namn`:

```
15;Mjölk
32;Bröd
89;Ost
```

Om filen saknas börjar programmet med en tom lista. Rader som är fel (tom rad, pris som inte är ett tal, tomt namn eller negativt pris) hoppas över.

## Felrapport

**Fel 1: krasch när items.txt saknas.** `Load()` läste filen utan att kolla om den fanns, så programmet kraschade med `FileNotFoundException`. Nu kollar jag med `File.Exists` först och startar med en tom lista om filen saknas.

**Fel 2: krasch vid start, och sökningen hittade inte varan.** Filen slutar med en radbrytning, så sista raden blev tom och `parts[1]` fanns inte (`IndexOutOfRangeException`). Dessutom skrev `Save()` `\r\n` men `Load()` delade bara på `\n`, så `\r` hamnade i namnet och sökningen matchade inte. Nu använder jag `File.ReadAllLines`, hoppar över rader med färre än två delar och använder `int.TryParse` för priset.

**Fel 3: krasch vid felaktig inmatning.** `Program.cs` använde `int.Parse`, som kastar `FormatException` om man skriver bokstäver. Jag bytte till `int.TryParse`. Om det inte är ett tal skrivs ett meddelande och menyn visas igen.

**Fel 4: krasch när man tar bort en vara som inte finns.** `RemoveAt` kontrollerade inte numret, så t.ex. 0 eller 99 gav `ArgumentOutOfRangeException`. Nu kontrollerar jag att numret är mellan 1 och antalet varor. Metoden returnerar `bool`, och `Program.cs` skriver ett meddelande om det blev `false`.

**Fel 5: fel totalsumma.** Loopen i `Total()` började på `i = 1`, så första varan räknades inte med. Jag fick 121 kr i stället för 136 kr. Nu börjar loopen på `i = 0`.

**Fel 6: programmet dolde att sparningen misslyckades.** `Save()` hade en tom `catch`, och "Listan är sparad." skrevs även när det gick fel. Jag flyttade meddelandet in i `try` så att det bara visas om sparningen lyckades. Den tomma `catch` är ersatt av `catch (UnauthorizedAccessException)` och `catch (IOException)`, som skriver ett felmeddelande.

## Designval

När budgettaket skulle överskridas valde jag att `Add` returnerar `false`. Annars returnerar den `true`.

Jag valde det för att ett fullt tak är något som händer ofta och inget programmeringsfel. `Program.cs` behöver bara en `if` för att berätta för användaren vad som hände, och programmet fortsätter köra.

Ogiltiga värden i `Item` (tomt namn eller negativt pris) är däremot fel från den som anropar. Där kastar konstruktorn `ArgumentException` och `ArgumentOutOfRangeException`, så att ett trasigt objekt aldrig skapas. `Program.cs` fångar dem och visar ett meddelande.

## Klassdiagram

```
+-------------------+     +--------------------------+     +----------------+
| Item              |     | ShoppingList             |     | Program        |
|-------------------|     |--------------------------|     |----------------|
| Name              |<----| items, path, budget      |<----| menyn          |
| Price             |     | Add(): bool              |     | fångar fel     |
| Item(name, price) |     | RemoveAt(): bool         |     | från Item      |
|                   |     | Total(), Find(), Print() |     |                |
|                   |     | Save(), Load()           |     |                |
+-------------------+     +--------------------------+     +----------------+
```