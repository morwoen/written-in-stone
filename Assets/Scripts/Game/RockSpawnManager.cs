using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RockSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;

    [SerializeField] private HealthSO playerHealth;

    [SerializeField] private Vector2 areaSize;

    [SerializeField] private Transform firstSpawn;

    [SerializeField] private float delay = 40;

    private GameObject spawnedRock;

    private bool keepSpawning = true;

    private void OnEnable() {
        playerHealth.death += OnPlayerDeath;
    }

    private void OnDisable() {
        playerHealth.death -= OnPlayerDeath;
    }

    private void OnPlayerDeath() {
        keepSpawning = false;
    }

    private IEnumerator Start() {
        yield return new WaitForSeconds(2);

        spawnedRock = Instantiate(rockPrefab, firstSpawn.position.WithY(0), Quaternion.identity);

        yield return new WaitForSeconds(delay);

        while (true) {
            if (!keepSpawning) break;

            if (!spawnedRock) {
                var x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
                var y = Random.Range(-areaSize.y / 2, areaSize.y / 2);
                var pos = new Vector3(x, 0, y);

                if (!NavMesh.SamplePosition(pos, out NavMeshHit hit, 0.5f, 1)) {
                    continue;
                }
                spawnedRock = Instantiate(rockPrefab, pos, Quaternion.identity);
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, 1, areaSize.y));
    }
}
