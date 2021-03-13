using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstruction_Graystone : Obstruction_Abstract
{
    GoalManager goalManager;


    public override void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        base.Start();
        
    }

    public override void Effect()
    {
        Debug.Log("발동");
    }

}
