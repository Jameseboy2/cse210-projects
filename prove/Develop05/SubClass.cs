public class SimpleGoal : Goals
{
    private bool _isCompleted;

    public SimpleGoal(string title, string description, int points, bool isCompleted) 
        : base(title, description, points)
    {
        _isCompleted = isCompleted;
    }

    public override int RecordEvent()
    {
        if (!_isCompleted)
        {
            _isCompleted = true;
            return _points;
        }
        return 0;
    }

    public override string GetStringRepresentation()
    {
        return $"SimpleGoal:{_goalTitle},{_goalDescription},{_points},{_isCompleted}";
    }

    public override string GetDetailsString()
    {
        return $"[{(_isCompleted ? "X" : " ")}] {_goalTitle} ({_goalDescription})";
    }
}

public class EternalGoal : Goals
{
    public EternalGoal(string title, string description, int points) 
        : base(title, description, points)
    {
    }

    public override int RecordEvent()
    {
        return _points;
    }

    public override string GetStringRepresentation()
    {
        return $"EternalGoal:{_goalTitle},{_goalDescription},{_points}";
    }

    public override string GetDetailsString()
    {
        return $"[ ] {_goalTitle} ({_goalDescription})";
    }
}

public class ChecklistGoal : Goals
{
    private int _bonusPoints;
    private int _targetCount;
    private int _completedCount;

    public ChecklistGoal(string title, string description, int points, int bonusPoints, int targetCount, int completedCount) 
        : base(title, description, points)
    {
        _bonusPoints = bonusPoints;
        _targetCount = targetCount;
        _completedCount = completedCount;
    }

    public override int RecordEvent()
    {
        _completedCount++;
        if (_completedCount == _targetCount)
        {
            return _points + _bonusPoints;
        }
        return _points;
    }

    public override string GetStringRepresentation()
    {
        return $"ChecklistGoal:{_goalTitle},{_goalDescription},{_points},{_bonusPoints},{_targetCount},{_completedCount}";
    }

    public override string GetDetailsString()
    {
        return $"[{(_completedCount >= _targetCount ? "X" : " ")}] {_goalTitle} ({_goalDescription}) -- Currently completed: {_completedCount}/{_targetCount}";
    }
}