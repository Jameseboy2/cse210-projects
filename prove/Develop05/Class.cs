using System.Reflection;

public abstract class Goals
{
    protected string _goalTitle;
    protected string _goalDescription;
    protected int _points;

    public Goals(string title, string description, int points)
    {
        _goalTitle = title;
        _goalDescription = description;
        _points = points;
    }

    public string GetGoalTitle()
    {
        return _goalTitle;
    }

    public abstract int RecordEvent();
    public abstract string GetStringRepresentation();
    public abstract string GetDetailsString();
}