using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CooldownManagement;

[CreateAssetMenu(fileName = "New Effect Cooldowns Tracker", menuName = "ScriptableObjects/Effect Cooldowns Tracker")]
public class EffectCooldownsTrackerSO : ScriptableObject
{
    public delegate void CooldownAdded(ActiveEffectSO effect, Cooldown cooldown);
    public event CooldownAdded cooldownAdded;

    private Dictionary<ActiveEffectSO, Cooldown> tracker;

    private void OnEnable() {
        tracker = new();
    }

    public void RegisterCooldown(ActiveEffectSO effect, Cooldown cooldown) {
        tracker[effect] = cooldown;
        cooldownAdded?.Invoke(effect, cooldown);
    }

    public void RemoveCooldown(ActiveEffectSO effect) {
        if (tracker.ContainsKey(effect)) {
            tracker[effect].Stop();
            tracker.Remove(effect);
        }
    }

    public bool HasCooldown(ActiveEffectSO effect) {
        return tracker.ContainsKey(effect);
    }

    public Cooldown GetCooldown(ActiveEffectSO effect) {
        return tracker.GetValueOrDefault(effect);
    }

    public void StopAll() {
        foreach (var cd in tracker.Values) {
            cd?.Stop();
        }
        tracker.Clear();
    }
}
