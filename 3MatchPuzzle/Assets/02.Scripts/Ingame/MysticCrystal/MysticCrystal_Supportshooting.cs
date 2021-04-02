using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticCrystal_Supportshooting : MysticCrystal_Abstract
{
    private MysticManager mysticManager;

    public override void Level_1()
    {
        mysticManager = FindObjectOfType<MysticManager>();
        mysticManager.B_Support_shooting = true;
    }

    public override void Level_2()
    {
    }

    public override void Level_3()
    {
    }
}
