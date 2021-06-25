using UnityEngine;

public class Timer
{
    private float _timer;

    public bool IsTimerEnd(float time)
    {
        _timer += Time.deltaTime;
        if (_timer >= time)
        {
            _timer = 0f;
            return true;
        }
        return false;
    }
}
