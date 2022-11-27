using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Experience", menuName = "ScriptableObjects/Experience")]
public class ExperienceSO : ScriptableObject
{
    private int level = 1;
    private int requiredExperience;
    private int currentExperience;

    public delegate void ExperienceChanged(int level, bool levelUp, int current, int required);
    public event ExperienceChanged change;

    private void OnEnable() {
        Restart();
    }

    public void Restart() {
        level = 1;
        currentExperience = 0;
        requiredExperience = RequiredExperience(level);
    }

    private int RequiredExperience(int level) {
        if (level < 50) {
            return 100 + level * 50;
        }
        return 100 + Mathf.CeilToInt(Mathf.Pow(level, 2));
    }

    public void AddExperience(int experience) {
        currentExperience += experience;

        var levelUp = false;

        if (currentExperience >= requiredExperience) {
            currentExperience -= requiredExperience;
            level += 1;
            requiredExperience = RequiredExperience(level);
            levelUp = true;
        }

        change?.Invoke(level, levelUp, currentExperience, requiredExperience);
    }
}
