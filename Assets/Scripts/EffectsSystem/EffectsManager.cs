using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private InventorySO playerInventory;
    [SerializeField] private InventorySO enemyInventory;
    [SerializeField] private EffectsDatabaseSO database;
    [SerializeField] private EnemyKilledSO enemyKilled;
    [SerializeField] private ExperienceSO experience;

    [SerializeField] private AudioSource randomisedSound;

    private void OnEnable() {
        enemyKilled.killed += OnKill;
        playerInventory.stonesChange += OnStonesChanged;
        experience.change += OnExperienceChange;
    }

    private void OnDisable() {
        enemyKilled.killed -= OnKill;
        playerInventory.stonesChange -= OnStonesChanged;
        experience.change -= OnExperienceChange;
    }

    private void Start() {
        UpdatePlayer();
        UpdateEnemy();
    }

    private void OnExperienceChange(int level, bool levelUp, int current, int required) {
        if (levelUp && level % 10 == 0) {
            var temp = enemyInventory.active.FirstOrDefault(slot => slot.temporary > 0);
            if (temp == null) {
                Debug.LogError("Cannot persist enemy spell");
                return;
            }

            enemyInventory.Promote(temp.effect);

            UpdateEnemy();
        }
    }

    private void OnStonesChanged(int stones, int rocks, bool consumed) {
        if (consumed) {
            UpdatePlayer();
        }
    }

    private void OnKill(int remaining, int total) {
        if (remaining > 0) return;

        UpdatePlayer();

        UpdateEnemy();

        Instantiate(randomisedSound);
    }

    private void UpdatePlayer() {
        playerInventory.RemoveTemporary();

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
    }

    private void UpdateEnemy() {
        enemyInventory.RemoveTemporary();

        if (database.enemyActive.Length <= 0) return;

        var effectNumber = Random.Range(0, database.enemyActive.Length - enemyInventory.active.Count);

        var index = 0;
        foreach (var effect in database.enemyActive) {
            var slot = enemyInventory.GetSlot(effect);

            if (slot == null) {
                if (index == effectNumber) {
                    enemyInventory.AddTemporary(effect);
                    break;
                } else {
                    index += 1;
                }
            }
        }

        //var enemyTypeRandom = Random.Range(0, 2);
        //if (/*enemyTypeRandom == 0 &&*/ database.enemyActive.Length > 0 && database.enemyActive.Length > enemyInventory.active.Count) {
        //    for (var i = 0; i < 5; i++) {
        //        var active = database.enemyActive[Random.Range(0, database.enemyActive.Length)];
        //        var slot = enemyInventory.GetSlot(active);
        //        if (slot != null) {
        //            continue;
        //        }
        //        enemyInventory.AddTemporary(active);
        //    }
        //}
        //} else if (database.enemyPassive.Length > 0) {
        //    var passive = database.enemyPassive[Random.Range(0, database.enemyPassive.Length)];
        //    enemyInventory.AddTemporary(passive, passive.rarities[Random.Range(0, passive.rarities.Length)].rarity);
        //}
    }
}
