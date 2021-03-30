using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticCrystal_Split : MysticCrystal_Abstract
{
    public GameObject MysticCrystal_Normal;

    public override void Level_1()
    {
        Instantiate(MysticCrystal_Normal, transform.position, Quaternion.identity);
        Instantiate(MysticCrystal_Normal, transform.position, Quaternion.identity);
        Instantiate(MysticCrystal_Normal, transform.position, Quaternion.identity);
    }

    public override void Level_2()
    {
    }

    public override void Level_3()
    {
    }

}
