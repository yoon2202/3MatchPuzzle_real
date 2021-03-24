﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public ActiveList activeList;
    public List<Dot> currentMatches = new List<Dot>();
    public static Queue<Transform> MovingDot = new Queue<Transform>();
    void Start()
    {

        board = FindObjectOfType<Board>();
    }

    public void FIndAllMathces()
    {
        StartCoroutine(FindAllMatchesCo());
    }


    //private List<Dot> isCrossBomb(Dot dot1, Dot dot2, Dot dot3) 
    //{
    //    List<Dot> currentDots = new List<Dot>();

    //    if (dot1.specialBlock == SpecialBlock.Cross)
    //        currentDots = currentDots.Union(GetCrossPieces(dot1.column, dot1.row)).ToList();
    //    if (dot2.specialBlock == SpecialBlock.Cross)
    //        currentDots = currentDots.Union(GetCrossPieces(dot2.column, dot2.row)).ToList();
    //    if (dot3.specialBlock == SpecialBlock.Cross)
    //        currentDots = currentDots.Union(GetCrossPieces(dot3.column, dot3.row)).ToList();
    //    return currentDots;
    //}

    //private List<Dot> isDiagonalBomb(Dot dot1, Dot dot2, Dot dot3)
    //{
    //    List<Dot> currentDots = new List<Dot>();

    //    if (dot1.specialBlock == SpecialBlock.Multiple)
    //        currentDots = currentDots.Union(GetdiagonalPieces(dot1.column, dot1.row)).ToList();
    //    if (dot2.specialBlock == SpecialBlock.Multiple)
    //        currentDots = currentDots.Union(GetdiagonalPieces(dot2.column, dot2.row)).ToList();
    //    if (dot3.specialBlock == SpecialBlock.Multiple)
    //        currentDots = currentDots.Union(GetdiagonalPieces(dot3.column, dot3.row)).ToList();

    //    return currentDots;
    //}


    private void GetNearbyPieces(Dot dot1, Dot dot2, Dot dot3)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
    }

    private void GetNearbyPieces(Dot dot1, Dot dot2, Dot dot3, Dot dot4)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
        AddToListAndMatch(dot4);
    }

    private void AddToListAndMatch(Dot dot)
    {
        if (!currentMatches.Contains(dot)) // Match된 Dot들을 리스트에 포함시킨다.
            currentMatches.Add(dot);
    }

    public IEnumerator FindAllMatchesCo() // 매칭 조건에 맞는다면 currentMatches에 리스트 추가.
    {
        board.b_matching = true;

        while (true)
        {
            if (MovingDot.Count > 0)
            {
                yield return null;
            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                Dot currentDot = board.allDots[i, j];
                if (currentDot != null)
                {
                    if (i > 0 && i < board.width - 1)  // 왼쪽 오른쪽 매치 확인
                    {
                        Dot leftDot = board.allDots[i - 1, j];
                        Dot rightDot = board.allDots[i + 1, j];

                        if (leftDot != null && rightDot != null && leftDot.b_IsTargeted == false && rightDot.b_IsTargeted == false)
                        {
                                if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag) // 현재 매치가 된 상태, 여기서 특수효과블록들을 찾는다.
                                {
                                    GetNearbyPieces(leftDot, currentDot, rightDot);
                                }
                        }
                    }

                    if (j > 0 && j < board.height - 1) // 위 아래 매치 확인 
                    {
                        Dot upDot = board.allDots[i, j + 1];
                        Dot downDot = board.allDots[i, j - 1];

                        if (upDot != null && downDot != null && upDot.b_IsTargeted == false && downDot.b_IsTargeted == false)
                        {
                                if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag) // 현재 매치가 된 상태
                                {
                                    GetNearbyPieces(upDot, currentDot, downDot);
                                }
                        }
                    }
                }
            }
        }

        //yield return new WaitForSeconds(0.03f);

        //if(currentMatches.Count > 0)
        //{
        //    board.DestroyMatches(false, false);
        //}
        //else
        //{
        //    board.currentState = GameState.move;
        //    board.currentDot = null;
        //}
    }


    #region 특수블록 스킬
    List<Dot> GetCrossDots(int column, int row) // 십자가형 매칭
    {
        List<Dot> dots = new List<Dot>();
        dots.Add(board.allDots[column, row]);
        var Maxindex = activeList.activeList[0].CurrentLevel;
        for (int CurrentIndex = 1; CurrentIndex <= Maxindex; CurrentIndex++)
        {
            if (0 < column)
            {
                if (column - CurrentIndex >= 0 && board.allDots[column - CurrentIndex, row] != null) //왼쪽
                {
                    dots.Add(board.allDots[column - CurrentIndex, row]);
                }
            }

            if (board.width - 1 > column)
            {
                if (column + CurrentIndex < board.width && board.allDots[column + CurrentIndex, row] != null) //오른쪽
                {
                    dots.Add(board.allDots[column + CurrentIndex, row]);
                }
            }
            if (board.height - 1 > row)
            {
                if (row + CurrentIndex < board.width && board.allDots[column, row + CurrentIndex] != null) //위쪽
                {
                    dots.Add(board.allDots[column, row + CurrentIndex]);
                }
            }

            if (0 < row)
            {
                if (row - CurrentIndex >= 0 && board.allDots[column, row - CurrentIndex] != null) //아래쪽
                {
                    dots.Add(board.allDots[column, row - CurrentIndex]);
                }
            }
        }

        return dots;
    }
    List<Dot> GetMultipleDots(int column, int row)// X형 매칭
    {
        List<Dot> dots = new List<Dot>();
        dots.Add(board.allDots[column, row]);
        var Maxindex = activeList.activeList[1].CurrentLevel;
        for (int CurrentIndex = 1; CurrentIndex <= Maxindex; CurrentIndex++)
        {
            if (0 < column && board.height - 1 > row)
            {
                if (column - CurrentIndex >= 0 && row + CurrentIndex <= board.height && board.allDots[column - CurrentIndex, row + CurrentIndex] != null) // 왼쪽 위 대각선
                {
                    dots.Add(board.allDots[column - CurrentIndex, row + CurrentIndex]);
                }
            }

            if (board.width - 1 > column && board.height - 1 > row)
            {
                if (column + CurrentIndex <= board.width && row + CurrentIndex <= board.height && board.allDots[column + CurrentIndex, row + CurrentIndex] != null) //오른쪽 위 대각선
                {
                    dots.Add(board.allDots[column + CurrentIndex, row + CurrentIndex]);
                }
            }
            if (0 < row && 0 < column)
            {
                if (column - CurrentIndex >= 0 && row - CurrentIndex >= 0 && board.allDots[column - CurrentIndex, row - CurrentIndex] != null) //왼쪽 아래 대각선
                {
                    dots.Add(board.allDots[column - CurrentIndex, row - CurrentIndex]);
                }
            }

            if (0 < row && board.width - 1 > column)
            {
                if (column + CurrentIndex <= board.width && row + CurrentIndex >= 0 && board.allDots[column + CurrentIndex, row - CurrentIndex] != null) //오른쪽 아래 대각선
                {
                    dots.Add(board.allDots[column + CurrentIndex, row - CurrentIndex]);
                }
            }
        }
        return dots;
    }

    List<Dot> GetAcornPieces(int column, int row) // 도토리 폭파에 사용될 스킬
    {
        List<Dot> dots = new List<Dot>();

        dots.Add(board.allDots[column, row]);

        if (0 < column && board.height - 1 > row)
        {
            if (board.allDots[column - 1, row + 1] != null) // 왼쪽 위 대각선
            {
                dots.Add(board.allDots[column - 1, row + 1]);
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

    List<Dot> GetColumnPieces(int column) // 세로형 매칭
    {
        List<Dot> dots = new List<Dot>();
        for (int i = 0; i < board.height; i++)
        {
            if (board.allDots[column, i] != null)
            {
                dots.Add(board.allDots[column, i]);
            }
        }
        return dots;
    }

    List<Dot> GetRowPieces(int row) // 가로형 매칭
    {
        List<Dot> dots = new List<Dot>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allDots[i, row] != null)
            {
                dots.Add(board.allDots[i, row]);
            }
        }
        return dots;
    }
    #endregion
    #region 특수스킬
    //public void Bird_AcornTree_Check() // 참새 블록 체크 후 생성
    //{
    //    var BirdCount = 0;
    //    var AcornTree = 0;
    //    board.BirdLimitingMove++;
    //    board.AcornTreeLimitingMove++;
    //    board.StalkTreeLimitingMove++;

    //    for (int i = 0; i < board.width; i++)
    //    {
    //        for (int j = 0; j < board.height; j++)
    //        {
    //            if (board.allDots[i, j] != null && board.allDots[i, j].isBird)
    //            {
    //                BirdCount++;
    //            }
    //            if (board.allDots[i, j] != null && board.allDots[i, j].isAcornTree)
    //            {
    //                AcornTree++;
    //            }
    //            if (board.allDots[i, j] != null && board.allDots[i, j].isStalkTree && board.allDots[i, j].isSpreader)
    //            {
    //                Create_StalkTree(i, j);
    //            }
    //        }
    //    }
    //    if (BirdCount > 0)
    //    {
    //        if (board.BirdLimitingMove == 3)
    //        {
    //            RandomCreateHinder(0);
    //            board.BirdLimitingMove = 0;
    //        }
    //    }
    //    else
    //        board.BirdLimitingMove = 0;

    //    if (AcornTree > 0)
    //    {
    //        if (board.AcornTreeLimitingMove == 3)
    //        {
    //            RandomCreateHinder(4);
    //            board.AcornTreeLimitingMove = 0;
    //        }
    //    }
    //    else
    //        board.AcornTreeLimitingMove = 0;


    //    if (board.StalkTreeLimitingMove == 1)
    //    {
    //        board.StalkTreeLimitingMove = 0;
    //    }

    //}

    //public bool AcornTree_Exist() // 참나무 체크 후 생성
    //{
    //    for (int i = 0; i < board.width; i++)
    //    {
    //        for (int j = 0; j < board.height; j++)
    //        {
    //            if (board.allDots[i, j] != null && board.allDots[i, j].isAcornTree)
    //            {
    //                //board.allDots[i, j].GetComponent<Dot>().isMatched = true;
    //                return true;
    //            }
    //        }
    //    }
    //    return false;
    //}

    //public void Create_StalkTree(int column, int row) // 엮인 줄기 생성
    //{
    //    var i = 0;
    //    while (i < 5)
    //    {
    //        int random = Random.Range(0, 4);

    //        if (random == 0)
    //        {
    //            if (0 < column)
    //            {
    //                if (board.allDots[column - 1, row] != null) //왼쪽
    //                {
    //                    if (!board.allDots[column - 1, row].isHinderBlock()) // 해당위치가 방해블록이 아니라면?
    //                    {
    //                        board.allDots[column, row].isSpreader = false;
    //                        //board.allDots[column - 1, row].CreateHinderBlock(2, false);
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        else if (random == 1)
    //        {
    //            if (board.width - 1 > column)
    //            {
    //                if (board.allDots[column + 1, row] != null) //오른쪽
    //                {
    //                    if (!board.allDots[column + 1, row].isHinderBlock()) // 해당위치가 방해블록이 아니라면?
    //                    {
    //                        board.allDots[column, row].isSpreader = false;
    //                        //board.allDots[column + 1, row].CreateHinderBlock(2, false);
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        else if (random == 2)
    //        {
    //            if (board.height - 1 > row)
    //            {
    //                if (board.allDots[column, row + 1] != null) //위쪽
    //                {
    //                    if (!board.allDots[column, row + 1].isHinderBlock()) // 해당위치가 방해블록이 아니라면?
    //                    {
    //                        board.allDots[column, row].isSpreader = false;
    //                        //board.allDots[column, row + 1].CreateHinderBlock(2, false);
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        else if (random == 3)
    //        {
    //            if (0 < row)
    //            {
    //                if (board.allDots[column, row - 1] != null) //아래쪽
    //                {
    //                    if (!board.allDots[column, row - 1].isHinderBlock()) // 해당위치가 방해블록이 아니라면?
    //                    {
    //                        board.allDots[column, row].isSpreader = false;
    //                        //board.allDots[column, row - 1].CreateHinderBlock(2, false);
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //        i++;
    //    }
    //}

    //public void Axe_Skill(int column, int row) // 참나무 도끼 발동
    //{
    //    currentMatches.Add(board.allDots[column, row]);
    //    for (int i = 0; i < board.width; i++)
    //    {
    //        for (int j = 0; j < board.height; j++)
    //        {
    //            if (board.allDots[i, j] != null && board.allDots[i, j].isAcornTree)
    //            {
    //                RandomCreateHinder(4);
    //            }
    //        }
    //    }
    //    board.SpecialDestroy();
    //}

    //public void SlingShot_Skill(int colunm, int row) // 새총 발동
    //{
    //    List<Dot> dots = new List<Dot>();
    //    var num = 0;
    //    var ii = 0;
    //    board.currentState = GameState.wait;
    //    dots.Add(board.allDots[colunm, row]);

    //    for (int i = 0; i < board.width; i++)
    //    {
    //        for (int j = 0; j < board.height; j++)
    //        {
    //            if (board.allDots[i, j] != null && board.allDots[i, j].isBird)
    //            {
    //                num++;
    //                dots.Add(board.allDots[i, j]);
    //            }
    //        }
    //    }

    //    while (ii < 15)
    //    {
    //        int RandomXPick = Random.Range(0, board.allDots.GetLength(0));
    //        int RandomYPick = Random.Range(0, board.allDots.GetLength(1));
    //        if (board.allDots[RandomXPick, RandomYPick] != null) // 1. 해당 블록의 존재 유무 판단.
    //        {
    //            if (board.allDots[RandomXPick, RandomYPick].SpecialBlockCheck()) // 선택된 블록이 특수블록을 갖고있는지 확인
    //            {
    //                num++;
    //                dots.Add(board.allDots[RandomXPick, RandomYPick]);
    //            }
    //        }

    //        if (num == 7)
    //            break;
    //        ii++;
    //    }
    //    StartCoroutine(SlingShot_Skill_Co(dots));


    //}
    //IEnumerator SlingShot_Skill_Co(List<Dot> list)
    //{
    //    for (int i = 1; i < list.Count; i++)
    //    {
    //        list[i].SlingShot_Target();
    //        yield return new WaitForSeconds(0.2f);
    //    }
    //    yield return new WaitForSeconds(3.0f);
    //    currentMatches = currentMatches.Union(list).ToList();
    //    board.currentState = GameState.move;
    //    board.SpecialDestroy();
    //}

    #endregion

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
