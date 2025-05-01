using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : BasicSpellController
{
    public float knockbackForce = 10f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && shootFromTag != "Enemy")
        {
            EnemyHealthController enemy = other.GetComponent<EnemyHealthController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController._PlayerVelocity.y = Mathf.Sqrt(knockbackForce * -10.0f * playerController.gravity);
            }

            if (shootFromTag != "Player")
            {
                PlayerHealthController playerHealth = other.GetComponent<PlayerHealthController>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                }
            }
        }
        else
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * knockbackForce * 2, ForceMode.Impulse);
            }
        }
    }

}
