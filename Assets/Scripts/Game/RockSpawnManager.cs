using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;

    [SerializeField] private Vector2 areaSize;

    private GameObject spawnedRock;

    private IEnumerator Start() {
        while (true) {
            if (!spawnedRock) {
                var x = Random.Range(-areaSize.x / 2, areaSize.x / 2);
                var y = Random.Range(-areaSize.y / 2, areaSize.y / 2);
                spawnedRock = Instantiate(rockPrefab, new Vector3(x, transform.position.y, y), Quaternion.identity, transform);
            }

            yield return new WaitForSeconds(40);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, 1, areaSize.y));
    }
}
