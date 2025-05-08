using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleController : BasicSpellController
{
    public float speed = 5f;
    private Transform target;
    GameObject[] enemies;

    public override void Start()
    {
        Destroy(gameObject, 30f);
        FindClosestEnemy();
        if(shootFromTag == "Mimic")
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            if(GameObject.FindGameObjectWithTag("Mimic"))
            {
                Destroy(gameObject, 0.5f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            MoveTowardsTarget();
        }
        else
        {
            FindClosestEnemy();
        }

    }

    private void FindClosestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            target = closestEnemy.transform;
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != shootFromTag)
        {
            DamageOnHitTrigger(other);
            Destroy(gameObject);

        }

    }

    private void OnDestroy()
    {
        PlayDeathSound(spellDeathSound, transform.position);
    }
}
