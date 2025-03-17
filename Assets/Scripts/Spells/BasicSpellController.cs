using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BasicSpellController : MonoBehaviour
{

    private float explosionRadius = 5f;
    private float explosionForce = 500f;
    public int damage;
    public string shootFromTag;

    private EnemyHealthController health;



    public void Start()
    {
        Destroy(this, 30f);
    }


    public void DamageOnHitTrigger(Collider other)
    {
        
        if (other.gameObject.tag != shootFromTag)
        {
            health = other.gameObject.GetComponent<EnemyHealthController>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }

    public void DamageAoeCollider(Collision collision)
    {
        // Generar una esfera para detectar objetos cercanos
        var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var obj in surroundingObjects)
        {
            if (obj.gameObject.tag != shootFromTag)
            {
                health = obj.GetComponent<EnemyHealthController>();

                var rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f);
                }

                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
        
    }
}
