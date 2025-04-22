using PlayerSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoController : BasicSpellController
{
    public float speed = 5f;
    public float knockbackForce = 10f;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != shootFromTag)
        {
            Debug.Log("Hit: " + other.gameObject.name);
            EnemyHealthController enemy = other.GetComponent<EnemyHealthController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("Aupa: " + other.gameObject.name);
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController._PlayerVelocity.y = Mathf.Sqrt(knockbackForce * -10.0f * playerController.gravity);
            }
        }
    }

}
