using MimicSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : EnemyHealthController
{
    public new void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }


}
