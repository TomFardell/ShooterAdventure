using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour {
    [SerializeField] private GameObject _enemy;

    private const float MinEnemySpawnX = -6;
    private const float MaxEnemySpawnX = 6;
    private const float MinEnemySpawnY = -4;
    private const float MaxEnemySpawnY = 4;
    private const float EnemySpawnInterval = 2;
    private const float FirstEnemyTime = 5;
    private const float SpawnDistanceFromPlayer = 2;

    private GameObject _player;

    private float _nextEnemy;

    public int EnemyCount { get; private set; }

    private void Start() {
        _player = GameObject.FindWithTag("Player");
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
        EnemyCount++;
    }

    public void DecrementEnemyCount() {
        EnemyCount--;
    }
}
