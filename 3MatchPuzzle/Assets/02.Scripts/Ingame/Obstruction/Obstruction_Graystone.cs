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


    //private GameObject Create_Particle;

    public override void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        ingameGetMissionInfo = FindObjectOfType<IngameGetMissionInfo>();
        base.Start();
    }

    public override void Init() { }

    public override void Effect()
    {
        Debug.Log("발동");
        Create_Particle();
        Destroy(gameObject);
    }

    private void Create_Particle()
    {
        var Particle = Instantiate(GrayStone_Particle.gameObject);
        Particle.transform.DOMove(ingameGetMissionInfo.GetPosition_timeCount(), 1.5f, false).SetEase(ease).OnComplete( () => { Debuff(Particle); });
    }

    private void Debuff(GameObject Particle)
    {
        goalManager.TimeCount -= 3;
        Destroy(Particle);
    }

}
