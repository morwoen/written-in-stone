using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillsCounter : MonoBehaviour
{
    [SerializeField] private EnemyKilledSO enemyKilled;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable() {
        enemyKilled.killed += OnKill;

        OnKill(enemyKilled.remaining);
    }

    private void OnDisable() {
        enemyKilled.killed -= OnKill;
    }

    private void OnKill(int remaining) {
        text.SetText(remaining.ToString("00.##"));
    }
}
