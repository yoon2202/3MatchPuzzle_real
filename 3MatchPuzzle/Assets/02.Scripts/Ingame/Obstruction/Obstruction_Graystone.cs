﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obstruction_Graystone : Obstruction_Abstract
{
    GoalManager goalManager;
    IngameGetMissionInfo ingameGetMissionInfo;

    [SerializeField]
    private Transform GrayStone_Particle;
    public Ease ease;

    public override void Init() 
    {
        goalManager = FindObjectOfType<GoalManager>();
        ingameGetMissionInfo = FindObjectOfType<IngameGetMissionInfo>();
    }

    public override void Effect()
    {
        Create_Particle();
        base.Effect();
    }

    private void Create_Particle()
    {
        var Particle = Instantiate(GrayStone_Particle.gameObject,transform.position,Quaternion.identity);
        Particle.transform.DOMove(ingameGetMissionInfo.GetPosition_timeCount(), 1.5f, false).SetEase(ease).OnComplete( () => { Debuff(Particle); });
    }

    private void Debuff(GameObject Particle)
    {
        goalManager.TimeCount -= 3;
        Destroy(Particle);
    }

}