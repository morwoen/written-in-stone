using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownManagement;

public class TemporaryEffectsPanel : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;
    [SerializeField] private EffectImage image1;
    [SerializeField] private EffectImage image2;
    [SerializeField] private EffectImage image3;

    private Cooldown image1Cooldown;
    private Cooldown image2Cooldown;
    private Cooldown image3Cooldown;

    private void OnEnable() {
        image1.SetTooltip(true, EffectImage.TooltipSide.TopRight);
        image2.SetTooltip(true, EffectImage.TooltipSide.TopRight);
        image3.SetTooltip(true, EffectImage.TooltipSide.TopRight);

        inventory.effectsChange += OnEffectsChange;
    }

    private void OnDisable() {
        inventory.effectsChange -= OnEffectsChange;
    }

    private void OnEffectsChange(List<InventorySO.ActiveInventorySlot> active, List<InventorySO.PassiveInventorySlot> passive) {
        var temporaryActive = active.FirstOrDefault(slot => slot.temporary > 0);

        var temporaryPassive = passive.Where(slot => slot.temporary > 0).ToList();

        if (temporaryActive != null && !image1.Is(temporaryActive.effect)) {
            image1.Apply(temporaryActive);
            image1.GetComponent<Animator>().SetTrigger("Show");
        }


        var nextImage = temporaryActive == null ? image1 : image2;
        var nextCooldown = temporaryActive == null ? image1Cooldown : image2Cooldown;
        if (temporaryPassive.Count > 0 && !nextImage.Is(temporaryPassive[0].effect)) {
            nextImage.Apply(temporaryPassive[0]);
            nextImage.GetComponent<Animator>().SetTrigger("Show");
        }

        nextImage = temporaryActive == null ? image2 : image3;
        nextCooldown = temporaryActive == null ? image2Cooldown : image3Cooldown;
        if (temporaryPassive.Count > 1 && !nextImage.Is(temporaryPassive[1].effect)) {
            nextImage.Apply(temporaryPassive[1]);
            nextImage.GetComponent<Animator>().SetTrigger("Show");
        }

        if (temporaryPassive.Count > 2 && !image3.Is(temporaryPassive[2].effect)) {
            image3.Apply(temporaryPassive[2]);
            image3.GetComponent<Animator>().SetTrigger("Show");
        }
    }
}
