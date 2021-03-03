using UnityEngine;
using UnityEngine.UI;

public class GetMissionItem : MonoBehaviour
{
    [SerializeField]
    protected Transform MissionGroupRootUI;

    [SerializeField]
    protected Text MissionScoreUI;

    [SerializeField]
    protected GameObject MissionItem;

    [SerializeField]
    protected Text CurrentStageText;

    protected Level CurrentStage;

    protected virtual void Start()
    {
        if (CurrentStageText != null)
            CurrentStageText.text = string.Format("Stage {0}", InfoManager.ReturnStageCount());

        CurrentStage = InfoManager.ReturnCurrentStage();

        MissionScoreUI.text = string.Format("{0}", CurrentStage.Score);
        MissionScoreUI.gameObject.SetActive(true);

        foreach (var block in CurrentStage.Blocks)
        {
            GameObject MissionItem_ = Instantiate(MissionItem);
            MissionItem_.transform.SetParent(MissionGroupRootUI, false);
            MissionItem MissionItemUI = MissionItem_.GetComponent<MissionItem>();
            MissionItemUI.MissionImg.sprite = InfoManager.ReturnMissionSprite(block.Block.tag);
            //MissionItemUI.MissionText.text = block.BlockCount.ToString();
        }
        MissionGroupRootUI.gameObject.SetActive(true);
    }
}

