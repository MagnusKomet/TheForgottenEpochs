using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BasicSpellController : MonoBehaviour
{

    public float explosionRadius;
    public float explosionForce;
    public int damage;
    public string shootFromTag;

    private EnemyHealthController health;

    public virtual void Start()
    {
        Destroy(gameObject, 30f);
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

    private void OnDrawGizmosSelected()
    {
        // Dibujar una esfera en el editor para visualizar el área de explosión
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
