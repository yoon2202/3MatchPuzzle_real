using UnityEngine;

#region 스테이지 관련 정보

public enum GameType
{
    Odd, Even, Boss
}

[System.Serializable]
public class MissionBlocks
{
    public GameObject Block;
    public int BlockCount = 0;
}

#endregion