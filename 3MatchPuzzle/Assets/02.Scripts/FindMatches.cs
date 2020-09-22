using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();
    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FIndAllMathces()
    {
        StartCoroutine(FindAllMatchesCo());
    }
    // 각각의 특수폭탄에서 이루어지는 매칭되는 블록들의 모집을 왜하는건지 모르겟다.
    private List<GameObject> isCrossBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();

        if (dot1.isCrossArrow)
            currentDots.Union(GetCrossPieces(dot1.column, dot1.row));
        if (dot2.isCrossArrow)
            currentDots.Union(GetCrossPieces(dot2.column, dot2.row));
        if (dot3.isCrossArrow)
            currentDots.Union(GetCrossPieces(dot3.column, dot3.row));

        return currentDots;
    }

    private List<GameObject> isDiagonalBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();

        if (dot1.isDiagonal)
            currentDots.Union(GetdiagonalPieces(dot1.column, dot1.row));
        if (dot2.isDiagonal)
            currentDots.Union(GetdiagonalPieces(dot2.column, dot2.row));
        if (dot3.isDiagonal)
            currentDots.Union(GetdiagonalPieces(dot3.column, dot3.row));

        return currentDots;
    }

    public void isColmnBomb(Dot dot)
    {
        currentMatches = currentMatches.Union(GetColumnPieces(dot.column)).ToList();
        board.SpecialDestroy();
        board.BombColumn(dot.column);
        //currentMatches.Union(GetColumnPieces(dot.column));
        //board.BombColumn(dot.column);
    }

    public void isRowBomb(Dot dot)
    {
        currentMatches = currentMatches.Union(GetRowPieces(dot.row)).ToList();
        board.SpecialDestroy();
        board.BombRow(dot.row);
    }

    private void AddToListAndMatch(GameObject dot)
    {
        if (!currentMatches.Contains(dot))         // Match된 Dot들을 리스트에 포함시킨다.
            currentMatches.Add(dot);
        dot.GetComponent<Dot>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }

    private IEnumerator FindAllMatchesCo() // 매칭 조건에 맞는다면 currentMatches에 리스트 추가.
    {
        //Debug.Log("FindAllMatchesCo");
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                if (currentDot != null)
                {
                    Dot currentDotDot = currentDot.GetComponent<Dot>();
                    if (i > 0 && i < board.width - 1)  // 왼쪽 오른쪽 매치 확인
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if (leftDot != null && rightDot != null)
                        {
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();
                            if (!leftDotDot.noMatchBlock() && !currentDotDot.noMatchBlock() && !rightDotDot.noMatchBlock())
                            {
                                if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag) // 현재 매치가 된 상태, 여기서 특수효과블록들을 찾는다.
                                {
                                    currentMatches.Union(isCrossBomb(leftDotDot, currentDotDot, rightDotDot));

                                    currentMatches.Union(isDiagonalBomb(leftDotDot, currentDotDot, rightDotDot));

                                    GetNearbyPieces(leftDot, currentDot, rightDot);
                                }
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1) // 위 아래 매치 확인 
                    {
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];
                        if (upDot != null && downDot != null)
                        {
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            Dot DownDotDot = downDot.GetComponent<Dot>();
                            if (!upDotDot.noMatchBlock() && !currentDotDot.noMatchBlock() && !DownDotDot.noMatchBlock())
                            {
                                if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag) // 현재 매치가 된 상태
                                {
                                    currentMatches.Union(isCrossBomb(upDotDot, currentDotDot, DownDotDot));

                                    currentMatches.Union(isDiagonalBomb(upDotDot, currentDotDot, DownDotDot));

                                    GetNearbyPieces(upDot, currentDot, downDot);
                                }
                            }

                        }
                    }
                }
            }
        }
    }
    #region 특수블록 매칭 함수
    List<GameObject> GetCrossPieces(int column, int row) // 십자가형 매칭
    {
        List<GameObject> dots = new List<GameObject>();

        board.allDots[column, row].GetComponent<Dot>().isMatched = true;

        if (0 < column)
        {
            if (board.allDots[column - 1, row] != null) //왼쪽
            {
                dots.Add(board.allDots[column - 1, row]);
                board.allDots[column - 1, row].GetComponent<Dot>().isMatched = true;
            }
        }

        if (board.width - 1 > column)
        {
            if (board.allDots[column + 1, row] != null) //오른쪽
            {
                dots.Add(board.allDots[column + 1, row]);
                board.allDots[column + 1, row].GetComponent<Dot>().isMatched = true;
            }
        }
        if (board.height - 1 > row)
        {
            if (board.allDots[column, row + 1] != null) //위쪽
            {
                dots.Add(board.allDots[column, row + 1]);
                board.allDots[column, row + 1].GetComponent<Dot>().isMatched = true;
            }
        }

        if (0 < row)
        {
            if (board.allDots[column, row - 1] != null) //아래쪽
            {
                dots.Add(board.allDots[column, row - 1]);
                board.allDots[column, row - 1].GetComponent<Dot>().isMatched = true;
            }
        }
        dots.Clear();
        return dots;
    }
    List<GameObject> GetdiagonalPieces(int column, int row)// X형 매칭
    {
        List<GameObject> dots = new List<GameObject>();

        board.allDots[column, row].GetComponent<Dot>().isMatched = true; // 기존에 4개의 매치가 된경우 매치형 특수효과를 위해 false시킨것을 다시 true로 수정

        if (0 < column && board.height - 1 > row)
        {
            if (board.allDots[column - 1, row + 1] != null) // 왼쪽 위 대각선
            {
                dots.Add(board.allDots[column - 1, row + 1]);
                board.allDots[column - 1, row + 1].GetComponent<Dot>().isMatched = true;
            }
        }

        if (board.width - 1 > column && board.height - 1 > row)
        {
            if (board.allDots[column + 1, row + 1] != null) //오른쪽 위 대각선
            {
                dots.Add(board.allDots[column + 1, row + 1]);
                board.allDots[column + 1, row + 1].GetComponent<Dot>().isMatched = true;
            }
        }
        if (0 < row && 0 < column)
        {
            if (board.allDots[column - 1, row - 1] != null) //왼쪽 아래 대각선
            {
                dots.Add(board.allDots[column - 1, row - 1]);
                board.allDots[column - 1, row - 1].GetComponent<Dot>().isMatched = true;
            }
        }

        if (0 < row && board.width - 1 > column)
        {
            if (board.allDots[column + 1, row - 1] != null) //오른쪽 아래 대각선
            {
                dots.Add(board.allDots[column + 1, row - 1]);
                board.allDots[column + 1, row - 1].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }

    List<GameObject> GetAcornPieces(int column, int row) // 도토리 폭파에 사용될 스킬
    {
        List<GameObject> dots = new List<GameObject>();

        dots.Add(board.allDots[column, row]);

        if (0 < column && board.height - 1 > row)
        {
            if (board.allDots[column - 1, row + 1] != null) // 왼쪽 위 대각선
            {
                dots.Add(board.allDots[column - 1, row + 1]);
                //board.allDots[column - 1, row + 1].GetComponent<Dot>().isMatched = true;
            }
        }

        if (board.width - 1 > column && board.height - 1 > row)
        {
            if (board.allDots[column + 1, row + 1] != null) //오른쪽 위 대각선
            {
                dots.Add(board.allDots[column + 1, row + 1]);
            }
        }
        if (0 < row && 0 < column)
        {
            if (board.allDots[column - 1, row - 1] != null) //왼쪽 아래 대각선
            {
                dots.Add(board.allDots[column - 1, row - 1]);
            }
        }

        if (0 < row && board.width - 1 > column)
        {
            if (board.allDots[column + 1, row - 1] != null) //오른쪽 아래 대각선
            {
                dots.Add(board.allDots[column + 1, row - 1]);
            }
        }

        if (0 < column)
        {
            if (board.allDots[column - 1, row] != null) //왼쪽
            {
                dots.Add(board.allDots[column - 1, row]);
            }
        }

        if (board.width - 1 > column)
        {
            if (board.allDots[column + 1, row] != null) //오른쪽
            {
                dots.Add(board.allDots[column + 1, row]);
            }
        }
        if (board.height - 1 > row)
        {
            if (board.allDots[column, row + 1] != null) //위쪽
            {
                dots.Add(board.allDots[column, row + 1]);
            }
        }

        if (0 < row)
        {
            if (board.allDots[column, row - 1] != null) //아래쪽
            {
                dots.Add(board.allDots[column, row - 1]);
            }
        }

        return dots;
    }

    List<GameObject> GetColumnPieces(int column) // 세로형 매칭
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.height; i++)
        {
            if (board.allDots[column, i] != null)
            {
                dots.Add(board.allDots[column, i]);
                //board.allDots[column, i].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }

    List<GameObject> GetRowPieces(int row) // 가로형 매칭
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allDots[i, row] != null)
            {
                dots.Add(board.allDots[i, row]);
                //board.allDots[i, row].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }
    #endregion
    public void CheckBombs() // currentDot은 움직인 개체
    {
        if (board.currentDot != null)
        {
            if (board.currentDot.isMatched) //움직인 개체가 매치상태이면?
            {
                board.currentDot.isMatched = false;

                int typeOfBomb = Random.Range(0, 100);
                if (typeOfBomb < 99)
                {
                    //board.currentDot.MakematchingBomb();
                    board.currentDot.SelectmatchingBomb();
                }
            }
            else if (board.currentDot.otherDot != null)
            {
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();

                if (otherDot.isMatched) // 움직여진 개체가 매치상태이면?
                {
                    otherDot.isMatched = false;

                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb < 99)
                    {
                        //board.currentDot.MakematchingBomb();
                        board.currentDot.SelectmatchingBomb();
                    }
                }
            }
        }
    }
    public void RandomCreateBombs() // 매치형, 선택형 블록 랜덤 생성
    {
        GameObject[,] currentdots = board.allDots;
        var i = 0;
        int Createpercent = Random.Range(0, 10); // 이거로 확률 계산 가능.

        while (i < 10)
        {
            int RandomXPick = Random.Range(0, currentdots.GetLength(0));
            int RandomYPick = Random.Range(0, currentdots.GetLength(1));
            if (currentdots[RandomXPick, RandomYPick] != null) // 1. 해당 블록의 존재 유무 판단.
            {
                if (currentdots[RandomXPick, RandomYPick].GetComponent<Dot>().SpecialBlockCheck()) // 선택된 블록이 특수블록을 갖고있는지 확인
                {
                    int typeOfBomb = Random.Range(0, 2);  // 랜덤 생성되는 특수, 선택 블록의 확률 계산
                    if (typeOfBomb == 1)
                        currentdots[RandomXPick, RandomYPick].GetComponent<Dot>().MakematchingBomb();
                    else
                        currentdots[RandomXPick, RandomYPick].GetComponent<Dot>().SelectmatchingBomb();

                    Debug.LogError("생성");
                    return;
                }
            }
            i++;
        }
    }

    public void RandomCreateHinder(int num) // num -> 0: 참새 1: 도토리나무 2: 엮인줄기나무 3: 랜덤
    {
        GameObject[,] currentdots = board.allDots;
        var i = 0;
        var Acornnum = 0;
        while (i < 15)
        {
            int RandomXPick = Random.Range(0, currentdots.GetLength(0));
            int RandomYPick = Random.Range(0, currentdots.GetLength(1));
            if (currentdots[RandomXPick, RandomYPick] != null) // 1. 해당 블록의 존재 유무 판단.
            {
                if (currentdots[RandomXPick, RandomYPick].GetComponent<Dot>().SpecialBlockCheck()) // 선택된 블록이 특수블록을 갖고있는지 확인
                {
                    currentdots[RandomXPick, RandomYPick].GetComponent<Dot>().CreateHinderBlock(num, true);

                    if (num != 4) // 도토리 블록이 아닌 경우
                        return;
                    else
                        Acornnum++;
                }
            }
            if (Acornnum == 4)
                return;
            i++;
        }
    }

    public void Bird_AcornTree_Check() // 참새 블록 체크 후 생성
    {
        var BirdCount = 0;
        var AcornTree = 0;
        board.BirdLimitingMove++;
        board.AcornTreeLimitingMove++;
        board.StalkTreeLimitingMove++;

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allDots[i, j] != null && board.allDots[i, j].GetComponent<Dot>().isBird)
                {
                    BirdCount++;
                }
                if (board.allDots[i, j] != null && board.allDots[i, j].GetComponent<Dot>().isAcornTree)
                {
                    AcornTree++;
                }
                if (board.allDots[i, j] != null && board.allDots[i, j].GetComponent<Dot>().isStalkTree && board.allDots[i, j].GetComponent<Dot>().isSpreader)
                {
                    Create_StalkTree(i, j);
                }
            }
        }
        if (BirdCount > 0)
        {
            if (board.BirdLimitingMove == 3)
            {
                RandomCreateHinder(0);
                board.BirdLimitingMove = 0;
            }
        }
        else
            board.BirdLimitingMove = 0;

        if (AcornTree > 0)
        {
            if (board.AcornTreeLimitingMove == 3)
            {
                RandomCreateHinder(4);
                board.AcornTreeLimitingMove = 0;
            }
        }
        else
            board.AcornTreeLimitingMove = 0;


        if (board.StalkTreeLimitingMove == 1)
        {
            board.StalkTreeLimitingMove = 0;
        }

    }

    public bool AcornTree_Exist() // 참나무 체크 후 생성
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allDots[i, j] != null && board.allDots[i, j].GetComponent<Dot>().isAcornTree)
                {
                    //board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                    return true;
                }
            }
        }
        return false;
    }

    public void Create_StalkTree(int column, int row) // 엮인 줄기 생성
    {
        var i = 0;
        while (i < 5)
        {
            int random = Random.Range(0, 4);

            if (random == 0)
            {
                if (0 < column)
                {
                    if (board.allDots[column - 1, row] != null) //왼쪽
                    {
                        if (!board.allDots[column - 1, row].GetComponent<Dot>().isHinderBlock()) // 해당위치가 방해블록이 아니라면?
                        {
                            board.allDots[column, row].GetComponent<Dot>().isSpreader = false;
                            board.allDots[column - 1, row].GetComponent<Dot>().CreateHinderBlock(2, false);
                            break;
                        }
                    }
                }
            }
            else if (random == 1)
            {
                if (board.width - 1 > column)
                {
                    if (board.allDots[column + 1, row] != null) //오른쪽
                    {
                        if (!board.allDots[column + 1, row].GetComponent<Dot>().isHinderBlock()) // 해당위치가 방해블록이 아니라면?
                        {
                            board.allDots[column, row].GetComponent<Dot>().isSpreader = false;
                            board.allDots[column + 1, row].GetComponent<Dot>().CreateHinderBlock(2, false);
                            break;
                        }
                    }
                }
            }
            else if (random == 2)
            {
                if (board.height - 1 > row)
                {
                    if (board.allDots[column, row + 1] != null) //위쪽
                    {
                        if (!board.allDots[column, row + 1].GetComponent<Dot>().isHinderBlock()) // 해당위치가 방해블록이 아니라면?
                        {
                            board.allDots[column, row].GetComponent<Dot>().isSpreader = false;
                            board.allDots[column, row + 1].GetComponent<Dot>().CreateHinderBlock(2, false);
                            break;
                        }
                    }
                }
            }
            else if (random == 3)
            {
                if (0 < row)
                {
                    if (board.allDots[column, row - 1] != null) //아래쪽
                    {
                        if (!board.allDots[column, row - 1].GetComponent<Dot>().isHinderBlock()) // 해당위치가 방해블록이 아니라면?
                        {
                            board.allDots[column, row].GetComponent<Dot>().isSpreader = false;
                            board.allDots[column, row - 1].GetComponent<Dot>().CreateHinderBlock(2, false);
                            break;
                        }
                    }
                }
            }
            i++;
        }
    }

    public void Axe_Skill(int column, int row) // 참나무 도끼 발동
    {
        currentMatches.Add(board.allDots[column, row]);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allDots[i, j] != null && board.allDots[i, j].GetComponent<Dot>().isAcornTree)
                {
                    RandomCreateHinder(4);
                }
            }
        }
        board.SpecialDestroy();
    }

    public void SlingShot_Skill(int colunm,int row) // 새총 발동
    {
        List<GameObject> dots = new List<GameObject>();
        var num = 0;
        var ii = 0;
        board.currentState = GameState.wait;
        dots.Add(board.allDots[colunm, row]);

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allDots[i, j] != null && board.allDots[i, j].GetComponent<Dot>().isBird)
                {
                    num++;
                    dots.Add(board.allDots[i, j]);
                }
            }
        }

        while (ii < 15)
        {
            int RandomXPick = Random.Range(0, board.allDots.GetLength(0));
            int RandomYPick = Random.Range(0, board.allDots.GetLength(1));
            if (board.allDots[RandomXPick, RandomYPick] != null) // 1. 해당 블록의 존재 유무 판단.
            {
                if (board.allDots[RandomXPick, RandomYPick].GetComponent<Dot>().SpecialBlockCheck()) // 선택된 블록이 특수블록을 갖고있는지 확인
                {
                    num++;
                    dots.Add(board.allDots[RandomXPick, RandomYPick]);
                }
            }

            if (num == 7)
                break;
            ii++;
        }
        StartCoroutine(SlingShot_Skill_Co(dots));


    }
    IEnumerator SlingShot_Skill_Co(List<GameObject> list)
    {
        for (int i = 1; i < list.Count; i++)
        {
            list[i].GetComponent<Dot>().SlingShot_Target();
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(3.0f);
        currentMatches = currentMatches.Union(list).ToList();
        board.currentState = GameState.move;
        board.SpecialDestroy();
    }

    public void AcornBoom_Skill(Dot dot) //도토리 폭파 발동
    {
        List<Dot> dots = new List<Dot>();
        var AcornExist = false;
        board.currentState = GameState.wait;

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allDots[i, j] != null && board.allDots[i, j].GetComponent<Dot>().isAcorn)
                {
                    dots.Add(board.allDots[i, j].GetComponent<Dot>());
                    AcornExist = true;
                }
            }
        }
        if (AcornExist == true)
            StartCoroutine(AcornBoom_Skill_Co(dots));
        else
        {
            currentMatches = currentMatches.Union(GetAcornPieces(dot.column, dot.row)).ToList();
            board.SpecialDestroy();
            board.currentState = GameState.move;
        }
    }
    IEnumerator AcornBoom_Skill_Co(List<Dot> dots)
    {
        for (int i = 0; i < dots.Count; i++)
        {
            currentMatches = currentMatches.Union(GetAcornPieces(dots[i].column, dots[i].row)).ToList();
            board.SpecialDestroy();
            yield return new WaitForSeconds(0.2f);
        }
        board.currentState = GameState.move;
        yield return null;
    }

    public void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
