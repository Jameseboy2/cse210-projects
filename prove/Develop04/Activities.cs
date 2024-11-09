public class Activities
{
    protected int remainingTime;
    protected readonly string _exitingMessage = "\nThank you for completing this activity!";
    protected readonly Random random = new Random();

    public virtual void StartActivity()
    {
        Console.WriteLine("Welcome to this activity!");
        Console.Write("How long would you like to spend on this activity (in seconds)? ");
        while (!int.TryParse(Console.ReadLine(), out remainingTime) || remainingTime <= 0)
        {
            Console.Write("Please enter a valid positive number of seconds: ");
        }
        Console.WriteLine("\nPreparing to begin...");
        ShowSpinner(3);
    }

    public void ShowSpinner(int seconds)
    {
        string[] spinnerChars = { "\\", "|", "/", "-" };
        int spinnerCount = seconds * 4;
        
        for (int i = 0; i < spinnerCount && remainingTime > 0; i++)
        {
            Console.Write("\b \b" + spinnerChars[i % spinnerChars.Length]);
            Thread.Sleep(250);
        }
        Console.Write("\b \b");
    }

    public void ShowCountdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write($"\b \b{i}");
            Thread.Sleep(1000);
            remainingTime--;
        }
        Console.Write("\b \b");
    }
}