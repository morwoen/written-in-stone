using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private HealthSO playerHealth;

    private float timeLapsed = 0;
    private int minutes;
    private int seconds;

    private bool dead = false;

    private void OnEnable() {
        playerHealth.death += OnPlayerDeath;
    }

    private void OnDisable() {
        playerHealth.death -= OnPlayerDeath;
    }

    private void OnPlayerDeath() {
        dead = true;
    }

    public string TimeString() {
        return $"{minutes.ToString("00.##")}:{seconds.ToString("00.##")}";
    }

    private void Update() {
        if (!InputManager.Instance.Player.enabled || dead) return;

        timeLapsed += Time.deltaTime;

        if (timeLapsed > 1) {
            timeLapsed -= 1;
            seconds += 1;
            if (seconds >= 60) {
                seconds -= 60;
                minutes += 1;
            }
            text.SetText(TimeString());
        }
    }
}
