using System;

class Program
{
    static int _totalPoints = 0;
    static List<Goals> _goals = new List<Goals>();

    static void Main(string[] args)
    {
        void SaveGoals()
        {
            try
            {
                Console.WriteLine("What .txt filename will you save it under? ");
                string filename = Console.ReadLine();
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    // Write total points first
                    writer.WriteLine(_totalPoints);
                    // Write each goal
                    foreach (var goal in _goals)
                    {
                        writer.WriteLine(goal.GetStringRepresentation());
                    }
                }
                Console.WriteLine("Goals saved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving goals: {ex.Message}");
            }
        }

        void RecordGoals()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals to record. Please create some goals first.");
                return;
            }

            Console.WriteLine("The goals are:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetGoalTitle()}");
            }

            Console.Write("Which goal did you accomplish? ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _goals.Count)
            {
                int earnedPoints = _goals[choice - 1].RecordEvent();
                _totalPoints += earnedPoints;
                Console.WriteLine($"Congratulations! You have earned {earnedPoints} points!");
                Console.WriteLine($"You now have {_totalPoints} points.");
            }
            else
            {
                Console.WriteLine("Invalid goal number.");
            }
        }

        void LoadGoals(string filename)
        {
            try
            {
                string[] lines = File.ReadAllLines(filename);
                if (lines.Length > 0)
                {
                    _totalPoints = int.Parse(lines[0]);
                    _goals.Clear();

                    for (int i = 1; i < lines.Length; i++)
                    {
                        string[] parts = lines[i].Split(':');
                        if (parts.Length == 2)
                        {
                            string goalType = parts[0];
                            string[] goalData = parts[1].Split(',');

                            Goals goal = null;
                            switch (goalType)
                            {
                                case "SimpleGoal":
                                    goal = new SimpleGoal(goalData[0], goalData[1], int.Parse(goalData[2]), bool.Parse(goalData[3]));
                                    break;
                                case "EternalGoal":
                                    goal = new EternalGoal(goalData[0], goalData[1], int.Parse(goalData[2]));
                                    break;
                                case "ChecklistGoal":
                                    goal = new ChecklistGoal(goalData[0], goalData[1], int.Parse(goalData[2]), 
                                        int.Parse(goalData[3]), int.Parse(goalData[4]), int.Parse(goalData[5]));
                                    break;
                            }
                            if (goal != null)
                            {
                                _goals.Add(goal);
                            }
                        }
                    }
                    Console.WriteLine("Goals loaded successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading goals: {ex.Message}");
            }
        }

        void CreateNewGoal()
        {
            Console.WriteLine("The types of Goals are:");
            Console.WriteLine("1. Simple Goal");
            Console.WriteLine("2. Eternal Goal");
            Console.WriteLine("3. Checklist Goal");
            Console.Write("Which type of goal would you like to create? ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                Console.Write("What is the name of your goal? ");
                string name = Console.ReadLine();
                Console.Write("What is a short description of it? ");
                string description = Console.ReadLine();
                Console.Write("What is the amount of points associated with this goal? ");
                int points = int.Parse(Console.ReadLine());

                Goals goal = null;
                switch (choice)
                {
                    case 1:
                        goal = new SimpleGoal(name, description, points, false);
                        break;
                    case 2:
                        goal = new EternalGoal(name, description, points);
                        break;
                    case 3:
                        Console.Write("How many times does this goal need to be accomplished? ");
                        int targetCount = int.Parse(Console.ReadLine());
                        Console.Write("What is the bonus for accomplishing it that many times? ");
                        int bonusPoints = int.Parse(Console.ReadLine());
                        goal = new ChecklistGoal(name, description, points, bonusPoints, targetCount, 0);
                        break;
                    default:
                        Console.WriteLine("Invalid goal type.");
                        return;
                }

                if (goal != null)
                {
                    _goals.Add(goal);
                    Console.WriteLine("Goal created successfully!");
                }
            }
        }

        void ListGoals()
        {
            if (_goals.Count == 0)
            {
                Console.WriteLine("No goals created yet.");
                return;
            }

            Console.WriteLine("The goals are:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
            }
        }

        int option = 0;
        Console.WriteLine("Welcome to the Goal Tracker!");

        while (option != 6)
        {
            Console.WriteLine($"\nYou have {_totalPoints} points.\n");
            Console.WriteLine("Menu Options:");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. List Goals");
            Console.WriteLine("3. Save Goals");
            Console.WriteLine("4. Load Goals");
            Console.WriteLine("5. Record Event");
            Console.WriteLine("6. Quit");
            Console.Write("Select a choice from the menu: ");

            if (int.TryParse(Console.ReadLine(), out option))
            {
                switch (option)
                {
                    case 1:
                        CreateNewGoal();
                        break;
                    case 2:
                        ListGoals();
                        break;
                    case 3:
                        SaveGoals();
                        break;
                    case 4:
                        Console.Write("What is the filename? ");
                        string filename = Console.ReadLine();
                        LoadGoals(filename);
                        break;
                    case 5:
                        RecordGoals();
                        break;
                    case 6:
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Please enter a valid option.");
                        break;
                }
            }
        }
    }
}