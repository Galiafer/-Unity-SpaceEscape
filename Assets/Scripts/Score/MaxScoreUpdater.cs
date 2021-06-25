using UnityEngine;
using UnityEngine.UI;

public class MaxScoreUpdater : MonoBehaviour, IScore
{
    [SerializeField] private Text _text;

    private void OnEnable() => UpdateScore();

    public void UpdateScore()
    {
        int savedScore = PlayerPrefs.GetInt("PlayerScore");
        bool isNewRecord = RoundScoreUpdater.Score >= savedScore ? true : false;

        if (isNewRecord)
        {
            PlayerPrefs.SetInt("PlayerScore", RoundScoreUpdater.Score);
            _text.text = $"New RECORD: {RoundScoreUpdater.Score}";
        } else
        {
            _text.text = $"Record: {savedScore}";
        }
    }
}
