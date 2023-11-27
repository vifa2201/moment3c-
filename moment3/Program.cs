using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace GuestBook
{
    // Klass för gästbokens info
    public class GuestEntry
    {
        //fält för klassen
        public string Owner { get; set; }
        public string Message { get; set; }
    }

    // Programklass
    class Program
    {
        // Statisk lista med gästboksinlägg
        private static List<GuestEntry> guestBookEntries = new List<GuestEntry>();

        // Sökväg till filen där inläggen sparas
        private static string filePath = "guestbook.json";

        static void Main(string[] args)
        {
            // Ladda tidigare sparade inlägg från filen
            LoadEntries();

            while (true)
            {
                Console.Clear(); // Rensa konsolen
                // Alternativ i konsolen
                Console.WriteLine("1. Lägg till inlägg");
                Console.WriteLine("2. Visa alla inlägg");
                Console.WriteLine("3. Avsluta");
                Console.WriteLine("4. Radera inlägg");
                Console.Write("Välj ett alternativ: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddEntry();

                        break;

                    case "2":
                        ShowAllEntries();
                        break;

                    case "3":
                        // Spara inlägg och avsluta
                        SaveEntries();
                        return;

                    case "4":
                        //ta bort inlägget
                        RemoveEntry();
                        break;

                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }

        private static void AddEntry()
        {
            Console.WriteLine("Ange ägare: ");
            string owner = Console.ReadLine();

            Console.WriteLine("Ange meddelande");
            string message = Console.ReadLine();

            // Om ägare och meddelande inte är tomma:
            if (!string.IsNullOrWhiteSpace(owner) && !string.IsNullOrWhiteSpace(message))
            {
                GuestEntry entry = new GuestEntry { Owner = owner, Message = message };
                guestBookEntries.Add(entry);
                Console.WriteLine("Inlägg tillagt!");
                //sparar inlägg
                   SaveEntries();

            }
            else
            { //om man angett tomt på ägare eller meddeladne
                Console.WriteLine("Felaktig inmatning. Både ägare och meddelande måste anges.");
            }

        //för att fortsätta till menyn
            if (!Console.IsInputRedirected && Console.KeyAvailable)
            {
                Console.ReadKey(true);
                  Console.WriteLine("Tryck valfri tangent för att fortsätta");
            }
        }

    //visar alla inlägg
      private static void ShowAllEntries()
{
    // om det finns inlägg 
    if (guestBookEntries.Count > 0)
    {
        for (int i = 0; i < guestBookEntries.Count; i++)
        {
            // skriver ut inläggen med index
            Console.WriteLine($"{i}. Ägare: {guestBookEntries[i].Owner}, Meddelande: {guestBookEntries[i].Message}");
        }
    }
    else
    {
        //om inga inlägg finns
        Console.WriteLine("Gästboken är tom");
    }

    Console.WriteLine("Tryck på valfri tangent för att fortsätta");
    Console.In.ReadLineAsync().GetAwaiter().GetResult();
}
        private static void SaveEntries()
        {
            // Serialisera och spara inläggen till filen
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<GuestEntry>));
                serializer.WriteObject(fs, guestBookEntries);
            }
        }

        private static void LoadEntries()
        {
            // Försök läsa inläggen från filen
            try
            {
                if (File.Exists(filePath))
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<GuestEntry>));
                        guestBookEntries = (List<GuestEntry>)serializer.ReadObject(fs);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid inläsning av inlägg: {ex.Message}");
            }
        }

 private static void RemoveEntry()
{
    Console.WriteLine("Välj index för inlägget du vill ta bort:");
 

    if (int.TryParse(Console.ReadLine(), out int selectedIndex) && selectedIndex >= 0 && selectedIndex < guestBookEntries.Count)
    {
        Console.WriteLine($"Ägare: {guestBookEntries[selectedIndex].Owner}, Meddelande: {guestBookEntries[selectedIndex].Message}");
        Console.WriteLine("Är du säker på att du vill ta bort detta inlägg? (ja/nej)");

        if (Console.ReadLine().ToLower() == "ja")
        {
            guestBookEntries.RemoveAt(selectedIndex);
            Console.WriteLine("Inlägg borttaget!");
        }
        else
        {
            Console.WriteLine("Inlägg inte borttaget.");
        }
    }
    else
    {
        Console.WriteLine("Ogiltigt index. Försök igen.");
    }

    Console.WriteLine("Tryck på valfri tangent för att fortsätta");
    Console.In.ReadLineAsync().GetAwaiter().GetResult();
}
    }
}
