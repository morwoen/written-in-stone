using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] private InventorySO inventory;
    [SerializeField] private HealthSO health;

    public void Collect(Transform player) {
        inventory.AddRock();
        health.Heal(10);
        Destroy(gameObject);
    }
}
