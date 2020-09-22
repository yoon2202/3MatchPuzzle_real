using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("Board Dimensions")]
    public int width;
    public int height;

    [Header("Starting Tiles")]
    public TileType[] boardLayout;

    [Header("Available Dots")]
    public GameObject[] dots;



    [Header("스테이지 목표 설정")]
    public EndGameRequirements endGameRequirements;
    public BlankGoal[] levelGoals;
}
