using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    protected int damage = 10;

    public void SetDamage(int damage) {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider collider){
        var player = collider.GetComponent<PlayerController>();
        player.Damage(damage);
    }
}
