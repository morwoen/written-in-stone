using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effects Database", menuName = "ScriptableObjects/Effects Database")]
public class EffectsDatabaseSO : ScriptableObject
{
    public ActiveEffectSO[] active;
    public PassiveEffectSO[] passive;
    public ActiveEffectSO[] enemyActive;
    public PassiveEffectSO[] enemyPassive;
}
