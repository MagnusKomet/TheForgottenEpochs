using MimicSpace;
using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : EnemyHealthController
{
    public GameObject deathCamera;
    public Transform deathCameraSpawnPoint;
    public RectTransform playerHealthBar;


    public override void Start()
    {
        GameObject healthBarObject = GameObject.Find("PlayerHealthBar");
        playerHealthBar = healthBarObject.GetComponent<RectTransform>();
        
        currentHealth = maxHealth;

        playerHealthBar.sizeDelta = new Vector2(playerHealthBar.sizeDelta.x, currentHealth);

        if (whoDies == null)
        {
            whoDies = gameObject;
        }
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        playerHealthBar.sizeDelta = new Vector2(playerHealthBar.sizeDelta.x, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public override void Die()
    {
        Instantiate(deathCamera, deathCameraSpawnPoint.position, deathCameraSpawnPoint.rotation);

        InventoryVisualManager.Instance.MenuActivated(false,true);

        Destroy(whoDies);
    }
}
