using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class VolumeManager : MonoBehaviour
{
    public static string VOLUME_KEY = "MasterVolume";

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeNumber;
    [SerializeField] private AudioMixer audioMixer;

    private void Start() {
        var value = PlayerPrefs.GetFloat(VOLUME_KEY, 0.8f);
        volumeSlider.value = value;
        UpdateVolume();
    }

    public void UpdateVolume() {
        var value = volumeSlider.value;
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        volumeNumber.SetText(((int)(value * 100)).ToString());
        audioMixer.SetFloat(VOLUME_KEY, Mathf.Log10(value.Remap(0, 1, 0.0001f, 1)) * 20);
    }
}
