using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownManagement;

public class ActiveEffectsPanel : MonoBehaviour
{
    [SerializeField] private EffectImage effectImagePrefab;
    [SerializeField] private InventorySO inventory;
    [SerializeField] private EffectCooldownsTrackerSO cooldownsTracker;
    [SerializeField] private EffectImage.TooltipSide tooltipSide = EffectImage.TooltipSide.BottomRight;

    private List<EffectImage> elements;

    private void OnEnable() {
        elements = new();

        inventory.effectsChange += OnEffectsChanged;
        inventory.effectAdded += OnEffectAdded;
        inventory.effectRemoved += OnEffectRemoved;
        cooldownsTracker.cooldownAdded += OnCooldownAdded;

        OnEffectsChanged(inventory.active, inventory.passive);
    }

    private void OnDisable() {
        inventory.effectsChange -= OnEffectsChanged;
        inventory.effectAdded -= OnEffectAdded;
        inventory.effectRemoved -= OnEffectRemoved;
        cooldownsTracker.cooldownAdded -= OnCooldownAdded;
    }

    private void OnCooldownAdded(ActiveEffectSO effect, Cooldown cooldown) {
        foreach (var el in elements) {
            if (el.Is(effect)) {
                el.GetComponent<CooldownUIOverlay>().ShowCooldown(cooldown);
                break;
            }
        }
    }

    private void OnEffectsChanged(List<InventorySO.ActiveInventorySlot> active, List<InventorySO.PassiveInventorySlot> passive) {
        foreach (var el in elements) {
            el.Refresh();
        }
    }

    private void OnEffectAdded(ActiveEffectSO active, PassiveEffectSO passive) {
        if (active == null) return;
        var el = Instantiate(effectImagePrefab, transform);
        el.Apply(inventory.GetSlot(active));
        el.SetTooltip(true, tooltipSide);
        elements.Add(el);
        el.GetComponent<CooldownUIOverlay>().ShowCooldown(cooldownsTracker.GetCooldown(active));
    }

    private void OnEffectRemoved(ActiveEffectSO active, PassiveEffectSO passive) {
        if (active == null) return;
        for (var i = 0; i < elements.Count; i++) {
            if (elements[i].Is(active)) {
                Destroy(elements[i].gameObject);
                elements.RemoveAt(i);
            }
        }
    }
}
