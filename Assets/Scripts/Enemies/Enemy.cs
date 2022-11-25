using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using CooldownManagement;

public class Enemy : MonoBehaviour
{
    private enum State {
        Chasing,
        Attacking,
        Stunned,
    }

    [SerializeField] private EnemyKilledSO enemyKilled;
    [SerializeField] private float range = 1;
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject experiencePrefab;
    [SerializeField] private GameObject deathEffect;

    private int health;

    private NavMeshAgent agent;
    private Transform player;

    Animator animator;

    private State state;

    private void Start() {
        SwitchState(State.Chasing);
        health = maxHealth;
    }

    private void OnEnable() {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().transform;
        animator = GetComponentInChildren<Animator>();

        agent.avoidancePriority = Random.Range(1, 500);
    }

    private void Update() {
        switch (state) {
            case State.Chasing:
                if (Vector3.Distance(transform.position, player.position) < range) {
                    SwitchState(State.Attacking);
                } else {
                    agent.SetDestination(player.position);
                }
                break;
        }
    }

    private void SwitchState(State newState) {
        switch (newState) {
            case State.Chasing:
                animator.SetTrigger("walking");
                break;
            case State.Attacking:
                //  TODO:
                Cooldown.Wait(1)
                    .OnComplete(() => {
                        SwitchState(State.Chasing);
                    });
                break;
        }
    }

    public void Damage(int damage) {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if (health == 0) {
            SwitchState(State.Stunned);
            enemyKilled.Kill();
            // TODO: Play effect / animation
            Instantiate(experiencePrefab, transform.position, Quaternion.identity);
            GameObject deathInstance = Instantiate(deathEffect, transform.position, transform.rotation);
            deathInstance.transform.localScale = transform.localScale;
            Destroy(deathInstance, 3);
            Destroy(gameObject);
        }
    }
}
