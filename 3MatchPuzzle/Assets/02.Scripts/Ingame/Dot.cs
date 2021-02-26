using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Dot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("현재 포지션 위치")]
    public int column;
    public int row;

    private HintManager hintManager;
    private FindMatches findMatches;
    private GoalManager goalManager;
    private Board board;
    public Dot otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    public float swipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Bool Type = 특수블록")]
    public SpecialBlock specialBlock = SpecialBlock.None;
    [HideInInspector]
    public Obstructionblock obstructionblock = Obstructionblock.None;

    // 가로 세로 가위
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isSlingShot;
    public GameObject SlingShotTarget;
    public bool isAxe;
    public bool isAcornBoom;

    public GameObject diagonalArrow;
    public GameObject CrossArrow;
    [Header("Bool Type = 방해블록")]
    public bool isAcorn;
    public bool isAcornTree;
    public bool isStalkTree;
    public GameObject StalkTree_obj;
    public bool isSpreader;
    public bool isBird;


    [Header("특수/방해블록 이미지")]
    public Sprite[] SpecialImg;
    public Sprite[] Obstruction;
    private SpriteRenderer DotSprite;

    void Start()
    {
        DotSprite = GetComponent<SpriteRenderer>();
        goalManager = FindObjectOfType<GoalManager>();
        hintManager = FindObjectOfType<HintManager>();
        findMatches = FindObjectOfType<FindMatches>();
        board = FindObjectOfType<Board>();


    }

    public void OnPointerDown(PointerEventData eventData) // 선택형 매치 판별
    {
        if (hintManager != null)
            hintManager.DestroyHint();

        if (specialBlock != SpecialBlock.None)
        {
            findMatches.SpecialSkill(this);
            return;
        }

        if (board.currentState == GameState.move && !IsSpecialBlock())
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (specialBlock != SpecialBlock.None)
            return;

        if (board.currentState == GameState.move && !IsSpecialBlock())
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

        if (otherDot != null && !otherDot.IsSpecialBlock())  // 여기에 이동불가 블록 추가하여 움직이지 못하게 판단.
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
        board.StartCoroutine(Action2D.MoveTo(this, otherDotPos, 0.15f, true));
        board.StartCoroutine(Action2D.MoveTo(otherDot, CurrentDotPos, 0.15f));
        yield return new WaitForSeconds(.2f);
        yield return StartCoroutine(findMatches.FindAllMatchesCo());

        if (otherDot != null)
        {
            if (!findMatches.currentMatches.Contains(this) && !findMatches.currentMatches.Contains(otherDot))
            {
                board.StartCoroutine(Action2D.MoveTo(this, CurrentDotPos, 0.15f, true));
                board.StartCoroutine(Action2D.MoveTo(otherDot, otherDotPos, 0.15f));
                yield return new WaitForSeconds(0.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                goalManager.Update_CurrentTimeMoveCount();
                board.DestroyMatches(true, true);
            }
        }

    }
    #endregion

    public void MakeSpecialBlock()// 선택형 특수블록 생성
    {
        int Length = System.Enum.GetNames(typeof(SpecialBlock)).Length;
        int Rannum = Random.Range(1, Length);
        specialBlock = (SpecialBlock)Rannum;
        DotSprite.sprite = SpecialImg[Rannum - 1];
    }


    public bool SpecialBlockCheck() // 특수/방해 블록 체크
    {
        if (isAcorn || isColumnBomb || isRowBomb || isBird || isAcornTree || isStalkTree || isSlingShot || isAxe || isAcornBoom)
            return false;
        else
            return true;
    }

    public bool isHinderBlock() // 방해블록 체크
    {
        if (isBird || isAcornTree || isStalkTree || isAcorn)
            return true;
        else
            return false;
    }

    public bool IsSpecialBlock() // 직접 매치가 안되는 블록 체크
    {
        if (specialBlock != SpecialBlock.None)
            return true;
        else
            return false;
    }
}


//public void MakematchingBomb() // 매치형 특수블록 생성
//{
//    int Rannum = Random.Range(0, 2);
//    if (Rannum == 0) // 크로스
//    {
//        isCrossArrow = true;
//        GameObject arrow = Instantiate(CrossArrow, transform.position, Quaternion.identity);
//        arrow.transform.parent = this.transform;
//    }
//    else if (Rannum == 1)
//    {
//        isDiagonal = true;
//        GameObject arrow = Instantiate(diagonalArrow, transform.position, Quaternion.identity);
//        arrow.transform.parent = this.transform;
//    }
//}

//public void CreateHinderBlock(int i, bool Spreader) // 방해블록 생성
//{
//    GetComponent<SpriteRenderer>().color = Color.white;
//    obstructionblock = (Obstructionblock)(i);

//    switch(obstructionblock)
//    {
//        case Obstructionblock.None:
//            int Rannum = Random.Range(0, 3);
//            CreateHinderBlock(Rannum, true);
//            break;
//        case Obstructionblock.Bird:
//            break;
//        case Obstructionblock.AcornTree:
//            if (findMatches.AcornTree_Exist())
//            {
//                obstructionblock = Obstructionblock.None;
//                int Rannum2 = Random.Range(0, 3);
//                CreateHinderBlock(Rannum2, true);
//            }
//            break;
//        case Obstructionblock.StalkTree:
//            if (Spreader == true) //초기 엮인 블록 생성자
//            {
//                isSpreader = true;
//                GameObject StalkTree_ = Instantiate(StalkTree_obj, transform.position, Quaternion.identity);
//                StalkTree_.transform.parent = this.transform;
//            }
//            else
//            {
//                if (matchblock != Matchblock.None) // 매치형 블록일 경우
//                {
//                    matchblock = Matchblock.None;
//                    Destroy(this.transform.GetChild(0));
//                }
//                else if (selectblock == Selectblock.ColumnBomb || selectblock == Selectblock.RowBomb) // 선택형 블록일 경우
//                {
//                    int Radnum = Random.Range(0, board.world.levels[board.level].dots.Length);
//                    GameObject dot = board.world.levels[board.level].dots[Radnum];
//                    this.tag = dot.tag;
//                    this.GetComponent<SpriteRenderer>().sprite = dot.GetComponent<SpriteRenderer>().sprite;
//                }
//                else // 일반 블록일 경우
//                {
//                    GameObject StalkTree_ = Instantiate(StalkTree_obj, transform.position, Quaternion.identity);
//                    StalkTree_.transform.parent = this.transform;
//                }
//            }
//            break;
//        case Obstructionblock.Acorn:
//            break;
//    }       
//}

//public void SlingShot_Target()
//{
//    GameObject arrow = Instantiate(SlingShotTarget, transform.position, Quaternion.identity);
//    arrow.transform.parent = this.transform;
//} // 새총 과녁 생성

