using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWallController : BasicSpellController
{
    private Rigidbody rb;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            Invoke("ActivateRigidbody", 5f);
        }

        Destroy(gameObject, 10f);
    }

    private void ActivateRigidbody()
    {
        rb.useGravity = true;
    }
}
