using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class GoalManager : MonoBehaviour
{

    private int timeCount;
    public int TimeCount
    {
        get => timeCount;
        set
        {
            timeCount = value;

            if (timeCount >= 0)
            {
                _TimeCountUpate.Invoke(timeCount);

                if (timeCount == 0)
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
    public int MaxGage = 10;

    private int currentGage;
    public int CurrentGage
    {
        get => currentGage;
        set
        {
            currentGage = value;
            _GageUpdate.Invoke(currentGage);

            if (currentGage >= MaxGage)
            {
                currentGage %= MaxGage;
                createMisteak.Createobj();
                Debug.Log("미스틱 생성!");
            }
        }
    }


    [HideInInspector]
    public bool b_MissionComplete = false;

    [HideInInspector]
    public bool b_TimeOver = false;

    public delegate void TimeCountUpdate(int count);
    public event TimeCountUpdate _TimeCountUpate;

    public delegate void ScoreUpdate(int Score);
    public event ScoreUpdate _ScoreUpate;

    public delegate void GageUpdate(int Gage);
    public event GageUpdate _GageUpdate;

    private CreateMisteak createMisteak;

    private void Start()
    {
        createMisteak = FindObjectOfType<CreateMisteak>();
    }

    // 스테이지 타입, 스코어, 시간 초기화
    public void Set_InitGame(Level level)
    {
        MissionScore = level.Score;
        TimeCount = level.Timer;
    }


    #region 스코어
    public void Update_CurrentScore(int score)
    {
        CurrentScore += score;
        b_MissionComplete = CompareScore();

    }

    private bool CompareScore()
    {
        if (MissionScore > CurrentScore)
            return false;
        else
            return true;

    }
    #endregion

    #region 타이머
    public void Time_CountDown()
    {
        StartCoroutine("time_CountDown_Co");
    }

    IEnumerator time_CountDown_Co()
    {
        var ws = new WaitForSeconds(1.0f);

        while (true)
        {
            TimeCount--;
            yield return ws;
        }
    }

    public void Time_CountDown_Stop()
    {
        StopCoroutine("time_CountDown_Co");
    }
    #endregion

    #region 게이지
    public void Update_CurrentGage(int gage)
    {
        CurrentGage += gage;
    }
    #endregion

    #region 임시 저장


    //게임 시작시에 미션 블록 정보 생성
    public void Add_MissionBlockInfo(MissionBlocks missionBlock, Text missionUI, GameObject CompleteUI)
    {
        MissionBlocksInfo.Add(new MissionBlockInfo(missionBlock.BlockCount, missionBlock.Block.tag, missionUI, CompleteUI));
    }

    public void UpdateGoals(string tag)
    {

        var Selects = MissionBlocksInfo.Where(x => x.tagName.CompareTo(tag) == 0);

        foreach (var Select in Selects)
        {
            Select.UpdateMissionBlock(1);
        }

        var Complete = MissionBlocksInfo.Count(x => x.numberCollected >= x.numberNeeded);

        b_MissionComplete = CompareBlockCount(Complete);
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
