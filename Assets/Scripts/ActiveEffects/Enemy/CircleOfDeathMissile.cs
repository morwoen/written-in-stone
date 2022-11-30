using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleOfDeathMissile : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private float velocity;

    private int damage;

    private void Update() {
        transform.Translate(velocity * Time.deltaTime * transform.forward, Space.World);
    }

    private void OnCollisionEnter(Collision collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player) {
            player.Damage(damage);
        }

        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SetDamage(int damage) {
        this.damage = damage;
    }
}