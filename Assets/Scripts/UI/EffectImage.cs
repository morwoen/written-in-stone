using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectImage : MonoBehaviour
{
    [SerializeField] private Image rarity;
    [SerializeField] private Image primary;
    [SerializeField] private Image secondary;
    [SerializeField] private RectTransform innerMask;
    [SerializeField] private float raritySize = 10;

    [SerializeField] private Color activeEffectRarity;
    [SerializeField] private Color commonRarity;
    [SerializeField] private Color uncommonRarity;
    [SerializeField] private Color rareRarity;
    [SerializeField] private Color epicRarity;
    [SerializeField] private Color legendaryRarity;

    private InventorySO.ActiveInventorySlot activeSlot;
    private InventorySO.PassiveInventorySlot passiveSlot;

    private Color transparent = new Color(1, 1, 1, 0);
    private Color opaque = new Color(1, 1, 1, 1);

    private void Awake() {
        innerMask.offsetMin = new Vector2(raritySize, raritySize);
        innerMask.offsetMax = new Vector2(-raritySize, -raritySize);

        var secondaryTransform = secondary.GetComponent<RectTransform>();
        var iconTransform = GetComponent<RectTransform>();

        var width = iconTransform.sizeDelta.x;
        if (width == 0) {
            width = GetComponentInParent<GridLayoutGroup>().cellSize.x;
        }

        var secondarySize = width / 2 - raritySize;
        secondaryTransform.sizeDelta = new Vector2(secondarySize, secondarySize);
        var secondaryPositionOffset = width / 4;
        secondaryTransform.anchoredPosition = new Vector2(secondaryPositionOffset, secondaryPositionOffset);
    }

    public bool Is(ActiveEffectSO effect) {
        return effect == activeSlot.effect;
    }

    public bool Is(PassiveEffectSO effect) {
        return effect == passiveSlot.effect;
    }

    public void Apply(InventorySO.ActiveInventorySlot slot) {
        activeSlot = slot;
        passiveSlot = null;
        Refresh();
    }

    public void Apply(InventorySO.PassiveInventorySlot slot) {
        passiveSlot = slot;
        activeSlot = null;
        Refresh();
    }

    public void Refresh() {
        if (activeSlot != null) {
            secondary.color = transparent;
            primary.sprite = activeSlot.effect.sprite;
            rarity.color = activeEffectRarity;
        } else {
            secondary.color = opaque;
            secondary.sprite = passiveSlot.effect.secondarySprite;
            primary.sprite = passiveSlot.effect.sprite;
            switch (passiveSlot.rarity) {
                case PassiveEffectSO.EffectRarity.Common:
                    rarity.color = commonRarity;
                    break;
                case PassiveEffectSO.EffectRarity.Uncommon:
                    rarity.color = uncommonRarity;
                    break;
                case PassiveEffectSO.EffectRarity.Rare:
                    rarity.color = rareRarity;
                    break;
                case PassiveEffectSO.EffectRarity.Epic:
                    rarity.color = epicRarity;
                    break;
                case PassiveEffectSO.EffectRarity.Legendary:
                    rarity.color = legendaryRarity;
                    break;
            }
        }
    }
}
