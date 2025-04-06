using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : EnemyController
{
    [Header("Robot")]
    [SerializeField]
    private Transform laserMount;
    [SerializeField]
    private Transform weapon;

    // Update is called once per frame
    private void Update()
    {
        IAMovement();

        if (playerInSightRange || playerInAttackRange)
        {
            laserMount.LookAt(player.position);
        }
        else 
        { 
            laserMount.Rotate(Vector3.up * Time.deltaTime * 50f);
        }

    }

    //cambiar que el laser mire al jugador y la ametralladora gire y haga particulas
    public override void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        
        // Rotar robot hacia el jugador solo en el eje Y
        Vector3 direction = player.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        /*
        // Rotar el arma hacia el jugador solo en el eje X
        Vector3 weaponDirection = player.position - weapon.transform.position;
        weaponDirection.x = 0;
        weapon.transform.rotation = Quaternion.LookRotation(weaponDirection);
        */

        

        if (!alreadyAttacked)
        {
            CastFireball(transform.position + transform.forward);
        }
    }

    //que vaya de punto a punto y que vaya dando vueltas el laser
    public override void Patroling()
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

    // hacer que compruebe si ve al jugador desde el laser
    public override bool CheckPlayerInRange(float range)
    {
        RaycastHit hit;

        if (Physics.Raycast(laserMount.position, (player.position - transform.position).normalized, out hit, range))
        {
            Debug.DrawRay(laserMount.position, (player.position - transform.position).normalized * sightRange, Color.blue);
            return hit.transform == player;
        }

        return false;
    }
}
