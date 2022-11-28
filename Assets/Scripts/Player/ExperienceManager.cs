using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private ExperienceSO playerExperience;
    [SerializeField] private InventorySO playerInventory;
    [SerializeField] private GameObject levelUpPrefab;

    private void OnEnable() {
        playerExperience.change += OnExperienceChange;
        playerInventory.effectsChange += OnEffectsChange;
    }

    private void OnDisable() {
        playerExperience.change -= OnExperienceChange;
        playerInventory.effectsChange -= OnEffectsChange;
    }

    private void OnExperienceChange(int level, bool levelUp, int current, int required) {
        if (levelUp) {
            Instantiate(levelUpPrefab, transform);
            playerInventory.AddStone();
        }
    }

    private void OnEffectsChange(List<InventorySO.ActiveInventorySlot> active, List<InventorySO.PassiveInventorySlot> passive) {
        playerExperience.SetMultiplier(1 + playerInventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.Experience) / 100f);
    }
}
