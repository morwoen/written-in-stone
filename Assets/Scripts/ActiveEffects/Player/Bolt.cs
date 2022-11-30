using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bolt : ActiveEffect
{
    [SerializeField]
    int range = 5;
    [SerializeField]
    int velocity = 1;
    [SerializeField]
    bool isPiercing = false;
    [SerializeField]
    float rangeMultiplier = 2f;
    [SerializeField]
    float explosionDuration = 1f;
    [SerializeField]
    int explosionScale = 2;

    [SerializeField]
    GameObject explosion;

    private bool exploded = false;

    Vector3 initialPosition;
    Vector3 direction = Vector3.forward;

    public override void UpdateGameObject() {
        BoxCollider collider = GetComponent<BoxCollider>();
        transform.localScale *= areaMultiplier;
        collider.size = collider.size.WithY(collider.size.y / areaMultiplier);
    }

    void Explode() {
        exploded = true;
        GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionInstance.transform.localScale *= explosionScale * areaMultiplier;
        Destroy(explosionInstance, explosionDuration);
        Destroy(gameObject);
    }

    void Awake() {
        initialPosition = transform.position;
    }

    void Update() {
        transform.Translate(direction * velocity * Time.deltaTime);

        if (Vector3.Distance(initialPosition, transform.position) >= range) {
            Explode();
        }
    }

    void OnTriggerEnter(Collider other) {
        if (exploded) return;

        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy) {
            LayerMask mask = LayerMask.GetMask("Enemy");
            Collider[] hitColliders = Physics.OverlapSphere(enemy.transform.position, transform.localScale.x * rangeMultiplier, mask);

            foreach (Collider hitCollider in hitColliders) {
                hitCollider.GetComponent<Enemy>()?.Damage(damage);
            }
        }

        if (!isPiercing || !enemy) {
            Explode();
        }
    }
}
