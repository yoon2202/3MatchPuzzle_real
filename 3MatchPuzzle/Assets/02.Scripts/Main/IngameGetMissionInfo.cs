using UnityEngine;
using UnityEngine.UI;

public class IngameGetMissionInfo : MonoBehaviour
{

    [SerializeField]
    private Transform MissionGroupRootUI;

    [SerializeField]
    private GameObject MissionScoreRootUI;

    [SerializeField]
    private Text TimeMoveCount;

    [SerializeField]
    private GameObject MissionItem;

    [SerializeField]
    private Text MissionScoreUI;

    [SerializeField]
    private Text CurrentScoreUI;

    private Level CurrentStage;

    private GoalManager goalManager;

    private void Awake()
    {
        goalManager = FindObjectOfType<GoalManager>();
    }

    private void Start()
    {
        CurrentStage = InfoManager.ReturnCurrentStage();
        goalManager._ScoreUpate += Update_CurrentScore;
        goalManager._TimeMoveCountUpate += Update_CurrentTimeMoveCount;
        goalManager.Set_InitGame(CurrentStage);

        switch (CurrentStage.gameType)
        {
            case GameType.Odd:
                MissionScoreUI.text = CurrentStage.Score.ToString();
                MissionScoreRootUI.SetActive(true);
                break;
            case GameType.Even:
                foreach (var block in CurrentStage.Blocks)
                {
                    GameObject MissionItem_ = Instantiate(MissionItem);
                    MissionItem_.transform.SetParent(MissionGroupRootUI, false);
                    IngameMissionItem MissionItemUI = MissionItem_.GetComponent<IngameMissionItem>();
                    MissionItemUI.MissionImg.sprite = InfoManager.ReturnMissionSprite(block.Block.tag);
                    MissionItemUI.MissionText.text = block.BlockCount.ToString();
                    goalManager.Add_MissionBlockInfo(block, MissionItemUI.MissionText, MissionItemUI.CompleteUI);
                }
                MissionGroupRootUI.gameObject.SetActive(true);
                break;    
        }
    }

    private void Update_CurrentScore(int score)
    {
        CurrentScoreUI.text = score.ToString();
    }

    private void Update_CurrentTimeMoveCount(int count)
    {
        TimeMoveCount.text = count.ToString();
    }

}
