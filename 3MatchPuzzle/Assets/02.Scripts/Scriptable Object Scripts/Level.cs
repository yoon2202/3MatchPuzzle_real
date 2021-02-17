using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    /*
* 1. 게임 타입 설정
*     홀수 스테이지 : 점수 , 시간
*     짝수 스테이지 : 목표 블록, 이동 횟수
*     보스 스테이지 : 홀수 + 짝수 접목시켜서.. 아직 미정
*/

    public int[] Tile;
    public GameObject[] dots;
    public GameType gameType;
    public int Score = 0;
    public int Timer = 0;
    public MissionBlocks[] Blocks;
    public int MoveCount = 0;

    public BlankGoal[] levelGoals;

    /// <summary>
    /// 게임 타입 이름들 반환하는 함수
    /// </summary>
    public string[] returngameType()
    {
        int Length = System.Enum.GetNames(typeof(GameType)).Length;
        List<string> NameList = new List<string>();

        for (int i = 0; i < Length; i++)
        {
            NameList.Add(((GameType)i).ToString());
        }

        return NameList.ToArray();
    }
}
