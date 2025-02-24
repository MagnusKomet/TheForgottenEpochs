using MimicSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] 
    private int maxHealth = 100;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private GameObject whoDies;

    public GameObject[] itemPrefabs;
    float explosionForce = 10f; 
    float explosionRadius = 2f; 
    float positionVariation = 1f; 

    private void Start()
    {
        currentHealth = maxHealth;
        if (whoDies == null)
        {
            whoDies = gameObject;
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (whoDies.gameObject.tag == "Mimic")
        {
            whoDies.GetComponent<MimicController>().ForceChaseOnDamage();
        }
        else if (whoDies.gameObject.tag == "Enemy")
        {
            whoDies.GetComponent<EnemyController>().ForceChaseOnDamage();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        DropItemsWithExplosion();
        Destroy(whoDies);
    }

    private void DropItemsWithExplosion()
    {

        foreach (GameObject itemPrefab in itemPrefabs)
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
