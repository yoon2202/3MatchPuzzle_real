using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction_Dust : Obstruction_Abstract
{

    public override void Effect()
    {
        obstructionManager.B_Dust_EffectStart = true;
        Destroy(gameObject);
    }

    public override void Init(){}

}
    