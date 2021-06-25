using System;
using UnityEngine;
using UnityEngine.UI;

public class RoundScoreUpdater : MonoBehaviour, IScore
{
    [SerializeField] private Text _scoreText;

    #region Consts
    private const float DECREASE_FORCE = 0.1f;
    private const int POINT = 1;
    #endregion

    #region Variables
    private Timer _timer = new Timer();
    private static int _score;
    private float _timeToScore = 1f;
    #endregion

    #region Properties
    public static int Score => _score;
    #endregion

    private void Start()
    {
        _score = 0;

        DifficultyIncreaser.OnDifficultyIncrease += IncreaseScoreCountingSpeed;
    }

    private void Update() => UpdateScore();

    public void UpdateScore()
    {
        if(_timer.IsTimerEnd(_timeToScore))
            _score += POINT;

        _scoreText.text = $"{_score}";
    }

    private void IncreaseScoreCountingSpeed()
    {
        _timeToScore -= DECREASE_FORCE;
        _timeToScore = Mathf.Clamp(_timeToScore, 0f, 1f);
    }
}
