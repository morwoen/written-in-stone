using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    int range = 5;
    [SerializeField]
    int velocity = 1;
    Vector3 initialPosition;
    Vector3 direction = Vector3.forward;

    void Awake() {
        initialPosition = transform.position;
    }

    void Update() {
        transform.Translate(direction * velocity * Time.deltaTime);

        if (Vector3.Distance(initialPosition, transform.position) >= range) {
            Destroy(gameObject);
        }
    }
}