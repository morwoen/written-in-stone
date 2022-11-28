using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPassivesManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InventorySO inventory;
    [SerializeField] private HealthSO health;
    [SerializeField] private DashSO dash;
    [SerializeField] private int defencePerDamageReduced;

    private void OnEnable() {
        inventory.effectsChange += OnEffectsChanged;
    }

    private void OnDisable() {
        inventory.effectsChange -= OnEffectsChanged;
    }

    private void OnEffectsChanged(List<InventorySO.ActiveInventorySlot> active, List<InventorySO.PassiveInventorySlot> passive) {
        playerController.SetSpeedMultiplier(1 + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.MovementSpeed) / 100f);
        health.SetBonusHealth(inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.Health));

        var desiredDashes = dash.StartingCharges + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.Dash);
        if (desiredDashes > dash.MaxCharges) {
            var toAdd = desiredDashes - dash.MaxCharges;
            for (int i = 0; i < toAdd; i++) {
                dash.AddCharge();
            }
        } else if (desiredDashes < dash.MaxCharges) {
            var toRemove =  dash.MaxCharges - desiredDashes;
            for (int i = 0; i < toRemove; i++) {
                dash.RemoveCharge();
            }
        }

        var defence = inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.Defence) / defencePerDamageReduced;
        playerController.SetDamageReduction(defence);
    }
}
