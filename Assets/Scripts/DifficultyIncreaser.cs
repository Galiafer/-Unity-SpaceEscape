using System;
using UnityEngine;

public class DifficultyIncreaser : MonoBehaviour
{
    [SerializeField] private float _timeToIncrease;

    private Timer _timer = new Timer();

    public static Action OnDifficultyIncrease;

    private void Update()
    {
        if (_timer.IsTimerEnd(_timeToIncrease))
            OnDifficultyIncrease?.Invoke();
    }
}
