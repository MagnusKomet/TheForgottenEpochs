using MimicSpace;
using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : EnemyHealthController
{
    public GameObject deathCamera;
    public Transform deathCameraSpawnPoint;

    public override void Start()
    {
        currentHealth = maxHealth;
        if (whoDies == null)
        {
            whoDies = gameObject;
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
        InventoryVisualManager.Instance.deathMenu.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Destroy(whoDies);
    }
}
