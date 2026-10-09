ShoppingList list = new ShoppingList("items.txt", 500); // 500 kr är budgettaketlist.Load();
list.Load();
while (true)
{
    Console.WriteLine();
    list.Print();
    Console.WriteLine();
    Console.WriteLine("1. Lägg till vara");
    Console.WriteLine("2. Ta bort vara");
    Console.WriteLine("3. Spara");
    Console.WriteLine("4. Sök vara");
    Console.WriteLine("5. Avsluta");
    Console.Write("Välj: ");

    // Om användaren inte skriver ett heltal visar vi menyn igen i stället för att krascha
    if (!int.TryParse(Console.ReadLine(), out int choice))
    {
        Console.WriteLine("Skriv en siffra mellan 1 och 5.");
        continue;
    }

    if (choice == 1)
    {
        Console.Write("Namn: ");
        string name = Console.ReadLine();
        Console.Write("Pris: ");

        // Priset måste vara ett heltal, annars går vi tillbaka till menyn
        if (!int.TryParse(Console.ReadLine(), out int price))
        {
            Console.WriteLine("Priset måste vara ett heltal.");
            continue;
        }
        list.Add(new Item(name, price));
    }
       else if (choice == 2)
    {
        Console.Write("Nummer: ");

        // Numret måste vara ett heltal, annars går vi tillbaka till menyn
        if (!int.TryParse(Console.ReadLine(), out int number))
        {
            Console.WriteLine("Numret måste vara ett heltal.");
            continue;
        }

        // Om numret inte finns berättar vi det för användaren
        if (!list.RemoveAt(number))
        {
            Console.WriteLine("Det finns ingen vara med det numret.");
        }
    }
    else if (choice == 3)
    {
        list.Save();
    }
    else if (choice == 4)
    {
        Console.Write("Namn att söka efter: ");
        string wanted = Console.ReadLine();
        Item found = list.Find(wanted);

        if (found == null)
        {
            Console.WriteLine("Varan finns inte i listan.");
        }
        else
        {
            Console.WriteLine($"Hittade: {found}");
        }
    }
    else if (choice == 5)
    {
        break;
    }
}