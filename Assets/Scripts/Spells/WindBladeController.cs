using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeController : MonoBehaviour
{
    public int damage;

    private void Start()
    {
        Destroy(this, 30f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        var health = collision.gameObject.GetComponent<EnemyHealthController>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
