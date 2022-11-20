using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour, IActiveEffect
{
    [SerializeField]
    int range = 5;
    [SerializeField]
    int velocity = 1;
    [SerializeField]
    bool isPiercing = false;

    [SerializeField]
    GameObject explosion;

    Vector3 initialPosition;
    Vector3 direction = Vector3.forward;

    float effectScale = 1f;

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
        BoxCollider collider = transform.GetComponent<BoxCollider>();
        collider.size = collider.size * EffectScale;
    }

    void Awake() {
        initialPosition = transform.position;
    }

    void Update() {
        transform.Translate(direction * velocity * Time.deltaTime);

        if (Vector3.Distance(initialPosition, transform.position) >= range) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (!isPiercing && !other.CompareTag("Player")) {
            Explosion explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
            explosionInstance.SetParameters(EffectScale);
            explosionInstance.UpdateGameObject();
            Destroy(gameObject);
        }
    }
}
