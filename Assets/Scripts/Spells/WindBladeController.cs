using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBladeController : BasicSpellController
{

    private void OnTriggerEnter(Collider other)
    {
        DamageOnHitTrigger(other);
    }
}
