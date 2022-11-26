using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private float timeLapsed = 0;
    private int minutes;
    private int seconds;

    private void Update() {
        if (!InputManager.Instance.Player.enabled) return;

        timeLapsed += Time.deltaTime;

        if (timeLapsed > 1) {
            timeLapsed -= 1;
            seconds += 1;
            if (seconds >= 60) {
                seconds -= 60;
                minutes += 1;
            }
            text.SetText($"{minutes.ToString("00.##")}:{seconds.ToString("00.##")}");
        }
    }
}
