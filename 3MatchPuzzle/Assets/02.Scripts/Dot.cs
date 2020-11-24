using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Dot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int temp_Column;
    public int temp_Row;
    public int targetX;
    public int targetY;

    private EndManager endManager;
    private HintManager hintManager;
    private FindMatches findMatches;
    private Board board;
    public Dot otherDot;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Bool Type = 특수블록")]
    [HideInInspector]
    public Selectblock selectblock = Selectblock.None;
    [HideInInspector]
    public Matchblock matchblock = Matchblock.None;
    [HideInInspector]
    public Obstructionblock obstructionblock = Obstructionblock.None;

    // 가로 세로 가위
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isSlingShot;
    public GameObject SlingShotTarget;
    public bool isAxe;
    public bool isAcornBoom;
    // X + 매칭 블록 
    public bool isCrossArrow;
    public bool isDiagonal;
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
    public Sprite[] MatchImg;
    public Sprite[] SelectImg;
    public Sprite[] Obstruction;
    private Sprite DotSprite;




    void Start()
    {
        DotSprite = GetComponent<SpriteRenderer>().sprite;
        endManager = FindObjectOfType<EndManager>();
        hintManager = FindObjectOfType<HintManager>();
        board = FindObjectOfType<Board>();
        findMatches = FindMatches.Instance;

    }

    private void OnEnable()
    {
        targetX = column;
        targetY = row;
    }

    void Update()
    {

        targetX = column;
        targetY = row;

        if (Mathf.Abs(targetX - transform.position.x) > .1)  // 행, 열이 바뀌는순간 배열상태에서 업데이트가 진행되고 매치된것들을 찾은다음에 Destroy 함수가 이루어진다.
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, Time.deltaTime * 15f);
            //if (board.allDots[column, row] != this)
            //{
            //    board.allDots[column, row] = this;
            //}
        }
        else
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this;
            this.gameObject.name = "(" + column + "," + row + ")";
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, Time.deltaTime * 15f);
            //if (board.allDots[column, row] != this)
            //{
            //    board.allDots[column, row] = this;
            //}
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this;
            this.gameObject.name = "(" + column + "," + row + ")";
        }
    }

    public void OnPointerDown(PointerEventData eventData) // 선택형 매치 판별
    {
        if (hintManager != null)
            hintManager.DestroyHint();

        #region 선택형 특수블록
        if (isColumnBomb)
        {
            findMatches.isColmnBomb(this);
            return;
        }
        else if (isRowBomb)
        {
            findMatches.isRowBomb(this);
            return;
        }
        else if (isSlingShot)
        {
            findMatches.SlingShot_Skill(column, row);
        }
        else if (isAxe)
        {
            findMatches.Axe_Skill(column, row);
        }
        else if (isAcornBoom)
        {
            findMatches.AcornBoom_Skill(GetComponent<Dot>());
        }
        #endregion

        if (board.currentState == GameState.move && Cannotmove(GetComponent<Dot>()))
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isColumnBomb || isRowBomb || isSlingShot || isAxe || isAcornBoom) // 선택블록
        {
            return;
        }

        if (board.currentState == GameState.move && Cannotmove(GetComponent<Dot>()))
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    void CalculateAngle()
    { // 두 게임오브젝트 사이의 각도를 알기위해 Atan2를 사용 -> 두점사이에 길이를 통해 각도를 알아낸다.
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MoviePieces();
            board.currentDot = this;
        }
        else
            board.currentState = GameState.move;
    }
    void MovePiecesActual(Vector2 direction) // 바꾸는 Dot 
    {
        otherDot = board.allDots[column + (int)direction.x, row + (int)direction.y];

        if (otherDot != null && Cannotmove(otherDot.GetComponent<Dot>()))  // 여기에 이동불가 블록 추가하여 움직이지 못하게 판단.
        {
            otherDot.column += -1 * (int)direction.x;
            otherDot.row += -1 * (int)direction.y;
            column += (int)direction.x;
            row += (int)direction.y;
            StartCoroutine(CheckMoveCo());
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

    public IEnumerator CheckMoveCo()
    {
        yield return StartCoroutine(findMatches.FindAllMatchesCo());

        if (otherDot != null)
        {
            if (!findMatches.currentMatches.Contains(this) && !findMatches.currentMatches.Contains(otherDot))
            {
                temp_Row = otherDot.GetComponent<Dot>().row;
                temp_Column = otherDot.GetComponent<Dot>().column;
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = temp_Row;
                column = temp_Column;

                UpdateBoardReplace(otherDot);
                yield return new WaitForSeconds(0.5f);
                board.currentDot = null;
                board.currentState = GameState.move;
            }
            else
            {
                if (endManager != null)
                {
                    if (endManager.requirements.gameType == GameType.Odd)
                    {
                        endManager.DecreaseCounterValue();
                    }
                }

                board.DestroyMatches(true, true);

                int Createpercent = Random.Range(0, 10); // 이거로 확률 계산 가능.
                if (Createpercent < 1)
                    findMatches.RandomCreateHinder(3);

                findMatches.Bird_AcornTree_Check();
            }
        }

    }
    private void UpdateBoardReplace(Dot a_OtherDot)
    {
        Dot temp_obj = board.allDots[column, row];
        board.allDots[a_OtherDot.column, a_OtherDot.row] = board.allDots[column, row];
        board.allDots[column, row] = temp_obj;
        Debug.Log(board.allDots[column, row].name);
    }

    public void MakematchingBomb() // 매치형 특수블록 생성
    {
        int Rannum = Random.Range(0, 2);
        if (Rannum == 0) // 크로스
        {
            isCrossArrow = true;
            GameObject arrow = Instantiate(CrossArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
        else if (Rannum == 1)
        {
            isDiagonal = true;
            GameObject arrow = Instantiate(diagonalArrow, transform.position, Quaternion.identity);
            arrow.transform.parent = this.transform;
        }
    }

    public void SelectmatchingBomb()// 선택형 특수블록 생성
    {
        int Rannum = Random.Range(1, 6);
        selectblock = (Selectblock)Rannum;
        GetComponent<SpriteRenderer>().color = Color.white;
        DotSprite = SelectImg[Rannum - 1];
    }

    public void CreateHinderBlock(int i, bool Spreader) // 방해블록 생성
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        obstructionblock = (Obstructionblock)(i + 1);

        switch(obstructionblock)
        {
            case Obstructionblock.None:
                int Rannum = Random.Range(0, 3);
                CreateHinderBlock(Rannum, true);
                break;
            case Obstructionblock.Bird:
                break;
            case Obstructionblock.AcornTree:
                if (findMatches.AcornTree_Exist())
                {
                    obstructionblock = Obstructionblock.None;
                    int Rannum2 = Random.Range(0, 3);
                    CreateHinderBlock(Rannum2, true);
                }
                break;
            case Obstructionblock.StalkTree:
                if (Spreader == true) //초기 엮인 블록 생성자
                {
                    isSpreader = true;
                    GameObject StalkTree_ = Instantiate(StalkTree_obj, transform.position, Quaternion.identity);
                    StalkTree_.transform.parent = this.transform;
                }
                else
                {
                    if (matchblock != Matchblock.None) // 매치형 블록일 경우
                    {
                        matchblock = Matchblock.None;
                        Destroy(this.transform.GetChild(0));
                    }
                    else if (selectblock == Selectblock.ColumnBomb || selectblock == Selectblock.RowBomb) // 선택형 블록일 경우
                    {
                        int Radnum = Random.Range(0, board.world.levels[board.level].dots.Length);
                        GameObject dot = board.world.levels[board.level].dots[Radnum];
                        this.tag = dot.tag;
                        this.GetComponent<SpriteRenderer>().sprite = dot.GetComponent<SpriteRenderer>().sprite;
                    }
                    else // 일반 블록일 경우
                    {
                        GameObject StalkTree_ = Instantiate(StalkTree_obj, transform.position, Quaternion.identity);
                        StalkTree_.transform.parent = this.transform;
                    }
                }
                break;
            case Obstructionblock.Acorn:
                obstructionblock = Obstructionblock.Acorn;
                break;
        }       
    }

    public void SlingShot_Target()
    {
        GameObject arrow = Instantiate(SlingShotTarget, transform.position, Quaternion.identity);
        arrow.transform.parent = this.transform;
    } // 새총 과녁 생성

    public bool SpecialBlockCheck() // 특수/방해 블록 체크
    {
        if (isAcorn || isColumnBomb || isDiagonal || isRowBomb || isCrossArrow || isBird || isAcornTree || isStalkTree || isSlingShot || isAxe || isAcornBoom)
            return false;
        else
            return true;
    }
    public bool Cannotmove(Dot otherDot) // Other Dot 이동불가 체크 
    {
        if (otherDot.isBird || otherDot.isAcornTree || otherDot.isStalkTree)
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

    public bool noMatchBlock() // 직접 매치가 안되는 블록 체크
    {
        if (isColumnBomb || isRowBomb || isBird || isAcorn || isStalkTree || isAcornTree || isAxe || isSlingShot || isAcornBoom)
        {
            return true;
        }
        else
            return false;
    }
}
