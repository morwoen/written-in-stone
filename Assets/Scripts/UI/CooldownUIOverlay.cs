using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CooldownManagement;

public class CooldownUIOverlay : MonoBehaviour
{
    [SerializeField] private Image overlay;

    private Cooldown cooldown;

    private void OnEnable() {
        overlay.fillAmount = 0;
    }

    public void ShowCooldown(Cooldown cooldown) {
        this.cooldown = cooldown?.OnProgress((current, total) => {
            if (overlay == null) return;
            overlay.fillAmount = 1 - (current / total);
        });
    }

    public bool IsAnimating() {
        return !!cooldown;
    }
}
