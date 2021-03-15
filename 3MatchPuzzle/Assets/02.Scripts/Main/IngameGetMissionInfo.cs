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

    public RectTransform gageUI_Icon;

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
    }


    protected override void Start()
    {
        base.Start();
        goalManager._ScoreUpate += Update_CurrentScore;
        goalManager._TimeCountUpate += Update_CurrentTimeCount;
        goalManager.Set_InitGame(CurrentStage);
    }

    private void Update()
    {
        gageUI_Image.fillAmount = Mathf.Lerp(gageUI_Image.fillAmount, (float)goalManager.CurrentGage / goalManager.MaxGage, 0.1f);
    }

    private void Update_CurrentScore(int score)
    {
        currentScoreUI_Text.text = score.ToString();
    }

    private void Update_CurrentTimeCount(int count)
    {
        timeCount_Text.text = count.ToString();
    }

    public Vector3 GetPosition_timeCount()
    {
        return timeCount_Text.transform.position;
    }

}
