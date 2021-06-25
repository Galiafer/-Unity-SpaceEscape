using UnityEngine;

public class EnemyMovements : MonoBehaviour
{
    [SerializeField] private float _speed;

    private const float SPEED_INCREASER = .1f;

    #region Unity Functions
    private void Start() => DifficultyIncreaser.OnDifficultyIncrease += IncreaseSpeed;

    private void Update() => transform.Translate(Vector2.left * _speed * Time.deltaTime);
    #endregion

    private void IncreaseSpeed()
    {
        _speed += SPEED_INCREASER;
        _speed = Mathf.Clamp(_speed, 2.5f, 50f);
    }
}
