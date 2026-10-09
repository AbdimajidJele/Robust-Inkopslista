// One item on the shopping list.
class Item
{
    public string Name { get; } // Ändrat från { get; set; }, namnet kan bara sättas i konstruktorn
    public int Price { get; } // Ändrat från { get; set; }, annars kunde priset bli negativt efteråt

    public Item(string name, int price)
    {
        if (string.IsNullOrWhiteSpace(name)) // Fångar både tomt namn och bara mellanslag
        {
            throw new ArgumentException("Namnet får inte vara tomt."); // Objektet skapas aldrig
        }

        if (price < 0) // Ett negativt pris är ogiltigt
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Priset får inte vara negativt."); // Objektet skapas aldrig
        }

        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} - {Price} kr";
    }
}