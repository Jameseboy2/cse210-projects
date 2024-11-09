public class Breathing : Activities
{
    private const int breatheDuration = 4;

    public override void StartActivity()
    {
        Console.WriteLine("This activity will help you relax by walking you through breathing in and out slowly.");
        base.StartActivity();

        while (remainingTime > 0)
        {
            Console.Write("\nBreathe in... ");
            ShowCountdown(breatheDuration);
            
            if (remainingTime <= 0) break;
            
            Console.Write("\nBreathe out... ");
            ShowCountdown(breatheDuration);
        }

        Console.WriteLine(_exitingMessage);
    }
}

public class Reflection : Activities
{
    private List<string> reflectionPrompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless.",
    };

    private List<string> reflectionQuestions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "What did you learn about yourself through this experience?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
    };

    public override void StartActivity()
    {
        Console.WriteLine("This activity will help you reflect on times in your life when you have shown strength and resilience.");
        base.StartActivity();

        string prompt = reflectionPrompts[random.Next(reflectionPrompts.Count)];
        Console.WriteLine($"\nConsider the following prompt:\n\n--- {prompt} ---\n");
        Console.WriteLine("When you have something in mind, press enter to continue.");
        Console.ReadLine();

        Console.WriteLine("\nNow ponder each of the following questions as they related to this experience.");
        Console.Write("You may begin in: ");
        ShowCountdown(3);
        Console.WriteLine();

        while (remainingTime > 0)
        {
            string question = reflectionQuestions[random.Next(reflectionQuestions.Count)];
            Console.Write($"\n> {question}  ");
            ShowCountdown(10);
            Console.WriteLine();
        }

        Console.WriteLine(_exitingMessage);
    }
}


public class Listing : Activities
{
    private readonly List<string> listingPrompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?",
    };

    public override void StartActivity()
    {
        Console.WriteLine("This activity will help you reflect on things in your life by having you list as many things as you can.");
        base.StartActivity();

        string prompt = listingPrompts[random.Next(listingPrompts.Count)];
        Console.WriteLine($"\nList as many responses as you can to the following prompt:\n--- {prompt} ---");
        
        Console.Write("You may begin in: ");
        ShowCountdown(3);
        Console.WriteLine();

        int itemCount = 0;
        DateTime endTime = DateTime.Now.AddSeconds(remainingTime);

        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                itemCount++;
            }
        }

        Console.WriteLine($"\nYou listed {itemCount} items!");
        Console.WriteLine(_exitingMessage);
    }
}