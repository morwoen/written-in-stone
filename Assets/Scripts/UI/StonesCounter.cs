using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CooldownManagement;

public class StonesCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stonesText;

    [SerializeField] private InventorySO inventory;

    [SerializeField] private GameObject prompt;

    [SerializeField] private Animator rock1;
    [SerializeField] private Animator rock2;
    [SerializeField] private Animator rock3;
    [SerializeField] private Animator stone;

    [SerializeField] private AudioSource stoneCompletedSound;

    private int previousStones = 0;

    private void OnEnable() {
        inventory.stonesChange += OnStonesChanged;

        rock1.gameObject.SetActive(false);
        rock2.gameObject.SetActive(false);
        rock3.gameObject.SetActive(false);

        OnStonesChanged(inventory.stones, inventory.rocks, false);
    }

    private void OnDisable() {
        inventory.stonesChange -= OnStonesChanged;
    }

    private void OnStonesChanged(int stones, int rocks, bool consumed) {
        stonesText.SetText(stones.ToString("00.##"));

        if (rocks > 0) {
            rock1.gameObject.SetActive(true);
        }
        if (rocks > 1) {
            rock2.gameObject.SetActive(true);
        }
        if (rock1.gameObject.activeSelf && rocks == 0) {
            rock3.gameObject.SetActive(true);
            rock1.SetTrigger("Merge");
            rock2.SetTrigger("Merge");
            rock3.SetTrigger("Merge");

            Cooldown.Wait(1).OnComplete(() => {
                rock1.gameObject.SetActive(false);
                rock2.gameObject.SetActive(false);
                rock3.gameObject.SetActive(false);
                Instantiate(stoneCompletedSound);
            });
        } else if (stones == previousStones + 1) {
            stone.SetTrigger("Gained");
            Instantiate(stoneCompletedSound);
        }

        previousStones = stones;
        prompt.SetActive(stones > 0);
    }
}
