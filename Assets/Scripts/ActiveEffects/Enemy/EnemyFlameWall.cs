using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlameWall : ActiveEffect
{
    [SerializeField] private Transform wallPrefab;
    [SerializeField] private float tickTime;
    [SerializeField] private float totalDuration;

    private int playerInColliders = 0;

    public override void UpdateGameObject() {
    }

    private IEnumerator Start() {
        Instantiate(wallPrefab, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
        Destroy(gameObject, totalDuration);

        var player = FindObjectOfType<PlayerController>();

        while (true) {
            if (playerInColliders > 0) {
                player.Damage(damage);
            }

            yield return new WaitForSeconds(tickTime);
        }
    }

    private void OnTriggerEnter() {
        playerInColliders += 1;
        Debug.Log(playerInColliders);
    }

    private void OnTriggerExit() {
        playerInColliders -= 1;
        Debug.Log(playerInColliders);
    }
}
