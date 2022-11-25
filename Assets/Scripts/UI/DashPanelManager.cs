using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownManagement;

public class DashPanelManager : MonoBehaviour
{
    [SerializeField] private DashSO dash;
    [SerializeField] private GameObject dashUIElementPrefab;

    private void OnEnable() {
        dash.used += OnDashUsed;
        dash.maxChargesChanged += OnMaxDashChargesChanged;
        OnMaxDashChargesChanged(dash.MaxCharges);
    }

    private void OnDisable() {
        dash.used -= OnDashUsed;
        dash.maxChargesChanged -= OnMaxDashChargesChanged;
    }

    private void OnDashUsed(Cooldown cooldown) {
        for (var i = 0; i < transform.childCount; i++) {
            var uiElement = transform.GetChild(i).GetComponent<DashUIElement>();
            if (!uiElement.IsAnimating()) {
                uiElement.ShowCooldown(cooldown);
                break;
            }
        }
    }

    private void OnMaxDashChargesChanged(int charges) {
        if (charges > transform.childCount) {
            var neededElements = charges - transform.childCount;
            for (var i = 0; i < neededElements; i++) {
                Instantiate(dashUIElementPrefab, transform);
            }
        } else if (charges < transform.childCount) {
            var extraElements = transform.childCount - charges;
            for (var i = 0; i < extraElements; i++) {
                Destroy(transform.GetChild(transform.childCount - i - 1));
            }
        }
    }
}
