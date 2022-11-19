using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownManagement;

[DefaultExecutionOrder(-1)]
public class EffectsExecutor : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;

    private Dictionary<ActiveEffectSO, Cooldown> cooldownTracker;

    private void OnEnable() {
        inventory.effectAdded += OnEffectAdded;
        inventory.effectRemoved += OnEffectRemoved;
        cooldownTracker = new();
    }

    private void OnDisable() {
        inventory.effectAdded -= OnEffectAdded;
        inventory.effectRemoved -= OnEffectRemoved;
    }

    private void OnEffectRemoved(ActiveEffectSO active, PassiveEffectSO passive) {
        if (active == null) return;
        if (cooldownTracker.ContainsKey(active)) {
            cooldownTracker[active].Stop();
            cooldownTracker.Remove(active);
        }
    }

    private void OnEffectAdded(ActiveEffectSO active, PassiveEffectSO passive) {
        if (active == null) return;
        if (cooldownTracker.ContainsKey(active)) return;
        ExecuteEvent(active);
    }

    private void ExecuteEvent(ActiveEffectSO active) {
        var slot = inventory.GetSlot(active);
        var level = active.levels[slot.permanent + slot.temporary - 1];
        cooldownTracker[active] = Cooldown.Wait(level.cooldown)
            .OnComplete(() => {
                if (active.spawnOnPlayer) {
                    Instantiate(level.spellPrefab, transform.position, transform.rotation, transform);
                } else {
                    Instantiate(level.spellPrefab, transform.position, transform.rotation);
                }
                ExecuteEvent(active);
            });
    }
}
