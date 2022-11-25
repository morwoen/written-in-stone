using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Effect", menuName = "ScriptableObjects/Passive Effect")]
public class PassiveEffectSO : ScriptableObject
{
    public enum EffectRarity {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
    }

    public enum EffectProperty
    {
        MovementSpeed,
        DodgeCount,
        Defence,
        Health,
        PickupRange,
        Experience,
        Multicast,
        SpellDamage,
        SpellCooldown,
        SpellArea,
        SpellCritDamage,
        SpellCritChance,
        SpellMulticast,
    }

    public Sprite sprite;
    public Sprite secondarySprite;
    public string displayName;
    public string tooltip;
    public ActiveEffectSO.EffectTrait[] traits;

    public EffectDetails[] rarities = new[] {
        new EffectDetails() {
            rarity = EffectRarity.Common,
            effectedProperties = new[] {
                new EffectedProperty() {
                    modifier = 5,
                }
            }
        },
        new EffectDetails() {
            rarity = EffectRarity.Uncommon,
            effectedProperties = new[] {
                new EffectedProperty() {
                    modifier = 10,
                }
            }
        },
        new EffectDetails() {
            rarity = EffectRarity.Rare,
            effectedProperties = new[] {
                new EffectedProperty() {
                    modifier = 20,
                }
            }
        },
        new EffectDetails() {
            rarity = EffectRarity.Epic,
            effectedProperties = new[] {
                new EffectedProperty() {
                    modifier = 40,
                }
            }
        },
        new EffectDetails() {
            rarity = EffectRarity.Legendary,
            effectedProperties = new[] {
                new EffectedProperty() {
                    modifier = 60,
                }
            }
        }
    };

    [Serializable]
    public class EffectDetails
    {
        public EffectRarity rarity;
        public EffectedProperty[] effectedProperties;
    }

    [Serializable]
    public class EffectedProperty
    {
        public EffectProperty property;
        public int modifier;
    }
}
