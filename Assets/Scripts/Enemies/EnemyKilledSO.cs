using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyKilled", menuName = "ScriptableObjects/EnemyKilled")]
public class EnemyKilledSO : ScriptableObject
{
    public int killsUntilEffectsSwap = 10;
    public int remaining = 10;
    private int total = 0;

    public delegate void EnemyKilled(int remaining, int total);
    public event EnemyKilled killed;

    private void OnEnable() {
        remaining = killsUntilEffectsSwap;
    }

    public void Kill() {
        remaining -= 1;
        total += 1;

        killed?.Invoke(remaining, total);

        if (remaining <= 0) {
            remaining = killsUntilEffectsSwap;
            killed?.Invoke(remaining, total);
        }
    }
}
