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
    [SerializeField] private GameObject attackPrefab;
    private Cooldown cooldown;

    private int health;

    private NavMeshAgent agent;
    private Transform player;

    private State state;

    private void Start() {
        state = State.Chasing;
        health = maxHealth;
    }

    private void OnEnable() {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<PlayerController>().transform;

        agent.avoidancePriority = Random.Range(1, 500);
    }

    private void OnDestroy()
    {
        cooldown?.Stop();
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
        state = newState;
        switch (newState) {
            case State.Attacking:
                //  TODO:
                agent.SetDestination(transform.position);
                cooldown = Cooldown.Wait(attackDelay)
                    .OnComplete(() => {
                        //TODO attack
                        Instantiate(attackPrefab, transform.position, transform.rotation, transform);

                        cooldown = Cooldown.Wait(attackRecovery)
                        .OnComplete(() =>
                        {
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
            // TODO: Play effect / animation
            Instantiate(experiencePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
