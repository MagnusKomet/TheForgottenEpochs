using System;
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
    int index;
    public Transform pathParent;
    Transform targetPoint;

    public void Start()
    {
        // Inicialitza l'índex en el primer punt del camí i assigna el primer punt com a targetPoint
        index = 0;
        targetPoint = pathParent.GetChild(index);
    }

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
        // Mou l'objecte cap al punt objectiu
        agent.SetDestination(targetPoint.position);


        // Comprova si l'objecte ha arribat al punt objectiu i si és així, canvia el punt objectiu al següent
        if (Vector3.Distance(transform.position, targetPoint.position) < 1f)
        {
            index++;
            index %= pathParent.childCount;
            targetPoint = pathParent.GetChild(index);
        }
    }

    // hacer que compruebe si ve al jugador desde el laser
    public override bool CheckPlayerInRange(float range)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, range))
        {
            Debug.DrawRay(transform.position, (player.position - transform.position).normalized * sightRange, Color.blue);
            return hit.transform == player;
        }

        return false;
    }

    // Dibuixa el camí en l'editor per visualitzar-lo millor
    void OnDrawGizmos()
    {
        Vector3 from;
        Vector3 to;
        // Recorre tots els fills de pathParent i dibuixa línies entre ells
        for (int a = 0; a < pathParent.childCount; a++)
        {
            from = pathParent.GetChild(a).position;
            to = pathParent.GetChild((a + 1) %
            pathParent.childCount).position;
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawLine(from, to);
        }
    }
}
