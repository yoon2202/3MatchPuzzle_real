using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    public int[] Tile = new int[81];
    public GameObject[] dots;
    public GameType gameType;
    public int Score = 0;
    public int Timer = 0;
    public MissionBlocks[] Blocks;

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
