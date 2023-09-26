using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private ScoringMatch _mainScore;

    public ScoringMatch MainScore
    {
        get => _mainScore;
        private set => _mainScore = value;
    }

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            _mainScore = new ScoringMatch();
            _subScores = new Dictionary<string, ScoringMatch>();
            Instance = this;
        }
    }

    private Dictionary<string, ScoringMatch> _subScores;
    
    /// <summary>
    /// Creates a new Subscore with the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the new subscore</returns>
    public ScoringMatch CreateBlankSubScore(string id)
    {
        var subScore = new ScoringMatch();
        _subScores.Add(id, subScore);
        return subScore;
    }

    public List<string> GetCurrentSubScoreIds()
    {
        return new List<string>(_subScores.Keys);
    }

    /// <summary>
    /// Creates a new subscore from the mainScore with the given id. The ids/players of the mainScore are copied into the new subScore, their points are set to 0.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the new subScore</returns>
    public ScoringMatch CreateSubScoreFromMainScore(string id)
    {
        var ids = _mainScore.GetIds();
        var subScore = new ScoringMatch();
        subScore.AddIds(ids.ToArray());
        _subScores.Add(id, subScore);
        return subScore;
    }

    /// <summary>
    /// Removes the subScore with the given id.
    /// </summary>
    /// <param name="id"></param>
    public void RemoveSubScore(string id)
    {
        _subScores.Remove(id);
    }

    /// <summary>
    /// Calls the provided Action with mainScore, subScore. mainScore being param1! 
    /// </summary>
    /// <example>
    /// Usage example:
    /// <code>
    /// ScoreManager.Instance.ResolveSubScore("subGame1", ScoreManager.MostPointsGetsOnePoint);
    /// </code>
    /// </example>
    /// <param name="id">of the subscore</param>
    /// <param name="resolveAction">defines how to resolve the subscore, gets the mainScore as param1 and the subScore as param2</param>
    /// <param name="deleteSubScore">whether or not the subscore should be deleted after calling the resolve function, defaults to true</param>
    public void ResolveSubScore(string id, Action<ScoringMatch, ScoringMatch> resolveAction, bool deleteSubScore = true)
    {
        resolveAction.Invoke(_mainScore, _subScores[id]);
        if (deleteSubScore)
        {
            _subScores.Remove(id);
        }
    }

    /// <summary>
    /// A simple ScoreResolve function provided for convenience.
    /// The best player in the subScore is awarded one point in the mainScore
    /// </summary>
    /// <param name="mainScore"></param>
    /// <param name="subScore"></param>
    public static void MostPointsGetsOnePoint(ScoringMatch mainScore, ScoringMatch subScore)
    {
        string winId = subScore.GetBestScore();
        mainScore.AddScore(winId, 1);
    }

    /// <summary>
    /// A simple scoreResolve function provided for convenience.
    /// All the subScores points are transferred over to the mainScore.
    /// </summary>
    /// <param name="mainScore"></param>
    /// <param name="subScore"></param>
    public static void TransferAllPoints(ScoringMatch mainScore, ScoringMatch subScore)
    {
        foreach (var score in subScore.Scores)
        {
            mainScore.AddScore(score.Key, score.Value);
        }
    }

}