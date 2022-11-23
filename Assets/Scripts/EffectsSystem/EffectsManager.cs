using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public InventorySO playerInventory;
    public InventorySO enemyInventory;
    public EffectsDatabaseSO database;
    public EnemyKilledSO enemyKilled;

    private void OnEnable() {
        enemyKilled.killed += OnKill;
        playerInventory.stonesChange += OnStonesChanged;
    }

    private void OnStonesChanged(int stones, int rocks, bool consumed) {
        if (consumed) {
            OnKill(0, 0);
        }
    }

    private void OnDisable() {
        enemyKilled.killed -= OnKill;
    }

    private void Start() {
        OnKill(0, 0);
    }

    private void OnKill(int remaining, int total) {
        if (remaining > 0) return;

        playerInventory.RemoveTemporary();
        enemyInventory.RemoveTemporary();

        var hasActive = false;
        int added = 0;
        var tempPassiveIndeces = new List<int>();

        while (added < 3) {
            if (((!hasActive && Random.Range(0, 3) == 0) || playerInventory.active.Count < 1) && database.active.Length > 0) {
                var active = database.active[Random.Range(0, database.active.Length)];
                var slot = playerInventory.GetSlot(active);
                if ((slot == null && playerInventory.active.Count >= playerInventory.maxActiveSlots) || (slot != null && active.levels.Length < (slot.permanent + slot.temporary + 1))) {
                    continue;
                }
                playerInventory.AddTemporary(active);
                hasActive = true;
                added += 1;
            } else if (database.passive.Length > 0) {
                var passiveIndex = Random.Range(0, database.passive.Length);
                if (tempPassiveIndeces.Contains(passiveIndex)) {
                    continue;
                }
                var passive = database.passive[passiveIndex];
                tempPassiveIndeces.Add(passiveIndex);
                playerInventory.AddTemporary(passive, passive.rarities[Random.Range(0, passive.rarities.Length)].rarity);
                added += 1;
            }
        }

        var enemyTypeRandom = Random.Range(0, 2);
        if (enemyTypeRandom == 0 && database.enemyActive.Length > 0) {
            enemyInventory.AddTemporary(database.enemyActive[Random.Range(0, database.enemyActive.Length)]);
        } else if (database.enemyPassive.Length > 0) {
            var passive = database.enemyPassive[Random.Range(0, database.enemyPassive.Length)];
            enemyInventory.AddTemporary(passive, passive.rarities[Random.Range(0, passive.rarities.Length)].rarity);
        }
    }
}
