using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] private InventorySO inventory;

    public void Collect(Transform player) {
        inventory.AddRock();
        // TODO: Animate?
        Destroy(gameObject);
    }
}
