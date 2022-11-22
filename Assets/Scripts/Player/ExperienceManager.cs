using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    [SerializeField] private ExperienceSO playerExperience;
    [SerializeField] private InventorySO playerInventory;

    private void OnEnable() {
        playerExperience.change += OnExperienceChange;
    }

    private void OnExperienceChange(int level, bool levelUp, int current, int required) {
        if (levelUp) {
            playerInventory.AddStone();
        }
    }
}
