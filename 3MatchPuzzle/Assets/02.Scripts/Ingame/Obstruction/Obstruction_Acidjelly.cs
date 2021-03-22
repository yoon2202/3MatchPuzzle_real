using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction_Acidjelly : Obstruction_Abstract
{
    public override void Effect()
    {
        obstructionManager.B_Acidjelly_EffectStart = true;
        base.Effect();
    }   

    public override void Init(){}
}
