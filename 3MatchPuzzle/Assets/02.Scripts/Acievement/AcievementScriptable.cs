using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AcievementScriptable", menuName = "AcievementScriptable")]
public class AcievementScriptable : ScriptableObject
{
    [Header("업적 레벨")]
    public int AchievementsLevel;
    [Header("업적 제목")]
    public string AchievementsTitle;
    [Header("업적 내용")]
    public string AchievementsContent;
    [Header("미션 최대 개수")]
    public int MaxCount;
    [Header("미션 현재 개수")]
    public int CurrentCount;
    [Header("보상 종류")]
    public string RewordType;
    [Header("보상 개수")]
    public int RewordCount;
}
