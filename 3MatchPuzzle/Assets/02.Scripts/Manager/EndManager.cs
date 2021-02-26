using UnityEngine;


public class EndManager : MonoBehaviour
{
    private Board board;
    private AnimationUI animationUI;
    private GoalManager goalManager;

    void Start()
    {
        board = FindObjectOfType<Board>();
        animationUI = FindObjectOfType<AnimationUI>();
        goalManager = FindObjectOfType<GoalManager>();
    }

    private void Update()
    {
        // 게임이 시작하고 현재 매칭이 안되어있는 경우
        if (board.b_PlayStart == true && board.b_matching == false && board.currentState != GameState.end)
        {
            if (goalManager.b_MissionComplete) // 성공
            {
                goalManager.Time_CountDown_Stop();
                animationUI.MissionSuccessUI.SetActive(true);
                board.currentState = GameState.end;
            }
            else
            {
                if (goalManager.b_TimeOver) // 실패
                {
                    goalManager.Time_CountDown_Stop();
                    animationUI.MissionFailUI.SetActive(true);
                    board.currentState = GameState.end;
                }
            }
        }
    }
}
