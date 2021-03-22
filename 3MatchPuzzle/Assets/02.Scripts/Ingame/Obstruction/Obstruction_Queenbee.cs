using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class Obstruction_Queenbee : Obstruction_Abstract
{

    public override void Effect()
    {
        obstructionManager.B_Queenbee_EffectStart = true;
        base.Effect();
    }

    public override void Init(){} 
    
}
