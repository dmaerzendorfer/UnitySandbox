using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class ScoringMatch
{
    private Dictionary<string, float> _scores = new Dictionary<string, float>();

    public Dictionary<string, float> Scores
    {
        get { return _scores; }
    }

    public List<string> GetIds()
    {
        return new List<string>(_scores.Keys);
    }

    public string GetBestScore()
    {
        return _scores.OrderByDescending(x => x.Value).Select(x => x.Key).First();
    }

    public void AddIds(params string[] ids)
    {
        foreach (var id in ids)
        {
            _scores.Add(id, 0f);
        }
    }

    public float GetScore(string id)
    {
        return _scores[id];
    }

    public void AddScore(string id, float score)
    {
        _scores[id] += score;
    }

    public void RemoveId(string id)
    {
        _scores.Remove(id);
    }

    public void ResetScores()
    {
        foreach (var key in _scores.Keys)
        {
            _scores[key] = 0f;
        }
    }

    public void RemoveAllIds()
    {
        _scores = new Dictionary<string, float>();
    }
}