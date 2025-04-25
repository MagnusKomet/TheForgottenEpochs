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
            EnemyHealthController enemy = other.GetComponent<EnemyHealthController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * knockbackForce * 2, ForceMode.Impulse);
            }
        }
        else
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController._PlayerVelocity.y = Mathf.Sqrt(knockbackForce * -10.0f * playerController.gravity);
            }
        }
    }

}
