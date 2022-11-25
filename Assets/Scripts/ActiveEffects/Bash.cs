using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bash : ActiveEffect
{
    [SerializeField]
    int timeToLive = 5;
    [SerializeField]
    float rangeMultiplier = 2f;
    [SerializeField]
    float distance = 1f;
    [SerializeField]
    int explosionScale = 4;
    [SerializeField]
    float explosionDuration = 1f;

    [SerializeField]
    GameObject explosion;

    void Awake() {
        Destroy(gameObject, timeToLive);
    }

    public override void UpdateGameObject() {
        transform.localScale *= areaMultiplier;
        distance *= areaMultiplier;
    }

    public void DealDamage() {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * distance, transform.localScale.x * rangeMultiplier, mask);

        foreach (Collider hitCollider in hitColliders) {
            hitCollider.GetComponent<Enemy>().Damage(damage);
        }

        Explode();
    }

    void Explode() {
        GameObject explosionInstance = Instantiate(explosion, transform.position + transform.forward * distance, Quaternion.identity);
        explosionInstance.transform.position = explosionInstance.transform.position.WithY(0);
        explosionInstance.transform.localScale *= explosionScale * areaMultiplier;
        Destroy(explosionInstance, explosionDuration);
    }
}
