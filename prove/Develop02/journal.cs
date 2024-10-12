class Journal
{
    public List<(string entry, DateTime timestamp)> entries = new List<(string entry, DateTime timestamp)>();
    public RandomPrompt randomPrompt = new();

    public void WriteEntry()
    {
        string prompt = randomPrompt.GetRandomPrompt();
        Console.WriteLine("Here is your writing prompt: " + prompt);
        Console.WriteLine("Enter your journal entry:");
        string entry = Console.ReadLine();
        DateTime timestamp = DateTime.Now;
        entries.Add((entry, timestamp));
        Console.WriteLine("Entry added.");
    }

    public void ReviewEntries()
    {
        Console.WriteLine("Your journal entries:");
        foreach (var (entry, timestamp) in entries)
        {
            Console.WriteLine($"[{timestamp}] {entry}");
        }
    }

    public void SaveToFile()
    {
        Console.WriteLine("Enter the filename to save:");
        string filename = Console.ReadLine();
        List<string> lines = new List<string>();
        foreach (var (entry, timestamp) in entries)
        {
            lines.Add($"{timestamp}\t{entry}");
        }
        File.WriteAllLines(filename, lines);
        Console.WriteLine("Entries saved to file.");
    }

    public void LoadFromFile()
    {
        Console.WriteLine("Enter the filename to load:");
        string filename = Console.ReadLine();
        if (File.Exists(filename))
        {
            entries.Clear();
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                string[] parts = line.Split('\t');
                DateTime timestamp = DateTime.Parse(parts[0]);
                string entry = parts[1];
                entries.Add((entry, timestamp));
            }
            Console.WriteLine("Entries loaded from file.");
        }
        else
        {
            Console.WriteLine("File not found.");
        }
        
    }
}

class RandomPrompt
{
    public List<string> prompts = new List<string>
    {
        "Write about a memorable moment from your childhood.",
        "Describe a place you would like to visit and why.",
        "What are your goals for the next year?",
        "Write about a person who has influenced your life.",
        "Describe your favorite hobby and why you enjoy it.",
        "Who did you meet today? What did you talk about?",
        "What did you eat today?",
        "What did you do today? Why did you do it?",
        "Did you learn something new today?",
        "What was the highlight of today? What made is special?",
        "How did you grow today?"
    };

    public Random random = new Random();

    public string GetRandomPrompt()
    {
        int index = random.Next(prompts.Count);
        return prompts[index];
    }
}