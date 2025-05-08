using System.Collections;
using System.Collections.Generic;
using SlimUI.ModernMenu;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BasicSpellController : MonoBehaviour
{

    public float explosionRadius;
    public float explosionForce;
    public int damage;
    public string shootFromTag;
    public AudioClip spellDeathSound;
    public GameObject whoDies;

    private EnemyHealthController health;

    public virtual void Start()
    {
        Destroy(whoDies, 30f);
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

    public static void PlayDeathSound(AudioClip clip, Vector3 position)
    {
        GameObject tmp = new GameObject("TempAudio");
        tmp.transform.position = position;

        AudioSource aSource = tmp.AddComponent<AudioSource>();
        aSource.clip = clip;
        aSource.spatialBlend = 1f; // 3D sound
        aSource.minDistance = 10f; 
        aSource.maxDistance = 100f;

        tmp.AddComponent<CheckMusicVolume>();

        aSource.Play();
        Destroy(tmp, clip.length);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar una esfera en el editor para visualizar el área de explosión
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
