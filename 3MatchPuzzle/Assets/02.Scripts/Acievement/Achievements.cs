using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public List<AchievementsCP> AchievementCPList = new List<AchievementsCP>();
    public Transform Content;
    void Start()
    {
        for(int i =0; i< AchievementsCheck.instance.acievementList.AcievementScriptables.Length; i++) // 업적 리스트 개수만큼 Item 생성
        {
            Transform item = Instantiate(Resources.Load<Transform>("Item"));
            item.SetParent(Content);
        }

        for (int i =0; i < Content.childCount; i++) // AchievementList에 AchievementsCP 컴포넌트 담기
        {
            AchievementCPList.Add(Content.GetChild(i).GetComponent<AchievementsCP>());
        }

        for(int i = 0; i < AchievementCPList.Count; i++)
        {
            var word = "";
            AchievementCPList[i].AchievementsTitle.text = AchievementsCheck.instance.acievementList.AcievementScriptables[i].AchievementsTitle;
            if (AchievementsCheck.instance.acievementList.AcievementScriptables[i].state == AcievementScriptable.AcievementType.고압분사기)
                word = "제거";
            else
                word = "사용";

            AchievementCPList[i].AchievementsContent.text = ContentText(i) + word;
            AchievementCPList[i].Get_order(i);
        }
    }

    string ContentText(int i)
    {
        var content = "";
        content += AchievementsCheck.instance.acievementList.AcievementScriptables[i].state.ToString();
        content += " ";
        content += AchievementsCheck.instance.acievementList.AcievementScriptables[i].MaxCount.ToString();
        content += "회 ";
        return content;
    }
}
