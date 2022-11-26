using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : EnemyAttack
{
    [SerializeField] int velocity = 1;

    Vector3 direction = Vector3.forward;

    void Awake() {
        transform.parent = null;

    }

    void Update() {
        transform.Translate(direction * velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player) {
            player.Damage(damage);
        }

        Destroy(gameObject);
    }
}
