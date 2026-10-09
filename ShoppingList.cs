// Holds the items and takes care of loading and saving them.
class ShoppingList
{
    private List<Item> items = new List<Item>();
    private string path;

    public ShoppingList(string path)
    {
        this.path = path;
    }

    public void Add(Item item)
    {
        items.Add(item);
    }

    // Returnerar true om varan togs bort, false om numret inte finns på listan
    public bool RemoveAt(int number)
    {
        // Giltiga nummer är 1 till antalet varor
        if (number < 1 || number > items.Count)
        {
            return false;
        }

        items.RemoveAt(number - 1);
        return true;
    }

    // Adds up the price of every item on the list.
    public int Total()
    {
        int sum = 0;

        for (int i = 0; i < items.Count; i++) // Index börjar på 0, annars hoppas första varan över.
        {
            sum += items[i].Price;
        }

        return sum;
    }

    // Looks up an item by its name. Returns null if there is no such item.
    public Item Find(string name)
    {
        foreach (Item item in items)
        {
            if (item.Name == name)
            {
                return item;
            }
        }

        return null;
    }

    public void Print()
    {
        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {items[i]}");
        }

        Console.WriteLine($"Totalt: {Total()} kr");
    }

    // Writes one item per line, as "price;name".
    public void Save()
    {
        List<string> lines = new List<string>();

        foreach (Item item in items)
        {
            lines.Add($"{item.Price};{item.Name}");
        }
        try
        {
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n");

            // Meddelandet står i try så det bara skrivs om sparningen lyckades
            Console.WriteLine("Listan är sparad.");
        }
        catch (UnauthorizedAccessException)
        {
            // Filen är skrivskyddad eller vi saknar behörighet
            Console.WriteLine("Kunde inte spara: du har inte behörighet att skriva till filen.");
        }
        catch (IOException)
        {
            // Till exempel att mappen saknas eller att filen används av något annat
            Console.WriteLine("Kunde inte spara listan: filen gick inte att skriva.");
        }

    }

    // Reads the file back into the list.
    public void Load()
    {
        if (!File.Exists(path)) return; // Om filen inte finns börjar vi med en tom lista i stället för att krascha.
        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            string[] parts = line.Split(';');
            if (parts.Length < 2) continue;
            if (!int.TryParse(parts[0], out int price)) continue; // Om priset inte är ett tal hoppar vi över raden i stället för att krascha
            items.Add(new Item(parts[1], price));
        }
    }
}