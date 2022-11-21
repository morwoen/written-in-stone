using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    [SerializeField] private EnemyKilledSO enemyKilled;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] bossPrefabs;
    [SerializeField] private Vector2 spawnDistanceMinMax;

    private LayerMask groundLayer;

    private Transform player;

    private int desiredEnemies = 10;
    private int maxDesiredEnemies = 30;

    private void OnEnable() {
        groundLayer = LayerMask.GetMask("Ground");

        player = FindObjectOfType<PlayerController>().transform;

        enemyKilled.killed += OnEnemyKilled;
    }

    private void OnDisable() {
        enemyKilled.killed -= OnEnemyKilled;
    }

    private void OnEnemyKilled(int remaining, int total) {
        if (remaining == 0) {
            desiredEnemies = Mathf.Clamp(desiredEnemies + 1, 1, maxDesiredEnemies);
        }

        if (total % 100 == 0) {
            SpawnEnemy(bossPrefabs);
        }
    }

    private void Update() {
        if (transform.childCount < desiredEnemies) {
            SpawnEnemy(enemyPrefabs);
        }
    }

    private void SpawnEnemy(GameObject[] prefabs) {
        for (var i = 0; i < 10; i++) {
            var offset = spawnDistanceMinMax.y.RandomPointInCircle(spawnDistanceMinMax.x);
            var position = new Vector3(
                player.transform.position.x + offset.x,
                100,
                player.transform.position.z + offset.y
            );
            if (!Physics.Raycast(position, Vector3.down, 101, groundLayer)) {
                continue;
            }
            position.y = 0;
            Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, transform.rotation, transform);
            break;
        }
    }
}
