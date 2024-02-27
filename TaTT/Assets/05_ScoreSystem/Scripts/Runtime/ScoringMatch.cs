using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ScoringMatch
{
    public UnityEvent<int> OnAnyScoreChanged = new UnityEvent<int>();

    private Dictionary<int, float> _scores = new Dictionary<int, float>();

    public Dictionary<int, float> Scores
    {
        get { return _scores; }
    }

    public List<int> GetIds()
    {
        return new List<int>(_scores.Keys);
    }

    //returns the ids of the best scores, also accounts for draws thats why it returns a list ;)
    public List<int> GetBestScores()
    {
        // take the current scores where the value is equal to the max of the current scores (aka the best scores)
        // then only select the player ids of these scores and return them as a list. 
        var topScore = _scores.Max(x => x.Value);
        return _scores.Where(x => Math.Abs(x.Value - topScore) < Mathf.Epsilon).Select(x => x.Key).ToList();
    }

    public List<Tuple<int, float>> GetDescendingScoreBoard()
    {
        return _scores.OrderByDescending(x => x.Value).Select(x =>
            Tuple.Create(x.Key, x.Value)
        ).ToList();
    }
    
    /// <summary>
    /// Returns a list of player id and awarded points tuples. If need be ask nico regarding the scoring system.
    /// <remarks>
    /// If players have equal points they share their placing eG two players share second place.
    /// </remarks>
    /// </summary>
    /// <returns>the evaluated score board for who gets how many points</returns>
    public List<Tuple<int, float>> GetThreeTwoOneScoreBoard()
    {
        var descPoints = GetDescendingScoreBoard();
        var resultBoard = new List<Tuple<int, float>>();
        float awardPoints = 3f;
        for (int i = 0; i < descPoints.Count; i++)
        {
            if (i > 0 && Math.Abs(descPoints[i].Item2 - descPoints[i - 1].Item2) < Mathf.Epsilon)
            {
                //we got a tie with the previous player, take the same points from him
                resultBoard.Add(Tuple.Create(descPoints[i].Item1, resultBoard[i - 1].Item2));
            }
            else
            {
                resultBoard.Add(Tuple.Create(descPoints[i].Item1, awardPoints));
                awardPoints--;
            }
        }

        return resultBoard;
    }

    public void AddIds(params int[] ids)
    {
        foreach (var id in ids)
        {
            if (!_scores.ContainsKey(id))
            {
                _scores.Add(id, 0f);
            }
        }
    }

    public float GetScore(int id)
    {
        return _scores[id];
    }

    public void AddScore(int id, float score)
    {
        if (!_scores.ContainsKey(id))
            _scores.Add(id, 0f);
        _scores[id] += score;
        OnAnyScoreChanged?.Invoke(id);
    }

    public void RemoveId(int id)
    {
        _scores.Remove(id);
    }

    public void ResetScores()
    {
        foreach (var key in _scores.Keys)
        {
            _scores[key] = 0f;
            OnAnyScoreChanged?.Invoke(key);
        }
    }

    public void RemoveAllIds()
    {
        _scores = new Dictionary<int, float>();
    }
}