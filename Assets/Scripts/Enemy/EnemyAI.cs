using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private enum EnemyState
    {
        Idle,
        Chasing,
        Attacking,
        Returning,
        Dead
    }

    [Header("Combat")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackDistance = 1.8f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("Chase")]
    [SerializeField] private float chaseLimitDistance = 18f;
    [SerializeField] private float deactivateDelayAfterReturn = 3f;

    [Header("Loot")]
    [SerializeField] private EnemyLoot enemyLoot;
    [SerializeField] private bool lootGiven;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private Transform player;
    private Vector3 spawnPosition;
    private EnemySpawnZone ownerZone;
    private EnemyIdentity enemyIdentity;

    private NavMeshAgent agent;
    private Health health;
    private bool experienceGiven;

    private EnemyState currentState;
    private float nextAttackTime;
    private float returnTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        enemyLoot = GetComponentInChildren<EnemyLoot>();
        enemyIdentity = GetComponent<EnemyIdentity>();
    }

    public void Initialize(Transform playerTarget, Vector3 originalSpawnPosition, EnemySpawnZone zone)
    {
        player = playerTarget;
        spawnPosition = originalSpawnPosition;
        ownerZone = zone;

        currentState = EnemyState.Chasing;
        returnTimer = 0f;
        lootGiven = false;
        experienceGiven = false;

        if (agent != null)
        {
            agent.enabled = true;
            agent.isStopped = false;
            agent.Warp(spawnPosition);
        }

        if (health != null)
        {
            health.ResetHealth();
        }

        SetMoveAnimation(1f);
    }

    private void Update()
    {
        if (player == null) return;

        if (health != null && health.IsDead)
        {
            Die();
            return;
        }

        switch (currentState)
        {
            case EnemyState.Chasing:
                ChasePlayer();
                break;

            case EnemyState.Attacking:
                AttackPlayer();
                break;

            case EnemyState.Returning:
                ReturnToSpawn();
                break;

            case EnemyState.Dead:
                break;
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceFromSpawn = Vector3.Distance(transform.position, spawnPosition);

        if (distanceFromSpawn > chaseLimitDistance)
        {
            currentState = EnemyState.Returning;
            return;
        }

        if (distanceToPlayer <= attackDistance)
        {
            currentState = EnemyState.Attacking;
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(player.position);

        SetMoveAnimation(1f);
    }

    private void AttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float distanceFromSpawn = Vector3.Distance(transform.position, spawnPosition);

        if (distanceFromSpawn > chaseLimitDistance)
        {
            currentState = EnemyState.Returning;
            return;
        }

        if (distanceToPlayer > attackDistance)
        {
            currentState = EnemyState.Chasing;
            return;
        }

        agent.isStopped = true;
        SetMoveAnimation(0f);

        LookAtPlayer();

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            if (animator != null)
                animator.SetTrigger("Attack01");
                        
            if (player.TryGetComponent<Health>(out var playerHealth))
                playerHealth.TakeDamage(damage);

            if(playerHealth.CurrentHealth < 0)
            {
                animator.SetTrigger("PlayerDie");
            }
        }
    }

    public void GetHit()
    {
        if(animator != null)
        {
            animator.SetTrigger("Hit");
        }
    }

    private void ReturnToSpawn()
    {
        agent.isStopped = false;
        agent.SetDestination(spawnPosition);

        SetMoveAnimation(1f);

        float distanceToSpawn = Vector3.Distance(transform.position, spawnPosition);

        if (distanceToSpawn <= 1f)
        {
            returnTimer += Time.deltaTime;
            SetMoveAnimation(0f);

            if (returnTimer >= deactivateDelayAfterReturn)
            {
                ownerZone.DeactivateEnemy(this);
            }
        }
    }

    private void Die()
    {
        if (currentState == EnemyState.Dead) return;

        currentState = EnemyState.Dead;

        agent.isStopped = true;
        SetMoveAnimation(0f);

        if (animator != null)
            animator.SetTrigger("Die");


        if (enemyIdentity != null)
        {
            QuestManager.Instance?.RegisterEnemyKilled(enemyIdentity.EnemyId);
        }
        if (!experienceGiven)
        {
            experienceGiven = true;

            if (enemyLoot != null && player != null)
            {
                PlayerExperience playerExperience = player.GetComponent<PlayerExperience>();

                if (playerExperience != null)
                    playerExperience.AddExperience(enemyLoot.GetExperienceReward());
            }
        }

        if (!lootGiven)
        {
            lootGiven = true;

            if (enemyLoot != null)
                enemyLoot.DropLoot(transform.position);
        }


        //ownerZone.DeactivateEnemy(this);
        Invoke(nameof(DeactivateSelf), 1.5f);
    }

    private void DeactivateSelf()
    {
        this.gameObject.SetActive(false);
    }

    private void LookAtPlayer()
    {
        Vector3 lookPosition = new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        );

        transform.LookAt(lookPosition);
    }

    private void SetMoveAnimation(float speed)
    {
        if (animator != null)
            animator.SetFloat("Speed", speed);
    }
}
