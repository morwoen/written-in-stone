using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] AudioClip attackSound;

    protected int damage = 10;
    private bool damaged = false;

    protected AudioSource audioSource;

    protected void OnEnable() {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetDamage(int damage) {
        this.damage = damage;
    }

    public void PlayAttackSound() {
        audioSource.PlayOneShot(attackSound);
    }

    private void OnTriggerEnter(Collider collider){
        if (damaged) return;
        var player = collider.GetComponent<PlayerController>();
        player.Damage(damage);
        damaged = true;
    }
}
