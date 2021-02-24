using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait, move, win, lose, pause
}

/// <summary>
/// 스페셜 블록 리스트
/// </summary>
public enum SpecialBlock
{
    None, Cross, Multiple
}
/// <summary>
/// 방해형 블록 리스트
/// 0: 랜덤 1: 참새 2: 도토리나무 3: 엮인줄기나무 4: 도토리
/// </summary>
public enum Obstructionblock
{
    None, Bird, AcornTree, StalkTree, Acorn
}



public class Board : MonoBehaviour
{
    public static Board Instance;

    public GameState currentState = GameState.move;

    [HideInInspector]
    public int width = 9;

    [HideInInspector]
    public int height = 9;

    [HideInInspector]
    public int offSet = 9;

    [Header("블록 떨어지는 스피드")]
    public float[] dropSpeed;

    [HideInInspector]
    public Level CurrentLevel;

    [Header("블록")]
    public GameObject tilePrefabs;
    public GameObject breakableTilePrefabs;
    public GameObject concreteTilePrefabs;
    public GameObject ObstructionPrefabs;

    public static GameObject[] dots;
    public GameObject destroyEffect;

    [Header("블록 타입 집합")]
    private bool[,] blankSpaces;
    private BackGroundTile[,] concreteTiles;
    public ObstructionDot[,] ObstructionDots;
    public Dot[,] allDots;
    private List<GameObject> Create_dots = new List<GameObject>();
    private GameObject Previous_Bellow;
    private GameObject Previous_Left;
    private int[,] TileSpace = new int[9, 9];

    public Dot currentDot;
    private FindMatches findMatches;

    //--------- Score---------
    public int basePieceValue = 20;
    private int streakValue = 1;

    private SoundManager soundManager;
    private GoalManager goalManager;

    [Header("이동횟수제한")]
    public int BirdLimitingMove = 0;
    public int AcornTreeLimitingMove = 0;
    public int StalkTreeLimitingMove = 0;

    private float refillDelay = 1f;
    public bool is_complete;


    private void Awake()
    {
        Instance = this;
        CurrentLevel = InfoManager.ReturnCurrentStage();
        dots = CurrentLevel.dots;
    }
    void Start()
    {

        goalManager = FindObjectOfType<GoalManager>();
        soundManager = FindObjectOfType<SoundManager>();
        findMatches = FindMatches.Instance;
        blankSpaces = new bool[width, height];
        concreteTiles = new BackGroundTile[width, height];
        ObstructionDots = new ObstructionDot[width, height];
        allDots = new Dot[width, height];

        InitList();
        Setup();

    }
    #region 초기 세팅 함수모음

    void InitList()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            Create_dots.Add(dots[i]);
        }
    }
    void Setup()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i, j);
                TileSpace[i, j] = InfoManager.ReturnCurrentStage().Tile[(8 - j) * 9 + i];
                switch (TileSpace[i, j])
                {
                    case 0:
                        NormalType(i, j);
                        break;
                    case 1:
                        blankSpaces[i, j] = true;
                        break;
                    case 2:
                        GameObject tile = Instantiate(concreteTilePrefabs, tempPosition, Quaternion.identity);
                        concreteTiles[i, j] = tile.GetComponent<BackGroundTile>();
                        break;
                    case 3:
                        GameObject Obstruction = Instantiate(ObstructionPrefabs, tempPosition, Quaternion.identity);
                        ObstructionDots[i, j] = Obstruction.GetComponent<ObstructionDot>();
                        break;
                }
            }
        }
        if (IsDeadlocked())
        {
            ShuffleBoard();
        }

        is_complete = true;
    }

    void NormalType(int i, int j)
    {
        Vector2 tempPosition = new Vector2(i, j);
        if (i > 0 && allDots[i - 1, j] != null)
            Previous_Left = previous_Obj(allDots[i - 1, j].gameObject);
        if (j > 0 && allDots[i, j - 1] != null)
            Previous_Bellow = previous_Obj(allDots[i, j - 1].gameObject);

        if (Previous_Left != null)
        {
            Create_dots.Remove(Previous_Left);
        }
        if (Previous_Bellow != null)
        {
            Create_dots.Remove(Previous_Bellow);
        }

        int dotTouse = Random.Range(0, Create_dots.Count);
        GameObject dotObj = Instantiate(Create_dots[dotTouse], tempPosition, Quaternion.identity);
        Dot dot = dotObj.GetComponent<Dot>();

        dot.row = j;
        dot.column = i;

        allDots[i, j] = dot;

        if (Previous_Left != null)
            Create_dots.Add(Previous_Left);

        if (Previous_Bellow != null && Create_dots.Contains(Previous_Bellow) == false)
            Create_dots.Add(Previous_Bellow);

        Previous_Left = null;
        Previous_Bellow = null;
    }

    GameObject previous_Obj(GameObject obj) // 똑같은 오브젝트 불러오기
    {
        for (int i = 0; i < dots.Length; i++)
        {
            if (dots[i].tag == obj.tag)
            {
                return dots[i];
            }
        }
        return null;
    }
    #endregion

    #region 방해타일 데미지입히는 함수
    private void DamageConcrete(int column, int row) // 콘크리트 타일
    {
        if (column > 0)
        {
            if (concreteTiles[column - 1, row])
            {
                concreteTiles[column - 1, row].TakeDamage(1);
                if (concreteTiles[column - 1, row].hitPoints <= 0)
                    concreteTiles[column - 1, row] = null;
            }
        }
        if (column < width - 1)
        {
            if (concreteTiles[column + 1, row])
            {
                concreteTiles[column + 1, row].TakeDamage(1);
                if (concreteTiles[column + 1, row].hitPoints <= 0)
                    concreteTiles[column + 1, row] = null;
            }
        }
        if (row > 0)
        {
            if (concreteTiles[column, row - 1])
            {
                concreteTiles[column, row - 1].TakeDamage(1);
                if (concreteTiles[column, row - 1].hitPoints <= 0)
                    concreteTiles[column, row - 1] = null;
            }
        }
        if (row < height - 1)
        {
            if (concreteTiles[column, row + 1])
            {
                concreteTiles[column, row + 1].TakeDamage(1);
                if (concreteTiles[column, row + 1].hitPoints <= 0)
                    concreteTiles[column, row + 1] = null;
            }
        }
    }

    private void DamageAcorn(int column, int row) // 도토리 타일
    {
        if (column > 0)
        {
            if (allDots[column - 1, row])
            {
                if (allDots[column - 1, row].GetComponent<Dot>().isAcorn)
                    Destroy(allDots[column - 1, row]);
            }
        }
        if (column < width - 1)
        {
            if (allDots[column + 1, row])
            {
                if (allDots[column + 1, row].GetComponent<Dot>().isAcorn)
                    Destroy(allDots[column + 1, row]);
            }
        }
        if (row > 0)
        {
            if (allDots[column, row - 1])
            {
                if (allDots[column, row - 1].GetComponent<Dot>().isAcorn)
                    Destroy(allDots[column, row - 1]);
            }
        }
        if (row < height - 1)
        {
            if (allDots[column, row + 1])
            {
                if (allDots[column, row + 1].GetComponent<Dot>().isAcorn)
                    Destroy(allDots[column, row + 1]);
            }
        }
    }

    private void DamageObstruction(int column, int row) // 콘크리트 타일
    {
        if (column > 0)
        {
            if (ObstructionDots[column - 1, row])
            {
                ObstructionDots[column - 1, row].TakeDamage(1);
                if (ObstructionDots[column - 1, row].Health <= 0)
                    ObstructionDots[column - 1, row] = null;
            }
        }
        if (column < width - 1)
        {
            if (ObstructionDots[column + 1, row])
            {
                ObstructionDots[column + 1, row].TakeDamage(1);
                if (ObstructionDots[column + 1, row].Health <= 0)
                    ObstructionDots[column + 1, row] = null;
            }
        }
        if (row > 0)
        {
            if (ObstructionDots[column, row - 1])
            {
                ObstructionDots[column, row - 1].TakeDamage(1);
                if (ObstructionDots[column, row - 1].Health <= 0)
                    ObstructionDots[column, row - 1] = null;
            }
        }
        if (row < height - 1)
        {
            if (ObstructionDots[column, row + 1])
            {
                ObstructionDots[column, row + 1].TakeDamage(1);
                if (ObstructionDots[column, row + 1].Health <= 0)
                    ObstructionDots[column, row + 1] = null;
            }
        }
    }
    #endregion

    #region 4,5 매치
    private int ColumnOrRow(Dot currentMatch) // 매치된 블록들에 대한 직접 4,5 매치 판단
    {
        List<Dot> matchCopy = findMatches.currentMatches;

        int columnMatch = 0;
        int rowMatch = 0;
        int diagonalMatch = 0;

        for (int j = 0; j < matchCopy.Count; j++) // i의 점에 대하여 
        {
            Dot nextDot = matchCopy[j];

            if (nextDot.column == currentMatch.column && nextDot.CompareTag(currentMatch.tag))
            {
                columnMatch++;
            }
            else if (nextDot.row == currentMatch.row && nextDot.CompareTag(currentMatch.tag))
            {
                rowMatch++;
            }
            else
            {
                diagonalMatch++;
            }
        }

        if (diagonalMatch > 0) // 사각형 매치시 특수블록
        {
            return 4;
        }
        if (columnMatch == 4 || rowMatch == 4) // 세로 혹은 가로 5개 매치
        {
            return 1;
        }
        if (columnMatch == 2 && rowMatch == 2) // 가로 세로 합 5개
        {
            return 2;
        }
        if (columnMatch == 3 || rowMatch == 3) // 세로 혹은 가로 4개 매치
        {
            return 3;
        }

        return 0;
    }

    private int Indirectmatching()
    {
        List<Dot> matchCopy = findMatches.currentMatches;

        for (int i = 0; i < matchCopy.Count; i++)
        {
            var column = matchCopy[i].column;
            var row = matchCopy[i].row;
            var Thistag = matchCopy[i].tag;

            var columnMatch = 0;
            var rowMatch = 0;

            //가로 5줄

            if (column - 2 >= 0 && allDots[column - 2, row] != null && allDots[column - 2, row].CompareTag(Thistag))
            {
                columnMatch++;
            }
            if (column - 1 >= 0 && allDots[column - 1, row] != null && allDots[column - 1, row].CompareTag(Thistag))
            {
                columnMatch++;
            }
            if (column + 1 < width && allDots[column + 1, row] != null && allDots[column + 1, row].CompareTag(Thistag))
            {
                columnMatch++;
            }
            if (column + 2 < width && allDots[column + 1, row] != null && allDots[column + 1, row].CompareTag(Thistag))
            {


                columnMatch++;
            }

        }
        // 만약 칼럼이 4일 경우에 +3을해보고 똑같은 Dot이 존재한다면 4매칭 취소로 한다.
        return 0;

    }

    private void CheckToMakeBombs() // 4,5매치를 한 경우
    {
        if (findMatches.currentMatches.Count > 3)
        {
            for (int i = 0; i < 2; i++)
            {
                int typeOfMatch = 0;
                Dot thisDot_ = currentDot;

                if (i == 1)
                    thisDot_ = currentDot.otherDot;

                typeOfMatch = ColumnOrRow(thisDot_);

                if (typeOfMatch != 0)
                {
                    if (findMatches.currentMatches.Contains(thisDot_))
                    {
                        findMatches.currentMatches.Remove(thisDot_);
                        //thisDot_.MakeSpecialBlock();
                    }
                }
            }
        }
    }
    #endregion

    public void DestroyMatches(bool isMove, bool Special)
    {
        if (findMatches.currentMatches.Count > 3 && isMove) // 직접 블록을 움직였을때, 4,5 매치 판단
            CheckToMakeBombs();


        for (int i = 0; i < findMatches.currentMatches.Count; i++)
        {
            var column = findMatches.currentMatches[i].column;
            var row = findMatches.currentMatches[i].row;

            if (findMatches.currentMatches[i] != null)
            {
                DamageConcrete(column, row);  // 여기 콘크리트 타일 데미지 입힌다.
                DamageAcorn(column, row); //여기에 도토리 타일 데미지 입히면된다.
                DamageObstruction(column, row);


                ObjectPool.ReturnObject(allDots[column, row].gameObject);               
                DestroyEffectPool.GetObject(column, row, allDots[column, row].gameObject);

                if (goalManager != null)
                {
                    //goalManager.CompareGoal(allDots[column, row].tag.ToString());
                    goalManager.Update_CurrentScore(basePieceValue * streakValue);
                    goalManager.UpdateGoals(allDots[column, row].tag);
                }
                // 스코어 점수에 따라 달라지도록 구현

                allDots[column, row] = null;
            }
        }

        //if (Special == false) //직접 매치 & 특수블록으로 인한 파괴가 아닌경우
        //    findMatches.RandomCreateBombs(); // 랜덤 확률로 특수, 선택 블록 생성

        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo()); // 행 내리기
    }

    public void SpecialDestroy() // 특수 블록, 선택블록에 대한 폭파
    {
        for (int i = 0; i < findMatches.currentMatches.Count; i++)
        {
            if (findMatches.currentMatches[i] != null)
            {
                var Dot = findMatches.currentMatches[i];
                ObjectPool.ReturnObject(allDots[Dot.column, Dot.row].gameObject);
                DestroyEffectPool.GetObject(Dot.column, Dot.row, allDots[Dot.column, Dot.row].gameObject);
                allDots[Dot.column, Dot.row] = null;
            }
        }
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo()); // 행 내리기
    }

    private IEnumerator DecreaseRowCo() // 행을 밑으로 내리는 함수
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j] && allDots[i, j] == null && !concreteTiles[i, j] && !ObstructionDots[i,j]) // 빈공간이 아니고, 해당열에 Dot이 없거나, 콘크리트 타일이 아닌경우
                {
                    for (int k = j + 1; k < height; k++) // 해당 열 위에서 아래로 공간 반복
                    {
                        if (allDots[i, k] != null)
                        {
                            StartCoroutine(Action2D.MoveTo(allDots[i, k], new Vector2(i, j), dropSpeed[k - j]));
                            allDots[i, k].row = j;
                            allDots[i, j] = allDots[i, k];
                            allDots[i, k] = null;
                            yield return null;
                            break;
                        }
                    }
                }
            }
            StartCoroutine(Refiilheight(i));
        }

        StartCoroutine(FillBoardCo());
    }

    private IEnumerator FillBoardCo() // 보드 리필함수 -> 매치 확인 함수 -> 데드락 확인 함수 관려 코루틴
    {
        yield return StartCoroutine(findMatches.FindAllMatchesCo());

        if (findMatches.currentMatches.Count > 0)
        {
            streakValue += 1;
            DestroyMatches(false, false);
            yield break;
        }

        currentDot = null;

        if (IsDeadlocked())
        {
            ShuffleBoard();
        }
        yield return new WaitForSeconds(refillDelay * 0.4f);
        currentState = GameState.move;
        streakValue = 1;
    }

    IEnumerator Refiilheight(int i)
    {
        var Delay = new WaitForSeconds(0.1f);

        for (int j = 0; j < height; j++)
        {
            if (allDots[i, j] == null && !blankSpaces[i, j] && !concreteTiles[i, j] && !ObstructionDots[i, j])
            {
                GameObject piece = ObjectPool.GetObject(i, j, offSet);
                allDots[i, j] = piece.GetComponent<Dot>();
                StartCoroutine(Action2D.MoveTo(allDots[i, j], new Vector2(i, j), dropSpeed[offSet - j]));
                yield return Delay;
            }
        }
    }



    public static void SetChangeDotArray(Dot CurrentDot, Vector2 OtherDot)
    {
        Dot temp_obj = Instance.allDots[(int)OtherDot.x, (int)OtherDot.y];
        Instance.allDots[(int)OtherDot.x, (int)OtherDot.y] = Instance.allDots[CurrentDot.column, CurrentDot.row];
        Instance.allDots[CurrentDot.column, CurrentDot.row] = temp_obj;
    }

    #region 데드락 함수 모음
    private void SwitchPieces(int column, int row, Vector2 direction) // 데드락 1. 위치 바꿔주는 함수
    {
        if (allDots[column + (int)direction.x, row + (int)direction.y] != null)
        {
            Dot holder = allDots[column + (int)direction.x, row + (int)direction.y];
            allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
            allDots[column, row] = holder;
        }
    }
    private bool CheckForMatches()  // 데드락 2. 매치되었는지 아닌지 확인하는 함수
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < width - 2)
                    {
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null) // 오른쪽 두개 체크
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag && allDots[i + 2, j].tag == allDots[i, j].tag)
                                return true;
                        }
                    }
                    if (j < height - 2)
                    {
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null) // 위 두개 체크
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag && allDots[i, j + 2].tag == allDots[i, j].tag)
                                return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool SwitchAndCheck(int column, int row, Vector2 direction)// 데드락 1. 위치 바꿔주는 함수 2. 매치되었는지 확인하는 함수 합친 함수
    {
        SwitchPieces(column, row, direction);
        if (CheckForMatches())
        {
            SwitchPieces(column, row, direction);
            return true;
        }
        SwitchPieces(column, row, direction);  //  체크위해서 스왑했다가 다시 되돌리기
        return false;
    }

    private bool IsDeadlocked() // 오른쪽,위 Dot 하나하나 다 검사
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }
                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
    private void ShuffleBoard() // 교착 상태일 경우 섞어주는 함수
    {
        List<GameObject> newBoard = new List<GameObject>();
        for (int i = 0; i < width; i++) // 모든 블록들을 새로운 블록에 저장한다.
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    newBoard.Add(allDots[i, j].gameObject);
                }
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j]) // 빈 공간 혹은 콘크리트 타일이 아닌경우
                {
                    if (i > 0 && allDots[i - 1, j] != null) // 왼쪽 
                        Previous_Left = previous_Obj(allDots[i - 1, j].gameObject);
                    if (j > 0 && allDots[i, j - 1] != null) // 밑에
                        Previous_Bellow = previous_Obj(allDots[i, j - 1].gameObject);

                    if (Previous_Left != null)
                    {
                        Create_dots.Remove(Previous_Left); // 왼쪽 제거
                    }
                    if (Previous_Bellow != null)
                    {
                        Create_dots.Remove(Previous_Bellow); // 아래 제거
                    }


                    int dotTouse = Random.Range(0, Create_dots.Count); // 이중에 한개를 뽑는다.
                    bool IsSuccess = false;
                    //int pieceToUse = Random.Range(0, newBoard.Count); // 사용할 블럭 수

                    for (int z = newBoard.Count - 1; 0 <= z; z--) //현재 보유 갯수
                    {
                        if (Create_dots[dotTouse].tag == newBoard[z].tag)
                        {
                            //Debug.Log("성공");
                            Dot piece;
                            piece = newBoard[z].GetComponent<Dot>();
                            piece.column = i;
                            piece.row = j;
                            allDots[i, j] = piece;
                            newBoard.Remove(newBoard[z]);
                            IsSuccess = true;
                            break;
                        }
                    }

                    if (IsSuccess == false) // 새로운 블록 생성
                    {
                        //Debug.Log("실패");
                        int dotTouse2 = Random.Range(0, Create_dots.Count);
                        GameObject dotObj = Instantiate(Create_dots[dotTouse2], new Vector2(i, j), Quaternion.identity);
                        Dot dot = dotObj.GetComponent<Dot>();
                        dot.GetComponent<Dot>().row = j;
                        dot.GetComponent<Dot>().column = i;

                        dot.transform.parent = this.transform;
                        dot.name = "(" + i + "," + j + ")";
                        allDots[i, j] = dot;

                        GameObject _newBoard = newBoard[newBoard.Count - 1];
                        newBoard.Remove(newBoard[newBoard.Count - 1]);
                        Destroy(_newBoard);
                    }

                    if (Previous_Left != null)
                        Create_dots.Add(Previous_Left);

                    if (Previous_Bellow != null && Create_dots.Contains(Previous_Bellow) == false)
                        Create_dots.Add(Previous_Bellow);

                    Previous_Left = null;
                    Previous_Bellow = null;
                }
            }
        }
        if (IsDeadlocked())
        {
            ShuffleBoard();
        }
    }
    #endregion

    //public void BombRow(int row)
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        if (concreteTiles[i, row])
    //        {
    //            concreteTiles[i, row].TakeDamage(1);
    //            if (concreteTiles[i, row].hitPoints <= 0)
    //                concreteTiles[i, row] = null;
    //        }
    //    }
    //}

    //public void BombColumn(int column)
    //{
    //    for (int i = 0; i < height; i++)
    //    {
    //        if (concreteTiles[column, i])
    //        {
    //            concreteTiles[column, i].TakeDamage(1);
    //            if (concreteTiles[column, i].hitPoints <= 0)
    //                concreteTiles[column, i] = null;
    //        }
    //    }
    //}
}
