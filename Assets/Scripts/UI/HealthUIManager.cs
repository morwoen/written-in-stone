using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private HealthSO health;

    private float desiredValue;

    private void OnEnable() {
        health.change += OnHealthChange;

        OnHealthChange(health.CurrentHealth, health.MaxHealth);
    }

    private void OnDisable() {
        health.change -= OnHealthChange;
    }

    private void OnHealthChange(int current, int max) {
        desiredValue = current / (float)max;
    }

    private void Update() {
        if (slider.value > desiredValue) {
            slider.value = desiredValue;
        } else {
            slider.value = Mathf.Lerp(slider.value, desiredValue, .5f);
        }
    }
}
