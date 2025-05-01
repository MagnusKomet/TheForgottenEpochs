using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRingController : BasicSpellController
{
    private float healAmount = 10f;
    private float damageAmount = 10f;
    private float interval = 3f;

    private HashSet<GameObject> objectsInRange = new HashSet<GameObject>();
    private Coroutine healingAndDamagingCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        objectsInRange.Add(other.gameObject);

        if (healingAndDamagingCoroutine == null)
        {
            healingAndDamagingCoroutine = StartCoroutine(HealAndDamageOverTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null || other.gameObject == null)
            return;

        objectsInRange.Remove(other.gameObject);

        if (objectsInRange.Count == 0 && healingAndDamagingCoroutine != null)
        {
            StopCoroutine(healingAndDamagingCoroutine);
            healingAndDamagingCoroutine = null;
        }
    }

    private IEnumerator HealAndDamageOverTime()
    {
        while (true)
        {
            foreach (var obj in new HashSet<GameObject>(objectsInRange))
            {
                if (obj == null)
                {
                    objectsInRange.Remove(obj);
                    continue;
                }

                if (obj.CompareTag("Player"))
                {
                    Debug.Log("Healing player: " + obj.name);
                    var healthController = obj.GetComponent<PlayerHealthController>();
                    if (healthController != null)
                    {
                        Debug.Log("Healed: " + obj.name);
                        healthController.Heal((int)healAmount);
                    }
                }
                else if (obj.CompareTag("Enemy"))
                {
                    var healthController = obj.GetComponent<EnemyHealthController>();
                    if (healthController != null)
                    {
                        healthController.TakeDamage((int)damageAmount);
                    }
                }
            }

            yield return new WaitForSeconds(interval);
        }
    }
}
