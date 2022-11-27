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
    [SerializeField] private EffectCooldownsTrackerSO cooldownsTracker;
    [SerializeField] private HealthSO playerHealth;

    private float multicast = 0;

    private void OnEnable() {
        inventory.effectAdded += OnEffectAdded;
        inventory.effectRemoved += OnEffectRemoved;
        inventory.effectsChange += OnEffectsChange;
        playerHealth.death += OnPlayerDeath;
    }

    private void OnDisable() {
        inventory.effectAdded -= OnEffectAdded;
        inventory.effectRemoved -= OnEffectRemoved;
        inventory.effectsChange -= OnEffectsChange;
        playerHealth.death -= OnPlayerDeath;
    }

    private void OnPlayerDeath() {
        cooldownsTracker.StopAll();
        this.enabled = false;
    }

    private void OnEffectRemoved(ActiveEffectSO active, PassiveEffectSO passive) {
        if (active == null) return;
        cooldownsTracker.RemoveCooldown(active);
    }

    private void OnEffectAdded(ActiveEffectSO active, PassiveEffectSO passive) {
        if (active == null) return;
        if (cooldownsTracker.HasCooldown(active)) return;
        ExecuteEvent(active);
    }

    private void OnEffectsChange(List<InventorySO.ActiveInventorySlot> active, List<InventorySO.PassiveInventorySlot> passive) {
        multicast = 1 + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.Multicast) / 100f;
    }

    private void CastSpell(ActiveEffectSO.EffectLevel effectLevel, bool onPlayer, float multicast, float damageMultiplier, float areaMultiplier) {
        if (multicast < 1) {
            var roll = Random.Range(0, 1f);
            if (roll > multicast) {
                return;
            }
        }

        multicast -= 1;

        var critChance = 0.05f + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellCritChance) / 100f;
        var crit = Random.Range(0, 1f) < critChance;

        var damage = effectLevel.damage * damageMultiplier;
        if (crit) {
            var critDamageMultiplier = 2 + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellCritDamage) / 100f;
            damage *= critDamageMultiplier;
        }

        GameObject go;
        if (onPlayer) {
            go = Instantiate(effectLevel.spellPrefab, transform.position + spawnPointOffset, transform.rotation, transform);
        } else {
            go = Instantiate(effectLevel.spellPrefab, transform.position + spawnPointOffset, transform.rotation);
        }

        ActiveEffect effect = go.GetComponent<ActiveEffect>();
        effect.SetParameters(Mathf.CeilToInt(damage), areaMultiplier);

        Cooldown.Wait(0.2f).OnComplete(() => {
            CastSpell(effectLevel, onPlayer, multicast, damageMultiplier, areaMultiplier);
        });
    }

    private void ExecuteEvent(ActiveEffectSO active) {
        var slot = inventory.GetSlot(active);
        var level = active.levels[slot.permanent + slot.temporary - 1];
        var cooldownMultiplier = 1 + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellCooldown, slot.effect.traits) / 100f;
        var cd = Cooldown.Wait(level.cooldown, cooldownMultiplier)
            .OnComplete(() => {
                var spellMulticast = multicast + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellMulticast, slot.effect.traits) / 100f;
                var damageMultiplier = 1 + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellDamage, slot.effect.traits) / 100f;
                var areaMultiplier = 1 + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.SpellArea, slot.effect.traits) / 100f;

                CastSpell(level, slot.effect.spawnOnPlayer, spellMulticast, damageMultiplier, areaMultiplier);
                
                ExecuteEvent(active);
            });
        cooldownsTracker.RegisterCooldown(active, cd);
    }
}
