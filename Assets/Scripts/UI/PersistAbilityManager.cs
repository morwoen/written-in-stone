using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PersistAbilityManager : MonoBehaviour
{
    [SerializeField] private AbilityUICard card1;
    [SerializeField] private AbilityUICard card2;
    [SerializeField] private AbilityUICard card3;

    [SerializeField] private GameObject content;

    [SerializeField] private InventorySO inventory;

    private bool isOpen = true;

    private void Awake() {
        Hide();
    }

    private void OnEnable() {
        InputManager.Instance.Player.Interact.performed += OnInteract;
        InputManager.Instance.Menu.ClosePauseMenu.performed += OnMenuClosed;
    }

    private void OnDisable() {
        InputManager.Instance.Player.Interact.performed -= OnInteract;
        InputManager.Instance.Menu.ClosePauseMenu.performed -= OnMenuClosed;
    }

    private void OnInteract(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        Show();
    }

    private void OnMenuClosed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        Hide();
    }

    public void Show() {
        if (isOpen || inventory.stones < 1) return;

        InputManager.Instance.SwitchTo(InputManager.Instance.Menu);

        content.SetActive(true);

        var activeSpell = inventory.active
            .Where(slot => slot.temporary > 0)
            .FirstOrDefault();

        var passiveSpells = inventory.passive
            .Where(slot => slot.temporary > 0)
            .GetEnumerator();

        if (activeSpell != null) {
            card1.Assign(activeSpell);
        } else {
            passiveSpells.MoveNext();
            card1.Assign(passiveSpells.Current);
        }

        passiveSpells.MoveNext();
        card2.Assign(passiveSpells.Current);

        passiveSpells.MoveNext();
        card3.Assign(passiveSpells.Current);

        EventSystem.current.SetSelectedGameObject(card1.gameObject);

        Time.timeScale = 0;

        isOpen = true;
    }

    public void Hide() {
        if (!isOpen) return;

        InputManager.Instance.SwitchTo(InputManager.Instance.Player);

        content.SetActive(false);

        Time.timeScale = 1;

        isOpen = false;
    }

    public void CardClicked(int card) {
        var current = card1;
        if (card == 2) {
            current = card2;
        } else if (card == 3) {
            current = card3;
        }

        if (current.activeEffect) {
            inventory.Promote(current.activeEffect);
        } else {
            inventory.Promote(current.passiveEffect, current.passiveRarity);
        }

        inventory.RemoveStone();

        Hide();
    }
}
