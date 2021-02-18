using UnityEngine;
using UnityEngine.UI;

public class GetMissionItem : MonoBehaviour
{
    [SerializeField]
    private Transform MissionGroupRootUI;

    [SerializeField]
    private Text MissionScoreUI;

    [SerializeField]
    private GameObject MissionItem;

    [SerializeField]
    private Text CurrentStageText;

    private Level CurrentStage;

    private void Start()
    {
        CurrentStageText.text = "Stage " + InfoManager.ReturnStageCount();
        CurrentStage = InfoManager.ReturnCurrentStage();

        switch (CurrentStage.gameType)
        {
            case GameType.Odd:
                MissionScoreUI.text = CurrentStage.Score.ToString();
                MissionScoreUI.gameObject.SetActive(true);
                break;
            case GameType.Even:
                foreach (var block in CurrentStage.Blocks)
                {
                    GameObject MissionItem_ = Instantiate(MissionItem);
                    MissionItem_.transform.SetParent(MissionGroupRootUI, false);
                    MissionItem MissionItemUI = MissionItem_.GetComponent<MissionItem>();
                    MissionItemUI.MissionImg.sprite = InfoManager.ReturnMissionSprite(block.Block.tag);
                    MissionItemUI.MissionText.text = block.BlockCount.ToString();
                }
                MissionGroupRootUI.gameObject.SetActive(true);
                break;    
        }
    }



}
