using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    private float explosionRadius = 5f;
    private float explosionForce = 500f;
    public int damage;
    [SerializeField] 
    private GameObject particles;

    private void OnCollisionEnter(Collision collision)
    {
        // Generar una esfera para detectar objetos cercanos
        var surroundingObjects = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var obj in surroundingObjects)
        {
            // Aplicar fuerza de explosión
            var rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f);
            }

            // Aplicar daño si el objeto tiene un script de salud
            var health = obj.GetComponent<HealthController>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }

        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}