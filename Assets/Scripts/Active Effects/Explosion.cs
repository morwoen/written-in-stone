using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IActiveEffect
{
    float effectScale = 1f;

    [SerializeField]
    int duration = 5;

    public float EffectScale {
        get {
            return effectScale;
        }

        set {
            effectScale = value;
        }
    }

    public void SetParameters(float effectScale) {
        this.EffectScale = effectScale;
    }

    public void UpdateGameObject() {
        transform.localScale = transform.localScale * EffectScale;
    }

    void Awake() {
        Destroy(gameObject, duration);
    }
}
