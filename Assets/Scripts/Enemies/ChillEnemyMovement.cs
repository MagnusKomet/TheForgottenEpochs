using MimicSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillEnemyMovement : MimicController
{

    // Update is called once per frame
    void Update()
    {
        MovimientoMimico();
        Patroling();
    }
}
