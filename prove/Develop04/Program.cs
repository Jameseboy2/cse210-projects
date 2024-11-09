using System;
using System.Collections.Generic;
using System.Threading;
class Program
{
    static void Main(string[] args)
    {
        var breathingAct = new Breathing();
        var reflectingAct = new Reflection();
        var listingAct = new Listing();
        int option = 0;

        while (option != 4)
        {
            Console.WriteLine("\nMindfulness Activities Menu:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Exit\n");

            if (!int.TryParse(Console.ReadLine(), out option))
            {
                Console.WriteLine("Please enter a valid number...");
                continue;
            }

            switch (option)
            {
                case 1:
                    breathingAct.StartActivity();
                    break;
                case 2:
                    reflectingAct.StartActivity();
                    break;
                case 3:
                    listingAct.StartActivity();
                    break;
                case 4:
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Please enter a valid option (1-4)...");
                    break;
            }
        }
    }
}