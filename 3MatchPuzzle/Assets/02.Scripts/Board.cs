﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait, move, win, lose, pause
}

public enum TileKind
{
    Breakable, Blank, Normal, Lock, Concrete, Slime
}
[System.Serializable]
public class TileType
{
    public int x;
    public int y;
    public TileKind tileKind;
}

public class Board : MonoBehaviour
{
    [Header("Scriptable Object Stuff")]
    public World world;
    public int level;

    public GameState currentState = GameState.move;
    [Header("Board Dimensions")]
    public int width;
    public int height;
    public int offSet;

    [Header("Prefabs")]
    public GameObject tilePrefabs;
    public GameObject breakableTilePrefabs;
    public GameObject lockTilePrefabs;
    public GameObject concreteTilePrefabs;
    public GameObject[] dots;
    public GameObject destroyEffect;

    [Header("Layout")]
    public TileType[] boardLayout;
    private bool[,] blankSpaces;
    private BackGroundTile[,] breakableTiles;
    public BackGroundTile[,] lockTiles;
    private BackGroundTile[,] concreteTiles;
    public GameObject[,] allDots;
    private List<GameObject> Create_dots = new List<GameObject>();
    private GameObject Previous_Bellow;
    private GameObject Previous_Left;

    public Dot currentDot;
    private FindMatches findMatches;
    //--------- Score---------
    public int basePieceValue = 20;
    private int streakValue = 1;
    private ScoreManager scoreManager;
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
        if (world != null)
        {
            if (level < world.levels.Length)
            {
                if (world.levels[level] != null)
                {
                    width = world.levels[level].width;
                    height = world.levels[level].height;
                    dots = world.levels[level].dots;
                    boardLayout = world.levels[level].boardLayout;
                }
            }
        }
    }
    void Start()
    {
        InitList();
        goalManager = FindObjectOfType<GoalManager>();
        soundManager = FindObjectOfType<SoundManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        breakableTiles = new BackGroundTile[width, height];
        lockTiles = new BackGroundTile[width, height];
        concreteTiles = new BackGroundTile[width, height];
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces = new bool[width, height];
        allDots = new GameObject[width, height];
        Setup();

    }
    #region 초기 세팅 함수모음
    public void GenerateBlankSpaces()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }

    public void GenerateBreakableTiles()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefabs, tempPosition, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackGroundTile>();
            }
        }
    }

    public void GenerateLockTiles()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Lock)
            {
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(lockTilePrefabs, tempPosition, Quaternion.identity);
                lockTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackGroundTile>();
            }
        }
    }
    public void GenerateConcreteTiles()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Concrete)
            {
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(concreteTilePrefabs, tempPosition, Quaternion.identity);
                concreteTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackGroundTile>();
            }
        }
    }

    void InitList()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            Create_dots.Add(dots[i]);
        }
    }
    void Setup()
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();
        GenerateLockTiles();
        GenerateConcreteTiles();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j] && !concreteTiles[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    //GameObject backgroundTile = Instantiate(tilePrefabs, tempPosition, Quaternion.identity);
                    //backgroundTile.transform.parent = this.transform;
                    //backgroundTile.name = "(" + i + "," + j + ")";

                    if (i > 0 && allDots[i - 1, j] != null)
                        Previous_Left = previous_Obj(allDots[i - 1, j]);
                    if (j > 0 && allDots[i, j - 1] != null)
                        Previous_Bellow = previous_Obj(allDots[i, j - 1]);

                    if (Previous_Left != null)
                    {
                        Create_dots.Remove(Previous_Left);
                    }
                    if (Previous_Bellow != null)
                    {
                        Create_dots.Remove(Previous_Bellow);
                    }

                    int dotTouse = Random.Range(0, Create_dots.Count);
                    GameObject dot = Instantiate(Create_dots[dotTouse], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().row = j;
                    dot.GetComponent<Dot>().column = i;

                    dot.transform.parent = this.transform;
                    dot.name = "(" + i + "," + j + ")";
                    allDots[i, j] = dot;

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
        is_complete = true;
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
            if (concreteTiles[column, row + 1])
            {
                if (allDots[column, row + 1])
                {
                    if (allDots[column, row + 1].GetComponent<Dot>().isAcorn)
                        Destroy(allDots[column, row + 1]);
                }
            }
        }
    }
    #endregion
    private int ColumnOrRow() // 매치된 블록들에 대한 4,5 매치 판단
    {
        List<GameObject> matchCopy = findMatches.currentMatches as List<GameObject>;

        for (int i = 0; i < matchCopy.Count; i++)
        {
            Dot thisDot = matchCopy[i].GetComponent<Dot>();

            int column = thisDot.column;
            int row = thisDot.row;
            int columnMatch = 0;
            int rowMatch = 0;

            for (int j = 0; j < matchCopy.Count; j++) // i의 점에 대하여 
            {
                Dot nextDot = matchCopy[j].GetComponent<Dot>();
                if (nextDot == thisDot)
                    continue;
                if (nextDot.column == thisDot.column && nextDot.CompareTag(thisDot.tag))
                {
                    columnMatch++;
                }
                if (nextDot.row == thisDot.row && nextDot.CompareTag(thisDot.tag))
                {
                    rowMatch++;
                }
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
        }
        return 0;
    }

    private void CheckToMakeBombs() // 이건 내가 실제로 4,5매치를 한 경우
    {
        if (findMatches.currentMatches.Count > 3)
        {
            int typeOfMatch = ColumnOrRow();
            if (typeOfMatch != 0)
            {
                findMatches.CheckBombs();
            }
        }
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {

            if (breakableTiles[column, row] != null) // 부셔지는 타입의 타일을 부시는 조건문
            {
                breakableTiles[column, row].TakeDamage(1);
                if (breakableTiles[column, row].hitPoints <= 0)
                    breakableTiles[column, row] = null;
            }

            if (lockTiles[column, row] != null) // 부셔지는 타입의 타일을 부시는 조건문
            {
                lockTiles[column, row].TakeDamage(1);
                if (lockTiles[column, row].hitPoints <= 0)
                    lockTiles[column, row] = null;
            }
            DamageConcrete(column, row);  // 여기 콘크리트 타일 데미지 입힌다.
            DamageAcorn(column, row); //여기에 도토리 타일 데미지 입히면된다.


            //findMatches.currentMatches.Remove(allDots[column, row]);

            if (goalManager != null)
            {
                goalManager.CompareGoal(allDots[column, row].tag.ToString());
                goalManager.UpdateGoals();
            }

            //if(soundManager != null)
            //{
            //    soundManager.PlayRandomDestroyNoise();
            //}
            GameObject destroyEffect_ = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(destroyEffect_, 2.0f);
            Destroy(allDots[column, row]);
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches(bool isMove, bool Special)
    {
        if (findMatches.currentMatches.Count > 3 && isMove) // 직접 블록을 움직였을때, 4,5 매치 판단
            CheckToMakeBombs();
        //Debug.Log("DestroyMatches");
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                    DestroyMatchesAt(i, j);
            }
        }
        if (Special == false) //직접 매치 & 특수블록으로 인한 파괴가 아닌경우
            findMatches.RandomCreateBombs(); // 랜덤 확률로 특수, 선택 블록 생성
        Debug.Log("매치된 갯수     " + findMatches.currentMatches.Count);
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2()); // 행 내리기
    }
    private IEnumerator DecreaseRowCo2() // 행을 밑으로 내리는 함수
    {
        //Debug.Log("DecreaseRowCo2");
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j] && allDots[i, j] == null && !concreteTiles[i, j]) // 빈공간, 해당열에 Dot이 없거나, 콘크리트 타일이 아닌경우
                {
                    for (int k = j + 1; k < height; k++) // 해당 열 위에서 아래로 공간 반복
                    {
                        if (allDots[i, k] != null)
                        {
                            allDots[i, k].GetComponent<Dot>().row = j;
                            allDots[i, k] = null;
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(refillDelay * 0.3f);
        StartCoroutine(FillBoardCo());
    }

    private IEnumerator FillBoardCo() // 보드 리필함수 -> 매치 확인 함수 -> 데드락 확인 함수 관려 코루틴
    {
        //Debug.Log("FillBoardCo");
        yield return new WaitForSeconds(refillDelay * 0.5f);
        RefillBoard();
        yield return null;
        findMatches.FIndAllMathces();
        yield return new WaitForSeconds(refillDelay * 0.4f);
        while (MatchesOnboard()) //채워진 곳에 대해 매치에 대한 부분 검사
        {
            //yield return null;
            streakValue += 1;
            DestroyMatches(false, false);
            yield break;
        }
        //findMatches.currentMatches.Clear();
        currentDot = null;

        if (IsDeadlocked())
        {
            //Debug.Log("DeadLocked!!");
            ShuffleBoard();
        }
        yield return new WaitForSeconds(refillDelay * 0.4f);
        currentState = GameState.move;
        streakValue = 1;
    }


    private void RefillBoard() // 보드에 리필해주는 함수
    {
        //Debug.Log("RefillBoard");
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null && !blankSpaces[i, j] && !concreteTiles[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }

            }
        }
    }

    private bool MatchesOnboard() //여기 파인드 매치와 뒤 반복문에 대한 시간차를 둔다.
    {
        //Debug.Log("MatchesOnboard");
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                        return true;
                }
            }
        }
        return false;
    }

    public void SpecialDestroy()
    {
        for(int i = 0; i<findMatches.currentMatches.Count; i++)
        {
            if (findMatches.currentMatches[i] != null)
            {
                GameObject destroyEffect_ = Instantiate(destroyEffect, findMatches.currentMatches[i].transform.position, Quaternion.identity);
                Destroy(destroyEffect_, 2.0f);
                Destroy(findMatches.currentMatches[i]);
                scoreManager.IncreaseScore(basePieceValue * streakValue);
                allDots[findMatches.currentMatches[i].GetComponent<Dot>().column, findMatches.currentMatches[i].GetComponent<Dot>().row] = null;
            }
        }
        findMatches.currentMatches.Clear();

     
        StartCoroutine(DecreaseRowCo2()); // 행 내리기
    }

    #region 데드락 함수 모음
    private void SwitchPieces(int column, int row, Vector2 direction) // 데드락 1. 위치 바꿔주는 함수
    {
        if (allDots[column + (int)direction.x, row + (int)direction.y] != null)
        {
            GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;
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
                    newBoard.Add(allDots[i, j]);
                }
            }
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j] && !concreteTiles[i, j]) // 빈 공간 혹은 콘크리트 타일이 아닌경우
                {
                    if (i > 0 && allDots[i - 1, j] != null) // 왼쪽 
                        Previous_Left = previous_Obj(allDots[i - 1, j]);
                    if (j > 0 && allDots[i, j - 1] != null) // 밑에
                        Previous_Bellow = previous_Obj(allDots[i, j - 1]);

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
                            allDots[i, j] = newBoard[z];
                            newBoard.Remove(newBoard[z]);
                            IsSuccess = true;
                            break;
                        }
                    }

                    if (IsSuccess == false) // 새로운 블록 생성
                    {
                        //Debug.Log("실패");
                        int dotTouse2 = Random.Range(0, Create_dots.Count);
                        GameObject dot = Instantiate(Create_dots[dotTouse2], new Vector2(i, j), Quaternion.identity);
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

    public void BombRow(int row)
    {
        for (int i = 0; i < width; i++)
        {
            if (concreteTiles[i, row])
            {
                concreteTiles[i, row].TakeDamage(1);
                if (concreteTiles[i, row].hitPoints <= 0)
                    concreteTiles[i, row] = null;
            }
        }
    }

    public void BombColumn(int column)
    {
        for (int i = 0; i < height; i++)
        {
            if (concreteTiles[column, i])
            {
                concreteTiles[column, i].TakeDamage(1);
                if (concreteTiles[column, i].hitPoints <= 0)
                    concreteTiles[column, i] = null;
            }
        }
    }
}