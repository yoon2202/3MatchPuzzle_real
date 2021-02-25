using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndManager : MonoBehaviour
{
    private Board board;

    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    private void Update()
    {
        // 게임 진행 상태 확인
        if (board.b_PlayStart == true && board.b_matching == false)
        {
            // 현재 게임 진행 상태가 끝난다면?

        }
    }
}
