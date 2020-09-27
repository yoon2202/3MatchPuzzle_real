using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement_No1 : AchievementsCheck
{
    
    private void Awake()
    {
       
    }

    void Start()
    {
        SettingUI();
    }

    public override void DotCheck(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }
    public override void SettingUI()
    {
        base.SettingUI();
    }
}
