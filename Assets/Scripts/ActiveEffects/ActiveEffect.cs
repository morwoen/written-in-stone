using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveEffect : MonoBehaviour
{
    [SerializeField] protected AudioClip effectSound;

    protected int damage = 1;
    protected float areaMultiplier = 1;

    private AudioSource audioSource;

    public void SetParameters(int damage, float areaMultiplier) {
        this.damage = damage;
        this.areaMultiplier = areaMultiplier;

        UpdateGameObject();
    }

    public void PlayEffectSound() {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(effectSound);
    }

    public abstract void UpdateGameObject();
}
