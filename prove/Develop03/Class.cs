using System;
using System.Collections.Generic;
using System.Linq;

public class Ref
{
    private List<string> _list;

    public Ref()
    {
        _list = new List<string>();
    }

    public void AddPhrase(string phrase)
    {
        string[] words = phrase.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        _list.Clear();
        _list.AddRange(words);
    }

    public List<string> GetWords()
    {
        return _list;
    }
}

public class Verse
{
    private string _selectedVerse;
    private readonly Ref _ref;

    public Verse(Ref refInstance)
    {
        _ref = refInstance;
    }

    public void PullVerse()
    {
        List<string> words = _ref.GetWords();
        _selectedVerse = string.Join(" ", words);
    }

    public string GetSelectedVerse()
    {
        return _selectedVerse;
    }

    public void SetSelectedVerse(string verse)
    {
        _selectedVerse = verse;
    }
}

public class Word
{
    private readonly Random _random = new();
    private HashSet<int> _removedIndices = new();
    private int _totalWords;

    public bool RemoveRandomWords(Verse verse)
    {
        string memorizeVerse = verse.GetSelectedVerse();
        List<string> words = memorizeVerse.Split(' ').ToList();
        _totalWords = words.Count;

        // If all words are already hidden, return false
        if (_removedIndices.Count >= _totalWords)
        {
            return false;
        }

        int wordsToRemove = Math.Min(3, _totalWords - _removedIndices.Count);
        int removedThisRound = 0;

        while (removedThisRound < wordsToRemove)
        {
            int index = _random.Next(words.Count);
            if (!_removedIndices.Contains(index))
            {
                words[index] = "_____";
                _removedIndices.Add(index);
                removedThisRound++;
            }
        }

        string newVerse = string.Join(" ", words);
        verse.SetSelectedVerse(newVerse);
        
        return true;
    }

    public void Reset()
    {
        _removedIndices.Clear();
    }
}