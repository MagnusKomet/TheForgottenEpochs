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

        Debug.Log($"{gameObject.name} recibió {damage} puntos de daño. Salud restante: {currentHealth}");

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

        Debug.Log($"{gameObject.name} se curó {amount} puntos de salud. Salud actual: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} ha muerto.");
        Destroy(whoDies);
    }
}
