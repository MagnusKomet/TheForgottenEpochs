using MimicSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] 
    public int maxHealth = 100;
    [SerializeField]
    public int currentHealth;
    [SerializeField]
    public GameObject whoDies;
    [SerializeField]
    public int minDropsQuantity;
    [SerializeField]
    public int maxDropsQuantity;

    public GameObject[] dropsPrefabs;
    float explosionForce = 10f; 
    float explosionRadius = 2f; 
    float positionVariation = 1f;

    public virtual void Start()
    {
        currentHealth = maxHealth;
        if (whoDies == null)
        {
            whoDies = gameObject;
        }

    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (whoDies.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            whoDies.GetComponent<EnemyController>().ForceChaseOnDamage();
        }

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public virtual void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public virtual void Die()
    {
        DropItemsWithExplosion();
        Destroy(whoDies);
    }

    public void DropItemsWithExplosion()
    {

        foreach (GameObject itemPrefab in dropsPrefabs)
        {
            if (itemPrefab.name == "EarthCrystalDrop" || itemPrefab.name == "MiguCore")
            {
                GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
                item.name = itemPrefab.name;
            }
            else
            {
                for (int i = 0; i < Random.Range(minDropsQuantity, maxDropsQuantity + 1); i++)
                {
                    // Crear una peque�a variaci�n en la posici�n inicial
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-positionVariation, positionVariation),
                        Random.Range(0, positionVariation), // Asegurar que algunos objetos salgan un poco m�s alto
                        Random.Range(-positionVariation, positionVariation)
                    );

                    // Instanciar el objeto en la posici�n del enemigo con la variaci�n
                    GameObject item = Instantiate(itemPrefab, transform.position + randomOffset, Quaternion.identity);
                    item.name = itemPrefab.name;

                    // Obtener el Rigidbody del objeto
                    Rigidbody rb = item.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        // Aplicar la explosi�n con AddExplosionForce
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);

                        // Aplicar una rotaci�n aleatoria para mayor realismo
                        Vector3 randomTorque = new Vector3(
                            Random.Range(-10f, 10f),
                            Random.Range(-10f, 10f),
                            Random.Range(-10f, 10f)
                        );
                        rb.AddTorque(randomTorque, ForceMode.Impulse);
                    }
                }
            }
            

        }
    }




}
