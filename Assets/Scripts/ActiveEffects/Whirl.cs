using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirl : ActiveEffect
{
    [SerializeField]
    int timeToLive = 5;
    [SerializeField]
    float rangeMultiplier = 2f;
    [SerializeField]
    float damageTickDelay = 1f;

    public override void UpdateGameObject() {
        transform.localScale *= areaMultiplier;
    }

    public void DealDamage() {
        LayerMask mask = LayerMask.GetMask("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, transform.localScale.x * rangeMultiplier, mask);

        foreach (Collider hitCollider in hitColliders) {
            hitCollider.GetComponent<Enemy>()?.Damage(damage);
        }
    }

    void Awake() {
        Destroy(gameObject, timeToLive);
    }

    IEnumerator Start() {
        float ticks = timeToLive / damageTickDelay;

        for (int i = 0; i < ticks; i++) {
            DealDamage();
            yield return new WaitForSeconds(damageTickDelay);
        }
    }
}
