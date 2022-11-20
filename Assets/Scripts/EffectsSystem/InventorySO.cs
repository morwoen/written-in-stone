using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "ScriptableObjects/Inventory")]
public class InventorySO : ScriptableObject
{
    [NonSerialized]
    public static int rocksPerStone = 3;

    public int maxActiveSlots = 6;
    public List<ActiveInventorySlot> active = new List<ActiveInventorySlot>();
    public List<PassiveInventorySlot> passive = new List<PassiveInventorySlot>();
    public int stones;
    public int rocks;

    public delegate void StonesChange(int stones, int rocks);
    public event StonesChange stonesChange;
    public delegate void EffectsChange(List<ActiveInventorySlot> active, List<PassiveInventorySlot> passive);
    public event EffectsChange effectsChange;
    public delegate void EffectRemoved(ActiveEffectSO active, PassiveEffectSO passive);
    public event EffectRemoved effectRemoved;
    public delegate void EffectAdded(ActiveEffectSO active, PassiveEffectSO passive);
    public event EffectAdded effectAdded;

    private void OnEnable() {
        active.RemoveAll(x => true);
        passive.RemoveAll(x => true);
        stones = 0;
        rocks = 0;
    }

    public void AddStone() {
        stones += 1;
        stonesChange?.Invoke(stones, rocks);
    }

    public void AddRock() {
        rocks += 1;
        if (rocks == rocksPerStone) {
            rocks = 0;
            stones += 1;
        }
        stonesChange?.Invoke(stones, rocks);
    }

    public void RemoveStone() {
        if (stones == 0) {
            Debug.LogError("Remove stones when 0 available");
            return;
        }

        stones -= 1;
        stonesChange?.Invoke(stones, rocks);
    }

    public ActiveInventorySlot GetSlot(ActiveEffectSO effect) {
        return active.FirstOrDefault(slot => slot.effect == effect);
    }

    public PassiveInventorySlot GetSlot(PassiveEffectSO effect, PassiveEffectSO.EffectRarity rarity) {
        return passive.FirstOrDefault(slot => slot.effect == effect && slot.rarity == rarity);
    }

    public PassiveInventorySlot GetSlot(PassiveEffectSO effect) {
        return passive.FirstOrDefault(slot => slot.effect == effect);
    }

    public int GetPassiveMultiplier(PassiveEffectSO.EffectProperty effectProperty) {
        return passive
            .Select(slot => slot.details.effectedProperties.FirstOrDefault(prop => prop.property == effectProperty)?.modifier ?? 0)
            .Sum();
    }

    public int GetPassiveMultiplier(PassiveEffectSO.EffectProperty effectProperty, ActiveEffectSO.EffectTrait[] traits) {
        return passive
            .Where(slot => slot.effect.traits.Union(traits).Count() > 0)
            .Select(slot => slot.details.effectedProperties.FirstOrDefault(prop => prop.property == effectProperty)?.modifier ?? 0)
            .Sum();
    }

    public void RemoveTemporary() {
        var activeSlotsToRemove = active.Where(slot => slot.temporary > 0)
            .Where(slot => {
                if (slot.permanent > 0) {
                    slot.temporary = 0;
                    return false;
                } else {
                    return true;
                }
            })
            .Select(slot => slot.effect);

        active.RemoveAll(slot => activeSlotsToRemove.Contains(slot.effect));

        var passiveSlotsToRemove = passive.Where(slot => slot.temporary > 0)
            .Where(slot => {
                if (slot.permanent > 0) {
                    slot.temporary = 0;
                    return false;
                } else {
                    return true;
                }
            })
            .Select(slot => Tuple.Create(slot.effect, slot.rarity));

        passive.RemoveAll(slot => passiveSlotsToRemove.Contains(Tuple.Create(slot.effect, slot.rarity)));

        effectsChange?.Invoke(active, passive);

        foreach (var activeRemoved in activeSlotsToRemove) {
            effectRemoved?.Invoke(activeRemoved, null);
        }
        foreach (var passiveRemoved in passiveSlotsToRemove) {
            effectRemoved?.Invoke(null, passiveRemoved.Item1);
        }
    }

    public void AddTemporary(ActiveEffectSO effect) {
        var slot = GetSlot(effect);
        if (slot != null) {
            if (slot.permanent + slot.temporary < slot.effect.levels.Length) {
                slot.temporary += 1;
            } else {
                Debug.LogError($"Trying to add too many levels on effect {effect.name}");
            }
        } else {
            if (active.Count < maxActiveSlots) {
                active.Add(new ActiveInventorySlot(effect, 0, 1));
                effectAdded?.Invoke(effect, null);
            } else {
                Debug.LogError($"Trying to add too many active effects. Last one - {effect.name}");
            }
        }

        effectsChange?.Invoke(active, passive);
    }

    public void AddTemporary(PassiveEffectSO effect, PassiveEffectSO.EffectRarity rarity) {
        var slot = GetSlot(effect, rarity);
        if (slot != null) {
            slot.temporary += 1;
        } else {
            passive.Add(new PassiveInventorySlot(effect, rarity, 0, 1));
            effectAdded?.Invoke(null, effect);
        }

        effectsChange?.Invoke(active, passive);
    }

    public void Promote(ActiveEffectSO effect) {
        var slot = GetSlot(effect);
        if (slot == null) {
            Debug.LogError("Promoting missing effect");
            return;
        }

        if (slot.temporary < 1) {
            Debug.LogError("Promoting effect without temporary stacks");
            return;
        }

        slot.temporary -= 1;
        slot.permanent += 1;

        effectsChange?.Invoke(active, passive);
    }

    [Serializable]
    public class InventorySlot
    {
        public int permanent;
        public int temporary;

        public InventorySlot(int permanent, int temporary) {
            this.permanent = permanent;
            this.temporary = temporary;
        }
    }

    [Serializable]
    public class ActiveInventorySlot : InventorySlot
    {
        public ActiveEffectSO effect;

        public ActiveInventorySlot(ActiveEffectSO effect, int permanent, int temporary) : base(permanent, temporary) {
            this.effect = effect;
        }
    }

    [Serializable]
    public class PassiveInventorySlot : InventorySlot
    {
        public PassiveEffectSO effect;
        public PassiveEffectSO.EffectRarity rarity;
        public PassiveEffectSO.EffectProperty[] properties;
        public PassiveEffectSO.EffectDetails details;

        public PassiveInventorySlot(PassiveEffectSO effect, PassiveEffectSO.EffectRarity rarity, int permanent, int temporary) : base(permanent, temporary) {
            this.effect = effect;
            this.rarity = rarity;
            this.details = effect.rarities.FirstOrDefault(e => e.rarity == rarity);
            this.properties = this.details.effectedProperties.Select(p => p.property).ToArray();
        }
    }
}
