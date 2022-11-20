using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownManagement;
using static Cinemachine.DocumentationSortingAttribute;

[DefaultExecutionOrder(-1)]
public class EffectsExecutor : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;
    [SerializeField] private Vector3 spawnPointOffset;

    private Dictionary<ActiveEffectSO, Cooldown> cooldownTracker;

    private float multicast = 0;

    private void OnEnable() {
        inventory.effectAdded += OnEffectAdded;
        inventory.effectRemoved += OnEffectRemoved;
        inventory.effectsChange += OnEffectsChange;
        cooldownTracker = new();
    }

    private void OnDisable() {
        inventory.effectAdded -= OnEffectAdded;
        inventory.effectRemoved -= OnEffectRemoved;
        inventory.effectsChange -= OnEffectsChange;
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

    private void OnEffectsChange(List<InventorySO.ActiveInventorySlot> active, List<InventorySO.PassiveInventorySlot> passive) {
        multicast = 1 + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.Multicast) / 100;
    }

    private void CastSpell(ActiveEffectSO.EffectLevel effectLevel, bool onPlayer, float multicast, float damageMultiplier, float areaMultiplier) {
        if (multicast > 1) {
            multicast -= 1;
        } else {
            var roll = Random.Range(0, 1f);
            if (roll > multicast) {
                return;
            }
        }

        var critChance = 0.05f + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellCritChance) / 100;
        var crit = Random.Range(0, 1f) < critChance;

        var damage = effectLevel.damage * damageMultiplier;
        if (crit) {
            var critDamageMultiplier = 2 + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellCritDamage) / 100;
            damage *= critDamageMultiplier;
        }

        GameObject go;
        if (onPlayer) {
            go = Instantiate(effectLevel.spellPrefab, transform.position + spawnPointOffset, transform.rotation, transform);
        } else {
            go = Instantiate(effectLevel.spellPrefab, transform.position + spawnPointOffset, transform.rotation);
        }

        // TODO: get the component & give it dmg & area

        Cooldown.Wait(0.2f).OnComplete(() => {
            CastSpell(effectLevel, onPlayer, multicast, damageMultiplier, areaMultiplier);
        });
    }

    private void ExecuteEvent(ActiveEffectSO active) {
        var slot = inventory.GetSlot(active);
        var level = active.levels[slot.permanent + slot.temporary - 1];
        var cooldownMultiplier = inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellCooldown, slot.effect.traits) / 100;
        cooldownTracker[active] = Cooldown.Wait(level.cooldown, cooldownMultiplier)
            .OnComplete(() => {
                var spellMulticast = multicast + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellMulticast, slot.effect.traits) / 100;
                var damageMultiplier = inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellDamage, slot.effect.traits) / 100;
                var areaMultiplier = inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellArea, slot.effect.traits) / 100;

                CastSpell(level, slot.effect.spawnOnPlayer, spellMulticast, damageMultiplier, areaMultiplier);
                
                ExecuteEvent(active);
            });
    }
}
