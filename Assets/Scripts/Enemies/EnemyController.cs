using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("IA")]
    public Transform player;
    public NavMeshAgent agent;

    [Header("Ranges")]
    public float sightRange = 10f;
    public float attackRange = 5f;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;

    public bool playerInSightRange;
    public bool playerInAttackRange;

    [Header("Patrol")]
    public float walkPointRange;
    public Vector3 walkPoint;
    public bool walkPointSet;

    [Header("Atacks")]
    public GameObject fireball;
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    [Header("Forced Chase")]
    public float forcedChaseDuration;
    public bool isForcedChaseActive;
    public float forcedChaseTimer;


    public void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        IAMovement();
    }

    public void IAMovement()
    {
        if (isForcedChaseActive)
        {
            forcedChaseTimer -= Time.deltaTime;
            if (forcedChaseTimer <= 0)
            {
                isForcedChaseActive = false;
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();
        }
    }

    public void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    public void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 3f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    public void ForceChaseOnDamage()
    {
        isForcedChaseActive = true;
        forcedChaseTimer = forcedChaseDuration;
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            var ball = Instantiate(fireball, transform.position + transform.forward, Quaternion.identity);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.forward * 50f;
            }
            Destroy(ball, 30f);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
