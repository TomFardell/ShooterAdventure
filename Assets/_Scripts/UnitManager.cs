using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    [SerializeField] GameObject _enemy;
    [SerializeField] GameObject _player;

    private const float MinEnemySpawnX = -6;
    private const float MaxEnemySpawnX = 6;
    private const float MinEnemySpawnY = -4;
    private const float MaxEnemySpawnY = 4;
    private const float EnemySpawnInterval = 2;
    private const float FirstEnemyTime = 5;
    private const float SpawnDistanceFromPlayer = 2;

    private float _nextEnemy;

    private void Start() {
        _nextEnemy = FirstEnemyTime;
    }

    private void Update() {
        if (Time.time > _nextEnemy) {
            SpawnEnemy();
            _nextEnemy = Time.time + EnemySpawnInterval;
        }
    }

    private void SpawnEnemy() {
        Vector2 spawnPosition;

        do {
            spawnPosition = new Vector2(Random.Range(MinEnemySpawnX, MaxEnemySpawnX),
                Random.Range(MinEnemySpawnY, MaxEnemySpawnY));
        } while (Vector2.Distance(_player.transform.position, spawnPosition) < SpawnDistanceFromPlayer);

        GameObject enemy = Instantiate(_enemy);
        enemy.transform.position = spawnPosition;
    }
}
