using UnityEngine;

public class ScoreSaver : MonoBehaviour
{
    private int _prevRecord;

    private void Start()
    {
        _prevRecord = PlayerPrefs.GetInt("PlayerScore", 0);

        Circle.OnPlayerDie += SaveScore;
    }

    private void SaveScore()
    {
        if(RoundScoreUpdater.Score > _prevRecord)
            PlayerPrefs.SetInt("PlayerScore", RoundScoreUpdater.Score);
    }
}
