using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public InventorySO playerInventory;
    public InventorySO enemyInventory;
    public EffectsDatabaseSO database;
    public EnemyKilledSO enemyKilled;

    private void OnEnable() {
        enemyKilled.killed += OnKill;
    }

    private void OnDisable() {
        enemyKilled.killed -= OnKill;
    }

    private void Start() {
        OnKill(0);
    }

    private void OnKill(int remaining) {
        if (remaining > 0) return;

        playerInventory.RemoveTemporary();
        enemyInventory.RemoveTemporary();

        var hasActive = false;

        for (int i = 0; i < 3; i++) {
            if (!hasActive && Random.Range(0, 3) == 0) {
                playerInventory.AddTemporary(database.active[Random.Range(0, database.active.Length)]);
                hasActive = true;
            } else {
                var passive = database.passive[Random.Range(0, database.passive.Length)];
                playerInventory.AddTemporary(passive, passive.rarities[Random.Range(0, passive.rarities.Length)].rarity);
            }
        }

        var enemyTypeRandom = Random.Range(0, 2);
        if (enemyTypeRandom == 0) {
            enemyInventory.AddTemporary(database.enemyActive[Random.Range(0, database.enemyActive.Length)]);
        } else {
            var passive = database.enemyPassive[Random.Range(0, database.enemyPassive.Length)];
            enemyInventory.AddTemporary(passive, passive.rarities[Random.Range(0, passive.rarities.Length)].rarity);
        }
        // TODO: Add random 3 effects that aren't too high level
    }
}
