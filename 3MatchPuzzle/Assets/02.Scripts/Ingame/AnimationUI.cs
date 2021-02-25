using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnimationUI : MonoBehaviour
{
    private Image MissionInfoUI;
    private GoalManager goalManager;
    private Board board;

    void Start()
    {
        board = FindObjectOfType<Board>();
        goalManager = FindObjectOfType<GoalManager>();
        MissionInfoUI.DOColor(new Color(0, 0, 0, 0), 3).SetDelay(2.5f).OnComplete(() => CompleteFunction()).SetEase(Ease.OutBack);
    }

    private void CompleteFunction()
    {
        board.b_PlayStart = true;
        goalManager.Time_CountDown();
        MissionInfoUI.raycastTarget = false;

    }
}
