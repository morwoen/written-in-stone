using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeteor : EnemyAttack
{
    [SerializeField] int spawnHeight;
    [SerializeField] int velocity;
    [SerializeField] float explosionDuration;
    [SerializeField] float markerDuration;
    [SerializeField] float explosionScale;
    [SerializeField] float markerScale;

    [SerializeField] GameObject explosion;
    [SerializeField] Transform marker;

    void Awake() {
        PlayerController player = FindObjectOfType<PlayerController>();

        if (player != null ) {
            transform.position = player.transform.position.WithY(spawnHeight);

            Transform markerInstance = Instantiate(marker, transform.position.WithY(0), Quaternion.identity);
            markerInstance.transform.localScale *= markerScale;
            Destroy(markerInstance.gameObject, markerDuration);
        }
    }

    void Update() {
        transform.Translate(Vector3.down * velocity * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        LayerMask mask = LayerMask.GetMask("Player");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x * explosionScale, mask);

        foreach (Collider hitCollider in hitColliders) {
            hitCollider.GetComponent<PlayerController>().Damage(damage);
        }

        Explode();
    }

    void Explode() {
        GameObject explosionInstance = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionInstance.transform.localScale *= explosionScale;
        Destroy(explosionInstance, explosionDuration);
        Destroy(gameObject);
    }
}
