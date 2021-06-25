using AngryLab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPositions;
    [SerializeField] private float _timeBetweenEnemiesSpawn;

    private void Start() => StartCoroutine(nameof(SpawnEnemy));

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject enemyTemp = SPManager.instance.GetNextAvailablePoolItem("Enemies");

            enemyTemp.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Count)].position;
            enemyTemp.SetActive(true);

            yield return new WaitForSeconds(_timeBetweenEnemiesSpawn);
        }
    }
}
