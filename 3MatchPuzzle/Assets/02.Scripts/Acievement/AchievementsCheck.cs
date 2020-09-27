using UnityEngine;
using UnityEngine.UI;

public abstract class AchievementsCheck : MonoBehaviour
{

    // ---------- 오브젝트 부착 ----------
    protected Text AchievementsTitle;
    protected Text AchievementsContent;
    protected Text RewardContent;
    protected Button RewardButton;


    public abstract void DotCheck(GameObject gameObject);
    
    public virtual void SettingUI()
    {
        AchievementsTitle = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        AchievementsContent = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        RewardContent = transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
        RewardButton = transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Button>();
    }
}
