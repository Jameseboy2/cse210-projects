using System;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    static void Main(string[] args)
    {
        Ref refClass = new();
        Verse verseClass = new(refClass);
        Word wordClass = new();

        Console.WriteLine("Scripture Memorization Program");
        Console.WriteLine("-----------------------------");
        
        while (true)
        {
            Console.WriteLine("\nEnter the phrase you would like to memorize (or type 'exit' to quit):");
            string input = Console.ReadLine();
            
            if (input == "exit")
            {
                break;
            }

            refClass.AddPhrase(input);
            verseClass.PullVerse();
            
            Console.WriteLine("\nOriginal phrase:");
            Console.WriteLine(verseClass.GetSelectedVerse());
            Console.WriteLine("\nPress Enter to begin...");
            Console.ReadLine();

            wordClass.Reset(); 
            bool canContinue = true;

            while (canContinue)
            {
                Console.Clear();  // Clear the console before showing new version
                canContinue = wordClass.RemoveRandomWords(verseClass);
                Console.WriteLine(verseClass.GetSelectedVerse());

                if (!canContinue)
                {
                    Console.WriteLine("\nAll words have been hidden! Press Enter to try a new phrase...");
                    Console.ReadLine();
                    break;
                }

                Console.WriteLine("\nPress Enter to hide more words or type 'new' for a new phrase (or 'exit' to quit):");
                string command = Console.ReadLine()?.ToLower();

                if (command == "new")
                {
                    break;
                }
                else if (command == "exit")
                {
                    return;
                }
            }
        }

        Console.WriteLine("\nThank you for using the Scripture Memorization Program!");
    }
}


// var ref = new Ref();
// var verse = new Verse(ref);
// var word = new Word(verse);

// ref.AddPhrase(""); // User inputs phrase
// verse.PullVerse(); // Display original verse
// word.RemoveRandomWords(); // Remove 3 random words