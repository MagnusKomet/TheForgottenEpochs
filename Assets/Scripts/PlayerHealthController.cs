using MimicSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : EnemyHealthController
{
    public new void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public new void Die()
    {
        
        // Create a new camera
        GameObject cameraObject = new GameObject("DeathCamera");
        Camera deathCamera = cameraObject.AddComponent<Camera>();

        Instantiate(deathCamera, cameraObject.transform);

        // Position the camera above the player
        cameraObject.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        cameraObject.transform.rotation = Quaternion.Euler(90, 0, 0);


        // Destroy the player game object
        Destroy(gameObject);
    }


}
