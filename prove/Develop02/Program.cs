using System;

class Program
    {
        static void Main(string[] args)
        {
            Journal myJournal = new();
            List<string> prompt = new List<string>();
            int option = 0;
            while (option != 5)
            {
                Console.WriteLine("\nWhat do you want to do?");
                Console.WriteLine("1. Write an Entry");
                Console.WriteLine("2. Review Entries");
                Console.WriteLine("3. Save to file");
                Console.WriteLine("4. Load from file");
                Console.WriteLine("5. Exit\n");

                option = int.Parse(Console.ReadLine());

                if (option == 1)
                {
                    myJournal.WriteEntry();
                }
                else if (option == 2)
                {
                    myJournal.ReviewEntries();
                }
                else if (option == 3)
                {
                    myJournal.SaveToFile();
                }
                else if (option == 4)
                {
                    myJournal.LoadFromFile();
                }
                else if (option == 5)
                {
                    Console.WriteLine("Goodbye!");
                }
                else
                {
                    Console.WriteLine("Please enter a valid response...");
                }
            }
        }
    }