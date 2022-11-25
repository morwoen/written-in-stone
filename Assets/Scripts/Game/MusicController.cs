using CooldownManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    AudioClip start;
    [SerializeField]
    AudioClip loop;
    [SerializeField]
    float delay;

    AudioSource source;

    void Awake() {
        source = GetComponent<AudioSource>();
    }

    IEnumerator Start() {
        source.clip = start;
        source.loop = false;
        source.Play();

        yield return new WaitForSeconds(start.length - delay);

        source.clip = loop;
        source.loop = true;
        source.Play();
    }
}
