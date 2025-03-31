using MimicSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : EnemyHealthController
{
    public GameObject deathCamera;
    public Transform deathCameraSpawnPoint;
    public GameObject deathMenu;

    public override void Start()
    {

        currentHealth = maxHealth;
        if (whoDies == null)
        {
            whoDies = gameObject;
        }

        deathMenu = GameObject.Find("DeathMenu");
        while (deathMenu.activeSelf)
        {            
            deathMenu.SetActive(false);
        }
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Instantiate(deathCamera, deathCameraSpawnPoint.position, deathCameraSpawnPoint.rotation);
        deathMenu.SetActive(true);

        Destroy(whoDies);
    }
}
