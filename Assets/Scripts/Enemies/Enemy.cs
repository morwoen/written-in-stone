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
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float attackRecovery= 1;
    [SerializeField] private float attackAngle = 20;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private EnemyAttack attackPrefab;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private float deathEffectScale;

    private Cooldown cooldown;

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
        agent.stoppingDistance = range;
    }

    private void OnDestroy() {
        cooldown?.Stop();
    }

    private void Update() {
        animator.SetFloat("Speed", agent.velocity.magnitude);
        switch (state) {
            case State.Chasing:
                var angle = Vector3.Angle(transform.forward, player.position - transform.position);
                var distance = Vector3.Distance(transform.position, player.position);

                if (distance < range && angle <= attackAngle) {
                    SwitchState(State.Attacking);
                } else if (distance > range) {
                    agent.SetDestination(player.position);
                } else {
                    RotateTowards(player);
                }
                break;
        }
    }

    private void SwitchState(State newState) {
        state = newState;
        switch (newState) {
            case State.Chasing:
                break;
            case State.Attacking:
                agent.SetDestination(transform.position);
                cooldown = Cooldown.Wait(attackDelay)
                    .OnComplete(() => {
                        animator.SetTrigger("Attack");
                        var attack = Instantiate(attackPrefab, transform.position, transform.rotation, transform);
                        attack.SetDamage(attackDamage);

                        cooldown = Cooldown.Wait(attackRecovery)
                            .OnComplete(() => {
                                SwitchState(State.Chasing);
                            });
                    });
                break;
        }
    }

    public void Damage(int damage) {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if (health == 0) {
            SwitchState(State.Stunned);
            enemyKilled.Kill();
            Instantiate(experiencePrefab, transform.position, Quaternion.identity);
            GameObject deathInstance = Instantiate(deathEffect, transform.position, transform.rotation);
            deathInstance.transform.localScale *= deathEffectScale;
            Destroy(deathInstance, 3);
            Destroy(gameObject);
        }
    }

    public void SetMultiplier(float multi) {
        this.attackDamage = Mathf.FloorToInt(this.attackDamage * multi);
        this.maxHealth = Mathf.FloorToInt(maxHealth * multi);
        this.health = Mathf.FloorToInt(maxHealth * multi);
    }

    private void RotateTowards(Transform target) {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction.WithY(0));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8);
    }
}
