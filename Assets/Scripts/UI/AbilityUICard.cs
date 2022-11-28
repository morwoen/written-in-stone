using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class AbilityUICard : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    [SerializeField] private EffectImage image;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Animator animator;

    [NonSerialized] public ActiveEffectSO activeEffect;
    [NonSerialized] public PassiveEffectSO passiveEffect;
    [NonSerialized] public PassiveEffectSO.EffectRarity passiveRarity;

    public void Assign(InventorySO.ActiveInventorySlot slot) {
        image.Apply(slot);
        title.SetText(slot.effect.displayName);
        description.SetText(slot.effect.tooltip);
        activeEffect = slot.effect;
        passiveEffect = null;
        valueText.SetText("Active Ability");
    }

    public void Assign(InventorySO.PassiveInventorySlot slot) {
        image.Apply(slot);
        title.SetText(slot.effect.displayName);
        description.SetText(slot.effect.tooltip);
        activeEffect = null;
        passiveEffect = slot.effect;
        passiveRarity = slot.rarity;

        valueText.SetText(slot.ToString());
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
