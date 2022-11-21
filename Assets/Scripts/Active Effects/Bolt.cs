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
    float explosionDuration = 1f;
    [SerializeField]
    int explosionScale = 2;

    [SerializeField]
    GameObject explosion;

    Vector3 initialPosition;
    Vector3 direction = Vector3.forward;

    public override void UpdateGameObject() {
        transform.localScale *= areaMultiplier;
    }

    void Explode() {
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
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy) {
            enemy.Damage(damage);
        }

        if (!isPiercing || !enemy) {
            Explode();
        }
    }
}
