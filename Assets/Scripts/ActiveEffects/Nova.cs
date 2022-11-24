using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nova : ActiveEffect
{
    [SerializeField]
    int timeToLive = 5;
    [SerializeField]
    float rangeMultiplier = 2f;

    public override void UpdateGameObject() {
        transform.localScale *= areaMultiplier;
    }

    public void DealDamage() {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x * rangeMultiplier, mask);

        foreach (Collider hitCollider in hitColliders) {
            hitCollider.GetComponent<Enemy>().Damage(damage);
        }
    }

    void Awake() {
        Destroy(gameObject, timeToLive);
    }
}
