using System;
using System.Collections.Generic;
using System.IO;

namespace BookStuff
{
    class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public bool IsBorrowed { get; set; }

        public Book(string name, string author, bool isBorrowed = false)
        {
            Name = name;
            Author = author;
            IsBorrowed = isBorrowed;
        }
    }

    class Program
    {
        static List<Book> books = new List<Book>();
        static string fileName = "books.txt";

        static void Main()
        {
            string asciiArt = @"
 _       _                     ______  _ _     _ _                 _     
| |     (_)                   (____  \(_) |   | (_)      _        | |    
| |      _ ____  _   _  ___    ____)  )_| | _ | |_  ___ | |_  ____| |  _ 
| |     | |  _ \| | | |/___)  |  __  (| | || \| | |/ _ \|  _)/ _  ) | / )
| |_____| | | | | |_| |___ |  | |__)  ) | |_) ) | | |_| | |_( (/ /| |< ( 
|_______)_|_| |_|\____(___/   |______/|_|____/|_|_|\___/ \___)____)_| \_)                                        
";

            Console.WriteLine(asciiArt);   //printar ascii konsten (Linus Bibliotek)
            LoadBooks(); //läser in böckerna

            bool running = true;
            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1": ListBooks(); break; //nämner alla böcker i biblioteket
                    case "2": AddBook(); break; //lägger till en bok i biblioteket
                    case "3": FindBook(); break; //söker efter boken
                    case "4": BorrowBook(); break; //lånar boken
                    case "5": ReturnBook(); break; //lämnar tillbaka boken
                    case "6": DeleteBook(); break; //tar bort boken helt
                    case "7": running = false; Console.WriteLine("Ha det bra!"); return; //om personen väljer att stänga av programmet
                    default: Console.WriteLine("Du måste svara med ett av alternativen!"); break;
                }
                Console.WriteLine("\nTryck på en tangent för att fortsätta...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void LoadBooks()
        {
            if (File.Exists(fileName))
            {
                foreach (string line in File.ReadAllLines(fileName))
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                        books.Add(new Book(parts[0], parts[1], parts[2] == "yes"));
                }
            }
            else
            {
                books.AddRange(new List<Book> //Alla böcker som orginellt fanns i biblioteket
                {
                    new Book("Ninja Timmy", "Henrik Tamm"),
                    new Book("Amulet", "Kazu Kibuishi"),
                    new Book("Tokyo Ghoul", "Sui Ishida"),
                    new Book("Pax", "Åsa Larsson"),
                    new Book("The Art of War", "Sun Zi"),
                    new Book("Hunger Games", "Suzanne Collins"),
                    new Book("Maze Runner", "James Dashner"),
                    new Book("Bibeln", "35 olika författare"),
                    new Book("The Great Gatsby", "F. Scott Fitzgerald"),
                    new Book("The Lord of the Rings", "J.R.R Tolkiens"),
                    new Book("Lord of the Flies", "William Golding")
                });
                SaveBooks(); //Sparar böckerna i biblioteket
            }
        }

        static void SaveBooks()
        {
            List<string> lines = new List<string>();
            books.ForEach(b => lines.Add($"{b.Name},{b.Author},{(b.IsBorrowed ? "yes" : "no")}"));
            File.WriteAllLines(fileName, lines);
        }

        static void ShowMenu()
        {
            Console.WriteLine("\nALTERNATIV:");
            Console.WriteLine("1. Visa alla böcker");
            Console.WriteLine("2. Lägg till bok");
            Console.WriteLine("3. Sök efter bok");
            Console.WriteLine("4. Låna bok");
            Console.WriteLine("5. Lämna tillbaka bok");
            Console.WriteLine("6. Ta bort bok");
            Console.WriteLine("7. Avsluta");
            Console.Write("Välj: ");
        } //printa alla alternativ

        static void ListBooks()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Inga böcker hittades!");
                return;
            }
            for (int i = 0; i < books.Count; i++)
                Console.WriteLine($"{i + 1}. {books[i].Author} - {books[i].Name} {(books[i].IsBorrowed ? "(Utlånad)" : "")}");
        }

        static void AddBook()
        {
            Console.Write("Namn: ");
            string name = Console.ReadLine();
            Console.Write("Författare: ");
            string author = Console.ReadLine();
            books.Add(new Book(name, author));
            SaveBooks();
            Console.WriteLine("Bok tillagd!");
        }

        static void FindBook()
        {
            Console.Write("Sök: ");
            string search = Console.ReadLine().ToLower();
            bool found = false;

            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Name.ToLower().Contains(search) || books[i].Author.ToLower().Contains(search))
                {
                    Console.WriteLine($"{i + 1}. {books[i].Author} - {books[i].Name} {(books[i].IsBorrowed ? "(Utlånad)" : "")}");
                    found = true;
                }
            }
            if (!found) Console.WriteLine("Inga träffar!");
        }

        static void BorrowBook()
        {
            ListBooks();
            Console.Write("Boknummer att låna: ");
            if (int.TryParse(Console.ReadLine(), out int num) && num > 0 && num <= books.Count)
            {
                if (books[num - 1].IsBorrowed)
                    Console.WriteLine("Denna bok är redan utlånad!");
                else
                {
                    books[num - 1].IsBorrowed = true;
                    SaveBooks();
                    Console.WriteLine("Bok lånad!");
                }
            }
            else Console.WriteLine("Ogiltigt nummer!");
        }

        static void ReturnBook()
        {
            ListBooks();
            Console.Write("Boknummer att lämna tillbaka: ");
            if (int.TryParse(Console.ReadLine(), out int num) && num > 0 && num <= books.Count)
            {
                if (!books[num - 1].IsBorrowed)
                    Console.WriteLine("Denna bok är inte utlånad!");
                else
                {
                    books[num - 1].IsBorrowed = false;
                    SaveBooks();
                    Console.WriteLine("Bok återlämnad!");
                }
            }
            else Console.WriteLine("Ogiltigt nummer!");
        }

        static void DeleteBook()
        {
            ListBooks();
            Console.Write("Boknummer att ta bort: ");
            if (int.TryParse(Console.ReadLine(), out int num) && num > 0 && num <= books.Count)
            {
                Console.Write("Är du säker? (j/n): ");
                if (Console.ReadLine().ToLower() == "j")
                {
                    books.RemoveAt(num - 1);
                    SaveBooks();
                    Console.WriteLine("Bok borttagen!");
                }
            }
            else Console.WriteLine("Ogiltigt nummer!");
        }
    }
}
