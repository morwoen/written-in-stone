using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Active Effect", menuName = "ScriptableObjects/Active Effect")]
public class ActiveEffectSO : ScriptableObject
{
    public enum EffectTrait
    {
        Bolt,
        Nova,
        Meteor,

        Stab,
        Whirl,
        Bash,

        Fire,
        Ice,
        Thunder,

        Axe,
        Spear,
        Sword,
    }

    public Sprite sprite;
    public string displayName;
    public string tooltip;
    public bool spawnOnPlayer = true;
    public EffectTrait[] traits;

    public EffectLevel[] levels = new[] {
        new EffectLevel(),
    };

    [Serializable]
    public class EffectLevel
    {
        public GameObject spellPrefab;
        public float cooldown;
        public int damage;
    }
}
