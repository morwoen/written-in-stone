using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CooldownManagement;

public class DashUIElement : MonoBehaviour
{
    [SerializeField] private Image overlay;

    private Cooldown cooldown;

    private void OnEnable() {
        overlay.fillAmount = 0;
    }

    public void ShowCooldown(Cooldown cooldown) {
        this.cooldown = cooldown.OnProgress((current, total) => {
            overlay.fillAmount = 1 - (current / total);
        });
    }

    public bool IsAnimating() {
        return !!cooldown;
    }
}
