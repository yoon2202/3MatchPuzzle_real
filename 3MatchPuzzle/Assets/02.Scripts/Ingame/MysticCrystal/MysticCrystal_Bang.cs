using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticCrystal_Bang : MysticCrystal_Abstract
{
    public override Transform TargetInit()
    {
        if (Level == 2 || Level == 3)
            return find_Block(true);
        else
            return find_Block();
    }


    public override void Level_1()
    {
        scoreManager.GetScore(5);
    }

    public override void Level_2()
    {
        scoreManager.GetScore(7);
    }

    public override void Level_3()
    {
        scoreManager.GetScore(10);
    }

}
