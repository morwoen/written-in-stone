using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDirector : MonoBehaviour
{
    [SerializeField] private EnemyKilledSO enemyKilled;
    [SerializeField] private ExperienceSO experience;
    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private Enemy[] bossPrefabs;
    [SerializeField] private Vector2 spawnDistanceMinMax;
    [SerializeField] private GameObject bossSpawnSound;

    private LayerMask groundLayer;

    private Transform player;

    private int desiredEnemies = 20;
    private int maxDesiredEnemies = 100;
    private float difficultyMultiplier = 1;

    private void OnEnable() {
        groundLayer = LayerMask.GetMask("Ground");

        player = FindObjectOfType<PlayerController>().transform;

        enemyKilled.killed += OnEnemyKilled;
        experience.change += OnExperienceGain;
    }

    private void OnDisable() {
        enemyKilled.killed -= OnEnemyKilled;
        experience.change -= OnExperienceGain;
    }

    private void OnEnemyKilled(int remaining, int total) {
        if (remaining == 0) {
            desiredEnemies = Mathf.Clamp(desiredEnemies + 1, 1, maxDesiredEnemies);
        }

        if (total % 101 == 0) {
            Instantiate(bossSpawnSound, player.transform.position, Quaternion.identity);
            SpawnEnemy(bossPrefabs);
        }
    }

    private void Update() {
        if (transform.childCount < desiredEnemies) {
            SpawnEnemy(enemyPrefabs);
        }
    }

    private void SpawnEnemy(Enemy[] prefabs) {
        if (prefabs.Length == 0) return;
        for (var i = 0; i < 10; i++) {
            var offset = spawnDistanceMinMax.y.RandomPointInCircle(spawnDistanceMinMax.x);
            var position = new Vector3(
                player.transform.position.x + offset.x,
                player.transform.position.y,
                player.transform.position.z + offset.y
            );
            if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 0.5f, 1)) {
                continue;
            }
            position.y = 0;
            var enemy = Instantiate(prefabs[Random.Range(0, prefabs.Length)], position, transform.rotation, transform);
            enemy.SetMultiplier(difficultyMultiplier);
            break;
        }
    }

    private void OnExperienceGain(int level, bool levelUp, int current, int required) {
        if (levelUp) {
            difficultyMultiplier += 0.1f;
        }
    }
}
