using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomisePitch : MonoBehaviour
{
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);
    [SerializeField] private bool destroyWhenDone = false;

    private void Start() {
        var source = GetComponent<AudioSource>();

        source.pitch = Random.Range(pitchRange.x, pitchRange.y);

        if (destroyWhenDone) {
            float life = source.clip.length / source.pitch;
            Destroy(gameObject, life);
        }
    }
}
