using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class AbilityUICard : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Animator animator;

    [NonSerialized] public ActiveEffectSO activeEffect;
    [NonSerialized] public PassiveEffectSO passiveEffect;

    public void Assign(InventorySO.ActiveInventorySlot slot) {
        image.sprite = slot.effect.sprite;
        title.SetText(slot.effect.displayName);
        description.SetText(slot.effect.tooltip);
        activeEffect = slot.effect;
        passiveEffect = null;
    }

    public void Assign(InventorySO.PassiveInventorySlot slot) {
        image.sprite = slot.effect.sprite;
        title.SetText(slot.effect.displayName);
        description.SetText(slot.effect.tooltip);
        activeEffect = null;
        passiveEffect = slot.effect;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnSelect(BaseEventData eventData) {
        animator.SetBool("Focus", true);
    }

    public void OnDeselect(BaseEventData eventData) {
        animator.SetBool("Focus", false);
    }
}
