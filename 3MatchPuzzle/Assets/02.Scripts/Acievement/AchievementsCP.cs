using UnityEngine;
using UnityEngine.UI;

public class AchievementsCP : MonoBehaviour
{
    // ---------- 오브젝트 부착 ----------
    public Text AchievementsTitle;
    public Text AchievementsContent;
    public Text RewardContent;
    public Button RewardButton;

    // ---------- 업적 순서  ----------
    private AcievementScriptable acievementScriptable;

    void Awake()
    {
        AchievementsTitle = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        AchievementsContent = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        RewardContent = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        RewardButton = transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Button>();
    }
    private void Start()
    {
     
        RewardButton.onClick.AddListener(() => RewordGet());
    }

    void RewordGet()
    {
        Debug.Log(acievementScriptable.AchievementsLevel+1* acievementScriptable.RewordCount);
    }

    private void OnEnable()
    {
        if (acievementScriptable != null)
        {
            if (acievementScriptable.CurrentCount < acievementScriptable.MaxCount)
                RewardButton.interactable = false;
            else
                RewardButton.interactable = true;
        }
    }
    public void Get_order(int i)
    {
        acievementScriptable = AchievementsCheck.instance.acievementList.AcievementScriptables[i];
    }
}
