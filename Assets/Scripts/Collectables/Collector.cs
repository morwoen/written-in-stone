using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;

    private float baseRadius;
    private new SphereCollider collider;

    private void Awake() {
        collider = GetComponent<SphereCollider>();
        baseRadius = collider.radius;
    }

    private void OnEnable() {
        inventory.effectsChange += OnEffectsChanged;
    }

    private void OnDisable() {
        inventory.effectsChange -= OnEffectsChanged;
    }
    
    private void OnEffectsChanged(List<InventorySO.ActiveInventorySlot> active, List<InventorySO.PassiveInventorySlot> passive) {
        collider.radius = baseRadius * (1 + inventory.GetPassiveMultiplier(PassiveEffectSO.EffectProperty.PickupRange) / 100f);
    }

    private void OnTriggerEnter(Collider other) {
        var collectable = other.GetComponent<ICollectable>();
        collectable?.Collect(transform.parent);
    }
}
