using System;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public static Action OnPlayerDie;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<Enemy>(out Enemy enemy))
            OnPlayerDie?.Invoke();
    }
}
