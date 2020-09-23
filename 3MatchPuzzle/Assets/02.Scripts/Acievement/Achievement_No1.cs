using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement_No1 : AchievementsCheck
{

    private void Awake()
    {
        CurrentCount = 5;
    }

    void Start()
    {
        Debug.Log(CurrentCount);
    }

    public override void DotCheck(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }
}
