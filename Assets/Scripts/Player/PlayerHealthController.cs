using MimicSpace;
using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : EnemyHealthController
{
    public GameObject deathCamera;
    public Transform deathCameraSpawnPoint;
    public RectTransform playerHealthBar;


    public override void Start()
    {
        currentHealth = maxHealth;

        GameObject healthBarObject = GameObject.Find("PlayerHealthBar");
        if(healthBarObject != null)
        {            
            playerHealthBar = healthBarObject.GetComponent<RectTransform>();
            playerHealthBar.sizeDelta = new Vector2(playerHealthBar.sizeDelta.x, currentHealth);
        }
        


        if (whoDies == null)
        {
            whoDies = gameObject;
        }
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (playerHealthBar != null)
            playerHealthBar.sizeDelta = new Vector2(playerHealthBar.sizeDelta.x, currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public override void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if (playerHealthBar != null)
            playerHealthBar.sizeDelta = new Vector2(playerHealthBar.sizeDelta.x, currentHealth);
    }

    public override void Die()
    {
        Instantiate(deathCamera, deathCameraSpawnPoint.position, deathCameraSpawnPoint.rotation);

        InventoryVisualManager.Instance.MenuActivated(false,true);

        Destroy(whoDies);
    }
}
