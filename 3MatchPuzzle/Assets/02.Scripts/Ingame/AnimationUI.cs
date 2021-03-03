using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class AnimationUI : MonoBehaviour
{
    [SerializeField]
    private Image MissionInfoUI;

    private GoalManager goalManager;
    private Board board;

    public GameObject MissionSuccessUI;
    public GameObject MissionFailUI;


    void Start()
    {
        board = FindObjectOfType<Board>();
        goalManager = FindObjectOfType<GoalManager>();
        MissionInfoUI.DOColor(new Color(0, 0, 0, 0), 3).SetDelay(2f).OnComplete(() => CompleteFunction()).SetEase(Ease.OutQuad);
    }

    private void CompleteFunction()
    {
        board.b_PlayStart = true;
        goalManager.Time_CountDown();
        MissionInfoUI.raycastTarget = false;
    }
}
