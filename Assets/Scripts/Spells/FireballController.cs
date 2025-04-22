using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireballController : BasicSpellController
{

    [SerializeField]
    private GameObject explosion;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != shootFromTag)
        {
            DamageAoeCollider(collision);
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}