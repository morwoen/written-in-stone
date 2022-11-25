using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter(Collider collider){
        var player = collider.GetComponent<PlayerController>();
        player.Damage(damage);
    }

    private void OnDrawGizmos() {
        var coll = GetComponent<BoxCollider>();
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + coll.center, coll.size);
    }
}
