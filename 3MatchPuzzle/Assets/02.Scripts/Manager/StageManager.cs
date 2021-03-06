﻿using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    //public List<GameObject> stageChapter;
    //public List<LevelButton> stage;
    //public int ActivatedChapterIndex;

    [SerializeField]
    private Button StageReady;

    [SerializeField]
    private GameObject StageReadyUI;

    [SerializeField]
    private Button StageStart;

    [SerializeField]
    private Button StageUIClose;


    void Start()
    {
        StageReady.onClick.AddListener(() => StageReadyUI.SetActive(true));
        StageStart.onClick.AddListener(() => Startbtn());
        StageUIClose.onClick.AddListener(() => StageReadyUI.SetActive(false));
    }



   public void Startbtn()
    {
        LoadingSceneController.LoadScene("Ingame");
    }

    //int temp = i; // Closure 문제때문에 복사해서 사용한다.
}


//void addStageList() // 각 챕터에 있는 레벨스테이지들을 정렬한다. 
//{
//    for (int i = 0; i < stageChapter.Count; i++)
//    {
//        stage = stage.Union(stageChapter[i].GetComponentsInChildren<LevelButton>()).ToList();
//    }

//    settingStage(stage);
//}

//void settingStage(List<LevelButton> stage) //게임시작 시 스테이지 별 레벨에 따른 세팅
//{
//    for (int i = 0; i < stage.Count; i++)
//    {
//        int temp = i;
//        stage[temp].SetText(temp + 1);
//        stage[temp].GetComponent<Button>().onClick.AddListener(() => ClickStage(temp));
//        if (temp == currentStage) // 현재 스테이지와 같으면?
//        {
//            stage[temp].SetFlag(true);
//            ActivatedChapterIndex = stageChapter.IndexOf(stage[temp].transform.parent.gameObject);
//            stageChapter[ActivatedChapterIndex].SetActive(true);
//        }
//        else if (temp > currentStage)
//            stage[temp].SetDisabled();
//    }

//    StageName_(this.ActivatedChapterIndex);
//}
//public void ClickPanel(string direction)
//{
//    var CurrentActivatedChapterIndex = ActivatedChapterIndex;

//    if (direction.Equals("Left") && ActivatedChapterIndex > 0) // 왼쪽버튼을 눌렀으면
//    {
//        this.ActivatedChapterIndex--;
//    }
//    else if (direction.Equals("Right") && ActivatedChapterIndex < stageChapter.Count - 1)
//    {
//        this.ActivatedChapterIndex++;
//    }
//    else
//        return;

//    StageName_(this.ActivatedChapterIndex);
//    stageChapter[CurrentActivatedChapterIndex].SetActive(false);
//    stageChapter[ActivatedChapterIndex].SetActive(true);
//}

//public void ClickStage(int i)
//{
//    stage[currentStage].SetFlag(false);
//    stage[i].SetFlag(true);
//    currentStage = i;
//}

//public void StageName_(int ActivatedChapterIndex) // 좌,우 클릭 할때마다 이름 변경
//{
//    StageName.text = "Theme " + ((ActivatedChapterIndex / 5) + 1).ToString() + " Chapter " + ((ActivatedChapterIndex % 5)+1).ToString();
//}
