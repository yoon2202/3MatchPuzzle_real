using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Obstruction_CurseBall : Obstruction_Abstract
{
    GoalManager goalManager;

    public override void Effect()
    {
        goalManager.CurrentScore -= (int)Math.Floor(goalManager.CurrentScore * 0.1f);
        base.Effect();
    }

    public override void Init()
    {
        goalManager = FindObjectOfType<GoalManager>();
    }
}
