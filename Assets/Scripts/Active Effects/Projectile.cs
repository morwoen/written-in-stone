using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : ActiveEffect
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

    public override void UpdateGameObject() {
        transform.localScale *= areaMultiplier;
    }

    void Explode() {
        Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<Explosion>();
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
