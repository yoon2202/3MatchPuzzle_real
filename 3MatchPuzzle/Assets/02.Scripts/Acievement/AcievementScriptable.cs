using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AcievementScriptable", menuName = "AcievementScriptable")]
public class AcievementScriptable : ScriptableObject
{
    public enum AcievementType { 십자나무꽃, 콘크리트블록, 일반블록, 고압분사기};
    public enum RewordType { 골드, 수정, 날개};

    [Header("업적 레벨")]
    public int AchievementsLevel;
    [Header("업적 제목")]
    public string AchievementsTitle;
    [Header("업적 종류")]
    public AcievementType state;
    [Header("미션 최대 개수")]
    public int MaxCount;
    [Header("미션 현재 개수")]
    public int CurrentCount;
    [Header("보상 종류")]
    public RewordType rewordType;
    [Header("보상 양")]
    public int RewordCount;
}
