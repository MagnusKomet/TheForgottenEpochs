using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireballController : BasicSpellController
{

    [SerializeField]
    private GameObject particles;

    private void OnCollisionEnter(Collision collision)
    {
        DamageAoeCollider(collision);
        Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}