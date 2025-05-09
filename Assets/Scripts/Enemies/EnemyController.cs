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
    public float sightRange = 20f;
    public float attackRange = 12f;
    public LayerMask whatIsGround;

    public bool playerInSightRange;
    public bool playerInAttackRange;

    [Header("Patrol")]
    public float walkPointRange;
    public Vector3 walkPoint;
    public bool walkPointSet;

    [Header("Attacks")]
    public GameObject spell;
    public int spellDamage;
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    [Header("Forced Chase")]
    public float forcedChaseDuration;
    public bool isForcedChaseActive;
    public float forcedChaseTimer;


    public virtual void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        IAMovement();
    }

    public void IAMovement()
    {
        playerInSightRange = CheckPlayerInRange(sightRange);
        playerInAttackRange = CheckPlayerInRange(attackRange);

        if (isForcedChaseActive)
        {
            forcedChaseTimer -= Time.deltaTime;
            if (forcedChaseTimer <= 0)
            {
                isForcedChaseActive = false;
            }
            else
            {
                if (playerInAttackRange && playerInSightRange)
                {
                    AttackPlayer();
                }
                else
                {
                    ChasePlayer();
                }
            }
        }
        else
        {

            if (playerInAttackRange && playerInSightRange) AttackPlayer();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (!playerInSightRange && !playerInAttackRange) Patroling();
        }
    }

    public virtual bool CheckPlayerInRange(float range)
    {
        if (player == null) return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, range))
        {
            Debug.DrawRay(transform.position, (player.position - transform.position).normalized * sightRange, Color.blue);
            return hit.transform == player;
        }

        return false;
    }

    public virtual void Patroling()
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
        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    public void ForceChaseOnDamage()
    {
        isForcedChaseActive = true;
        forcedChaseTimer = forcedChaseDuration;
    }

    public virtual void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ShootSpell(spell);
        }
        else
        {
            Patroling();
        }
    }

    public void ShootSpell(GameObject spell)
    {
        var shootedSpell = Instantiate(spell, transform.position + transform.forward, Quaternion.identity);
        Rigidbody rb = shootedSpell.GetComponent<Rigidbody>();
        BasicSpellController controller = shootedSpell.GetComponent<BasicSpellController>();
        if (controller != null)
        {
            controller.damage = spellDamage;
            controller.shootFromTag = gameObject.tag;
        }

        if (rb != null)
        {
            rb.velocity = transform.forward * 50f;
        }

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
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
