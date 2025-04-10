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
            CharacterController playerController = other.GetComponent<CharacterController>();
            if (playerController != null)
            {
                Vector3 knockbackDirection = Vector3.up * knockbackForce;
                playerController.Move(knockbackDirection * Time.deltaTime);
            }
        }
    }
}
