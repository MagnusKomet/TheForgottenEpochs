using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWallController : BasicSpellController
{
    private Rigidbody rb;
    private AudioSource audioSource;

    public override void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (rb != null)
        {
            rb.useGravity = false;
            Invoke("ActivateRigidbody", 5f);
        }

        Destroy(whoDies, 10f);
    }

    private void ActivateRigidbody()
    {
        rb.useGravity = true;
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
