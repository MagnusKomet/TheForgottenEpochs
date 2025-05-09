using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeController : BasicSpellController
{
    public override void Start()
    {
        Destroy(whoDies, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        DamageOnHitTrigger(other);
    }
}
