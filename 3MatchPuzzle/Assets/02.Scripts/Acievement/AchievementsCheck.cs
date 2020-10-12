using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsCheck : MonoBehaviour
{
    public static AchievementsCheck instance = null;
    public AcievementList acievementList;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void Checkfunction(Dot dot )
    {
        if (dot.SpecialBlockCheck())
            acievementList.AcievementScriptables[1].CurrentCount++;

    }

}

