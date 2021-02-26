using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GoalManager : MonoBehaviour
{
    [HideInInspector]
    public GameType gameType;

    private int timeMoveCount;
    public int TimeMoveCount
    {
        get => timeMoveCount;
        set
        {
            timeMoveCount = value;

            if (timeMoveCount >= 0)
            {
                _TimeMoveCountUpate.Invoke(timeMoveCount);

                if (timeMoveCount == 0)
                    b_TimeOver = true;
            }
        }
    }

    private List<MissionBlockInfo> MissionBlocksInfo = new List<MissionBlockInfo>();

    private int MissionScore;

    private int currentScore = 0;
    public int CurrentScore
    {
        get => currentScore;
        set
        {
            currentScore = value;
            _ScoreUpate.Invoke(currentScore);
        }
    }

    [HideInInspector]
    public bool b_MissionComplete = false;

    [HideInInspector]
    public bool b_TimeOver = false;

    public delegate void TimeMoveCountUpdate(int count);
    public event TimeMoveCountUpdate _TimeMoveCountUpate;

    public delegate void ScoreUpdate(int Score);
    public event ScoreUpdate _ScoreUpate;


    // 스테이지 타입, 스코어, 시간 초기화
    public void Set_InitGame(Level level)
    {
        gameType = level.gameType;
        MissionScore = level.Score;

        if (GameType.Odd == gameType)
            TimeMoveCount = level.Timer;
        else if (GameType.Even == gameType)
            TimeMoveCount = level.MoveCount;

    }


    //게임 시작시에 미션 블록 정보 생성
    public void Add_MissionBlockInfo(MissionBlocks missionBlock, Text missionUI, GameObject CompleteUI)
    {
        MissionBlocksInfo.Add(new MissionBlockInfo(missionBlock.BlockCount, missionBlock.Block.tag, missionUI, CompleteUI));
    }


    #region 홀수 스테이지
    public void Update_CurrentScore(int score)
    {
        CurrentScore += score;

        if (GameType.Odd == gameType)
            b_MissionComplete = CompareScore();

    }

    public void Time_CountDown()
    {
        if(GameType.Odd == gameType)
            StartCoroutine("time_CountDown_Co");
    }

    IEnumerator time_CountDown_Co()
    {
        var ws = new WaitForSeconds(1.0f);

        while (true)
        {
            TimeMoveCount--;
            yield return ws;
        }
    }

    public void Time_CountDown_Stop()
    {
        StopCoroutine("time_CountDown_Co");
    }


    private bool CompareScore()
    {
        if (MissionScore > CurrentScore)
            return false;
        else
            return true;

    }
    #endregion

    #region 짝수 스테이지
    public void UpdateGoals(string tag)
    {
        if (GameType.Even != gameType)
            return;

        var Selects = MissionBlocksInfo.Where(x => x.tagName.CompareTo(tag) == 0);

        foreach (var Select in Selects)
        {
            Select.UpdateMissionBlock(1);
        }

        var Complete = MissionBlocksInfo.Count(x => x.numberCollected >= x.numberNeeded);

        b_MissionComplete = CompareBlockCount(Complete);
    }

    public void Update_CurrentTimeMoveCount()
    {
        if (GameType.Even == gameType)
            TimeMoveCount--;
    }

    private bool CompareBlockCount(int Complete)
    {
        if (MissionBlocksInfo.Count == Complete)
            return true;
        else
            return false;
    }

    #endregion
}
