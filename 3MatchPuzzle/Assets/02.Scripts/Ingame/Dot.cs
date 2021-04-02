using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Dot : State, IPointerDownHandler, IPointerUpHandler
{
    [Header("현재 포지션 위치")]
    public int column = -1;
    public int row = -1;

    private FindMatches findMatches;
    private GoalManager goalManager;
    private ObstructionManager obstructionManager;


    private Board board;
    public Dot otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    public float swipeAngle = 0;
    public float swipeResist = 1f;

    public bool isAcorn;

    void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        findMatches = FindObjectOfType<FindMatches>();
        obstructionManager = FindObjectOfType<ObstructionManager>();
        board = FindObjectOfType<Board>();

    }

    public void OnPointerDown(PointerEventData eventData) // 선택형 매치 판별
    {
        if (board.currentState == GameState.move)
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    #region Swipe 관련 함수
    void CalculateAngle()
    { // 두 게임오브젝트 사이의 각도를 알기위해 Atan2를 사용 -> 두점사이에 길이를 통해 각도를 알아낸다.
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MoviePieces();
        }
        else
            board.currentState = GameState.move;
    }

    void MoviePieces()
    {
        if ((swipeAngle > -45 && swipeAngle <= 45) && column < board.width - 1)
        {
            //Right swipe
            MovePiecesActual(Vector3.right);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Up swipe
            MovePiecesActual(Vector3.up);
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            MovePiecesActual(Vector3.left);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //Down swipe
            MovePiecesActual(Vector3.down);
        }
        else
        {
            board.currentState = GameState.move;
        }
    }

    void MovePiecesActual(Vector2 direction) // 바꾸는 Dot 
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];

        if (otherDot != null && board.DecreaseRowArray[column + (int)direction.x] == null && otherDot.dotState == DotState.Possible)  // 여기에 이동불가 블록 추가하여 움직이지 못하게 판단.
        {
            Vector2 otherDotPos = new Vector2(otherDot.column, otherDot.row);
            Vector2 CurrentDotPos = new Vector2(column, row);
            StartCoroutine(CheckMoveCo(otherDotPos, CurrentDotPos));
        }
        else
            board.currentState = GameState.move;
    }

    public IEnumerator CheckMoveCo(Vector2 otherDotPos, Vector2 CurrentDotPos)
    {
        board.currentDot = this;
        board.StartCoroutine(Action2D.MoveTo(transform, otherDotPos, 0.15f, true));
        board.StartCoroutine(Action2D.MoveTo(otherDot.transform, CurrentDotPos, 0.15f));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(findMatches.FindAllMatchesCo());

        if (otherDot != null)
        {
            if (FindMatches.currentMatches.Contains(this) == false && FindMatches.currentMatches.Contains(otherDot) == false)
            {
                board.StartCoroutine(Action2D.MoveTo(transform, CurrentDotPos, 0.15f, true));
                board.StartCoroutine(Action2D.MoveTo(otherDot.transform, otherDotPos, 0.15f));
                yield return new WaitForSeconds(0.2f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();

                if(board.mysticManager.B_Support_shooting)
                    Instantiate(board.mysticManager.Nomal_MysticBlock, transform.position, Quaternion.identity);
            }
        }

    }
    #endregion

}


