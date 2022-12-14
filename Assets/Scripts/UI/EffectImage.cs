using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EffectImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum TooltipSide {
        BottomLeft,
        BottomRight,
        TopRight,
    }

    [SerializeField] private Image rarity;
    [SerializeField] private Image primary;
    [SerializeField] private Image secondary;
    [SerializeField] private RectTransform innerMask;
    [SerializeField] private float raritySize = 10;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private RectTransform tooltipObject;

    [SerializeField] private Color activeEffectRarity;
    [SerializeField] private Color enemyActiveEffectRarity;
    [SerializeField] private Color commonRarity;
    [SerializeField] private Color uncommonRarity;
    [SerializeField] private Color rareRarity;
    [SerializeField] private Color epicRarity;
    [SerializeField] private Color legendaryRarity;

    private InventorySO.ActiveInventorySlot activeSlot;
    private InventorySO.PassiveInventorySlot passiveSlot;

    private Color transparent = new Color(1, 1, 1, 0);
    private Color opaque = new Color(1, 1, 1, 1);

    private bool tooltipEnabled = false;

    private void Awake() {
        innerMask.offsetMin = new Vector2(raritySize, raritySize);
        innerMask.offsetMax = new Vector2(-raritySize, -raritySize);

        tooltipObject.gameObject.SetActive(false);

        var secondaryTransform = secondary.GetComponent<RectTransform>();
        var iconTransform = GetComponent<RectTransform>();

        float width;
        var grid = GetComponentInParent<GridLayoutGroup>();
        if (grid) {
            width = grid.cellSize.x;
        } else {
            width = iconTransform.sizeDelta.x;
        }

        var secondarySize = width / 2 - raritySize;
        secondaryTransform.sizeDelta = new Vector2(secondarySize, secondarySize);
        var secondaryPositionOffset = width / 4;
        secondaryTransform.anchoredPosition = new Vector2(secondaryPositionOffset, secondaryPositionOffset);
    }

    public bool Is(ActiveEffectSO effect) {
        return effect == activeSlot?.effect;
    }

    public bool Is(PassiveEffectSO effect) {
        return effect == passiveSlot?.effect;
    }

    public void Apply(InventorySO.ActiveInventorySlot slot) {
        activeSlot = slot;
        passiveSlot = null;
        Refresh();
    }

    public void Apply(InventorySO.PassiveInventorySlot slot, bool showQuantity = false) {
        passiveSlot = slot;
        activeSlot = null;
        Refresh(showQuantity);
    }

    public void Refresh(bool showQuantity = false) {
        if (activeSlot != null) {
            secondary.color = transparent;
            primary.sprite = activeSlot.effect.sprite;

            var isEnemy = activeSlot.effect.traits.Contains(ActiveEffectSO.EffectTrait.Enemy);
            rarity.color = isEnemy ? enemyActiveEffectRarity : activeEffectRarity;
            quantityText.SetText("");
            tooltipText.SetText(activeSlot.effect.displayName);

            if (activeSlot.effect.secondarySprite) {
                secondary.color = opaque;
                secondary.sprite = activeSlot.effect.secondarySprite;
            } else {
                secondary.color = transparent;
            }
        } else {
            if (passiveSlot.effect.secondarySprite) {
                secondary.color = opaque;
                secondary.sprite = passiveSlot.effect.secondarySprite;
            } else {
                secondary.color = transparent;
            }

            if (showQuantity) {
                quantityText.SetText($"x{passiveSlot.permanent + passiveSlot.temporary}");
            } else {
                quantityText.SetText("");
            }

            tooltipText.SetText(passiveSlot.effect.displayName);

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

    public void SetTooltip(bool enabled = true, TooltipSide side = TooltipSide.BottomLeft) {
        tooltipEnabled = true;
        switch (side) {
            case TooltipSide.BottomLeft:
                tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Right;
                tooltipObject.anchorMin = new Vector2(0, 0);
                tooltipObject.anchorMax = new Vector2(0, 0);
                tooltipObject.anchoredPosition = new Vector2(-tooltipObject.sizeDelta.x / 2, -tooltipObject.sizeDelta.y / 2 + 20);
                break;
            case TooltipSide.BottomRight:
                tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                tooltipObject.anchorMin = new Vector2(1, 0);
                tooltipObject.anchorMax = new Vector2(1, 0);
                tooltipObject.anchoredPosition = new Vector2(tooltipObject.sizeDelta.x / 2, -tooltipObject.sizeDelta.y / 2 + 20);
                break;
            case TooltipSide.TopRight:
                tooltipText.horizontalAlignment = HorizontalAlignmentOptions.Left;
                tooltipObject.anchorMin = new Vector2(1, 1);
                tooltipObject.anchorMax = new Vector2(1, 1);
                tooltipObject.anchoredPosition = new Vector2(tooltipObject.sizeDelta.x / 2, tooltipObject.sizeDelta.y / 2 + 20);
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltipObject.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        tooltipObject.gameObject.SetActive(tooltipEnabled);
    }
}
