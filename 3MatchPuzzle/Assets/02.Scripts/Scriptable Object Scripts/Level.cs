﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [HideInInspector]
    public int[] Tile = new int[81];

    [Header("사용 가능한 일반블록")]
    public GameObject[] dots;

    [Header("스테이지 목표 설정")]
    public EndGameRequirements endGameRequirements;
    public BlankGoal[] levelGoals;
}
