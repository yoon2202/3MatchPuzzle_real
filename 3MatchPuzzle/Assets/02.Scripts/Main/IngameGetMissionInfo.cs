using UnityEngine;
using UnityEngine.UI;

public class IngameGetMissionInfo : GetMissionItem
{
    [SerializeField]
    private Text timeCount_Text;

    [SerializeField]
    private Text currentScoreUI_Text;

    [SerializeField]
    private Image gageUI_Image;

    private GoalManager goalManager;

    public Transform gageUI_Icon;

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
    }


    protected override void Start()
    {
        base.Start();
        goalManager._ScoreUpate += Update_CurrentScore;
        goalManager._TimeCountUpate += Update_CurrentTimeCount;
        goalManager._GageUpdate += Update_CurrentGage;
        goalManager.Set_InitGame(CurrentStage);
    }

    private void Update_CurrentScore(int score)
    {
        currentScoreUI_Text.text = score.ToString();
    }

    private void Update_CurrentTimeCount(int count)
    {
        timeCount_Text.text = count.ToString();
    }

    private void Update_CurrentGage(int gage)
    {
        gageUI_Image.fillAmount = (float)gage / goalManager.MaxGage;
    }

}
