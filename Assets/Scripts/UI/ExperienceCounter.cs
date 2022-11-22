using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider slider;

    [SerializeField] private ExperienceSO experience;

    private void OnEnable() {
        experience.change += OnExperienceChange;

        OnExperienceChange(1, false, 0, 100);
    }

    private void OnDisable() {
        experience.change -= OnExperienceChange;
    }

    private void OnExperienceChange(int level, bool levelUp, int current, int required) {
        levelText.SetText(level.ToString());
        slider.value = ((float)current) / required;
    }
}
