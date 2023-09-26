using UnityEngine;
using File = System.IO.File;

/// <summary>
/// Demo implementation for saving and loading a ScoringMatch in the StreamingAssets.
/// Has not been tested!
/// </summary>
public static class ScorePersister
{
    private static readonly string path = Application.streamingAssetsPath + "/MainScore.json";

    public static ScoringMatch LoadScores()
    {
        var jsonString = File.ReadAllText(path);
        ScoringMatch scores = JsonUtility.FromJson<ScoringMatch>(jsonString);
        return scores;
    }


    public static void SaveScores(ScoringMatch scoringMatch)
    {
        string json = JsonUtility.ToJson(scoringMatch);
        File.WriteAllText(path, json);
    }
}