using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StonesCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stonesText;
    [SerializeField] private TextMeshProUGUI rocksText;

    [SerializeField] private InventorySO inventory;

    [SerializeField] private GameObject prompt;

    private void OnEnable() {
        inventory.stonesChange += OnStonesChanged;

        OnStonesChanged(inventory.stones, inventory.rocks, false);
    }

    private void OnDisable() {
        inventory.stonesChange -= OnStonesChanged;
    }

    private void OnStonesChanged(int stones, int rocks, bool consumed) {
        stonesText.SetText(stones.ToString("00.##"));
        rocksText.SetText(rocks.ToString("0.#/3"));

        prompt.SetActive(stones > 0);
    }
}
