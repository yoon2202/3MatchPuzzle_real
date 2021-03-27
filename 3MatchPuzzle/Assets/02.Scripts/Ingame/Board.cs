using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Board : MonoBehaviour
{
    public static Board Instance;

    public GameState currentState = GameState.move;

    [HideInInspector]
    public int width = 9;

    [HideInInspector]
    public int height = 9;

    [HideInInspector]
    public int offSet = 11;

    [Header("블록 떨어지는 스피드")]
    public float[] dropSpeed;

    [HideInInspector]
    public Level CurrentLevel;

    [Header("블록")]
    public GameObject concreteTilePrefabs;
    public GameObject ObstructionPrefabs;

    public static GameObject[] dots;

    private bool[,] blankSpaces;
    private BackGroundTile[,] concreteTiles;

    public Obstruction_Abstract[,] ObstructionDots;
    public Queue<Obstruction_Abstract> Obstruction_Queue;

    public Mystic_Abstract[,] MysticDots;

    public Dot[,] allDots;

    private List<GameObject> Create_dots = new List<GameObject>();
    private GameObject Previous_Bellow;
    private GameObject Previous_Left;
    private int[,] TileSpace = new int[9, 9];

    public Dot currentDot;

    [HideInInspector]
    public Coroutine[] DecreaseRowArray = new Coroutine[9]; 

    private FindMatches findMatches;
    private SoundManager soundManager;
    private GoalManager goalManager;
    private ScoreManager scoreManager;

    private float refillDelay = 1f;

    [HideInInspector]
    public bool b_matching = false;

    private bool b_playStart = false;
    public bool b_PlayStart
    {
        get => b_playStart;
        set
        {
            b_playStart = value;

            if (b_playStart == true)
            {
                goalManager.Time_CountDown();
            }
        }
    }

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
        findMatches = FindObjectOfType<FindMatches>();
        scoreManager = FindObjectOfType<ScoreManager>();

        blankSpaces = new bool[width, height];
        concreteTiles = new BackGroundTile[width, height];
        ObstructionDots = new Obstruction_Abstract[width, height];
        Obstruction_Queue = new Queue<Obstruction_Abstract>();
        MysticDots = new Mystic_Abstract[width, height];
        allDots = new Dot[width, height];

        InitList();
        Setup();
        findMatches.FIndAllMathces();
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
                        ObstructionDots[i, j] = Obstruction.GetComponent<Obstruction_Abstract>();
                        break;
                }
            }
        }
        //if (IsDeadlocked())
        //{
        //    ShuffleBoard();
        //}

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
    #endregion

    public void DestroyMatches(bool isMove, bool Special)
    {
        List<int> columnList = new List<int>();

        while(FindMatches.currentMatches.Count > 0)
        {
            Dot dot = FindMatches.currentMatches.Dequeue();

            var column = dot.column;
            var row = dot.row;

            if (dot != null && dot.dotState == DotState.Possible)
            {
                DamageConcrete(column, row);  // 여기 콘크리트 타일 데미지 입힌다.
                DamageAcorn(column, row); //여기에 도토리 타일 데미지 입히면된다.

                DestroyEffectPool.GetObject(column, row, dot.gameObject);
                ObjectPool.ReturnObject(dot.gameObject);

                if (goalManager != null)
                {
                    goalManager.Update_CurrentGage(3);
                    goalManager.Update_CurrentScore((int)scoreManager.GetScore());
                }

                columnList.Add(column);
            }
        }
        //findMatches.currentMatches.Clear();

        columnList = columnList.Distinct().ToList();

        foreach (int i in columnList)
        {
            if (DecreaseRowArray[i] != null)
            {
                StopCoroutine(DecreaseRowArray[i]);
                Debug.Log("코루틴 정지");
            }

            DecreaseRowArray[i] = StartCoroutine(DecreaseRowCo(i)); // 행 내리기
        }

        //yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator DecreaseRowCo(int i) // 행을 밑으로 내리는 함수
    {
        yield return new WaitForSeconds(0.15f);

        List<State> dot = new List<State>();

        for (int j = 0; j < height; j++)
        {
            if (!blankSpaces[i, j] && allDots[i, j] == null && ObstructionDots[i, j] == null && MysticDots[i, j] == null)
            {
                for (int k = j + 1; k < height; k++) // 해당 열 위에서 아래로 공간 반복
                {
                    if (allDots[i, k] != null)
                    {

                        if (allDots[i, k].dotState == DotState.Targeted)
                        {
                            yield return new WaitUntil(() => allDots[i, k].dotState != DotState.Targeted);
                            yield break;
                        }

                        StartCoroutine(Action2D.MoveTo(allDots[i, k].transform, new Vector2(i, j), dropSpeed[k - j] * 1.5f));
                        allDots[i, j] = allDots[i, k];
                        allDots[i, k] = null;
                        dot.Add(allDots[i, j]);
                        yield return null;
                        break;
                    }
                    else if (ObstructionDots[i, k] != null)
                    {
                        if (ObstructionDots[i, k].dotState == DotState.Targeted)
                        {
                            yield return new WaitUntil(() => ObstructionDots[i, k].dotState != DotState.Targeted);
                            break;
                        }

                        StartCoroutine(Action2D.MoveTo(ObstructionDots[i, k].transform, new Vector2(i, j), dropSpeed[k - j] * 1.5f));
                        ObstructionDots[i, j] = ObstructionDots[i, k];
                        ObstructionDots[i, k] = null;
                        dot.Add(ObstructionDots[i, j]);
                     
                        yield return null;
                        break;
                    }
                    else if (MysticDots[i, k] != null)
                    {
                        if (MysticDots[i, k].dotState == DotState.Targeted)
                        {
                            yield return new WaitUntil(() => MysticDots[i, k].dotState != DotState.Targeted);
                            break;
                        }

                        StartCoroutine(Action2D.MoveTo(MysticDots[i, k].transform, new Vector2(i, j), dropSpeed[k - j] * 1.5f));
                        MysticDots[i, j] = MysticDots[i, k];
                        MysticDots[i, k] = null;
                        dot.Add(MysticDots[i, j]);
                        yield return null;
                        break;
                    }
                }
            }
        }

        var Delay = new WaitForSeconds(0.13f);

        yield return Delay;

        for (int j = 0; j < height; j++)
        {

            if (allDots[i, j] == null && !blankSpaces[i, j] && !ObstructionDots[i, j] && !MysticDots[i, j])
            {
                GameObject piece = ObjectPool.GetObject(i, j, offSet);
                allDots[i, j] = piece.GetComponent<Dot>();
                StartCoroutine(Action2D.MoveTo(allDots[i, j].transform, new Vector2(i, j), dropSpeed[offSet - j] * 1.5f));
                yield return Delay;
            }
        }


        yield return new WaitUntil(() => dot.Where(x => x.dotState == DotState.Moving).Count() == 0);
        DecreaseRowArray[i] = null;

        StartCoroutine(FillBoardCo());
    }

    private IEnumerator FillBoardCo() // 보드 리필함수 -> 매치 확인 함수
    {
        if (findMatches.FindAllMatche != null)
        {
            findMatches.StopCoroutine(findMatches.FindAllMatche);
            Debug.Log("Find match 코루틴 정지");
        }

        yield return findMatches.FindAllMatche = findMatches.StartCoroutine(findMatches.FindAllMatchesCo());

        if (FindMatches.currentMatches.Count > 0)
        {
            DestroyMatches(false, false);
            yield break;
        }

        currentDot = null;

        yield return new WaitForSeconds(refillDelay * 0.4f);
        b_matching = false;
        currentState = GameState.move;
    }

    public static void SetChangeDotArray(Dot CurrentDot, Vector2 OtherDot)
    {
        Dot temp_obj = Instance.allDots[(int)OtherDot.x, (int)OtherDot.y];
        Instance.allDots[(int)OtherDot.x, (int)OtherDot.y] = Instance.allDots[CurrentDot.column, CurrentDot.row];
        Instance.allDots[CurrentDot.column, CurrentDot.row] = temp_obj;
    }

    public static void Destroy_DecreaseRow(Transform Block)
    {
        if (Instance.DecreaseRowArray[(int)Block.position.x] != null)
        {
            Instance.StopCoroutine(Instance.DecreaseRowArray[(int)Block.position.x]);
            Debug.Log("코루틴 정지");
        }

        if (Block.GetComponent<Obstruction_Abstract>())
        {
            Instance.ObstructionDots[(int)Block.position.x, (int)Block.position.y] = null;
            Instance.Obstruction_Queue.Dequeue();
        }
        else if (Block.GetComponent<Mystic_Abstract>())
        {
            Instance.MysticDots[(int)Block.position.x, (int)Block.position.y] = null;
        }

        Instance.DecreaseRowArray[(int)Block.position.x] = Instance.StartCoroutine(Instance.DecreaseRowCo((int)Block.position.x));

        Destroy(Block.gameObject);
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
        //if (IsDeadlocked())
        //{
        //    ShuffleBoard();
        //}
    }
    #endregion
}
