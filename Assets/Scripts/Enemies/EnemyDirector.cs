using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDirector : MonoBehaviour
{
    [SerializeField] private EnemyKilledSO enemyKilled;
    [SerializeField] private GameObject[] enemyPrefabs;

    private Transform player;

    private int desiredEnemies = 10;

    private void OnEnable() {
        player = FindObjectOfType<PlayerController>().transform;

        enemyKilled.killed += OnEnemyKilled;
    }

    private void OnEnemyKilled(int remaining) {
        if (remaining == 0) {
            desiredEnemies += 1;
        }
    }

    private void Update() {
        if (transform.childCount < desiredEnemies) {
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], transform.position, transform.rotation, transform);
        }
    }
}
