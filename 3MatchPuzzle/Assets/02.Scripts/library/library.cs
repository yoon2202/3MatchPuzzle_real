using UnityEngine;
using UnityEngine.UI;

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

#region 인게임 미션 관련 정보
public class MissionBlockInfo
{
    public int numberNeeded;
    public int numberCollected = 0;
    public string tagName;
    public Text numberUI;
    public GameObject CompleteUI;

    public MissionBlockInfo(int numberNeeded, string tagName, Text numberUI, GameObject CompleteUI)
    {
        this.numberNeeded = numberNeeded;
        this.tagName = tagName;
        this.numberUI = numberUI;
        this.CompleteUI = CompleteUI;
    }

    public void UpdateMissionBlock(int number)
    {
        numberCollected += number;
        if (numberNeeded - numberCollected <= 0)
            CompleteUI.SetActive(true);
        else
            numberUI.text = (numberNeeded - numberCollected).ToString();

    }

}

public enum GameState
{
    wait, move, end, pause
}
#endregion