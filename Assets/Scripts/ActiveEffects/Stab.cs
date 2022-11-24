using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stab : ActiveEffect
{
    [SerializeField]
    float range = 5;
    [SerializeField]
    int velocity = 1;

    Vector3 initialPosition;
    Vector3 direction = Vector3.forward;

    public override void UpdateGameObject() {
        transform.localScale *= areaMultiplier;
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

    private void OnTriggerEnter(Collider other) {
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy) {
            enemy.GetComponent<Enemy>().Damage(damage);
        }
    }
}
