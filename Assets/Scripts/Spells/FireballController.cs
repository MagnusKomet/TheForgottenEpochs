using System.Collections;
using UnityEngine;

public class FireballController : BasicSpellController
{

    [SerializeField]
    private GameObject explosion;

    public bool automatic;

    public override void Start()
    {
        if(automatic)
        {
            Destroy(gameObject, 30f);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 50f;
        }
    }


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