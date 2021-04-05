﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MysticCrystal_Boom : MysticCrystal_Abstract
{
    private GoalManager goalManager;

    [SerializeField]
    private GameObject BoomParticle;

    public override void Level_1()
    {
        Boom_Destroy(GetBoomDots_Lv1((int)transform.position.x, (int)transform.position.y));
    }

    public override void Level_2()
    {
    }

    public override void Level_3()
    {
    }

    private void Boom_Destroy(Queue<State> blocks)
    {
        //goalManager = FindObjectOfType<GoalManager>();

        List<int> columnList = new List<int>();

        while (blocks.Count > 0)
        {
            State dot = blocks.Dequeue();

            if (dot != null && dot.dotState == DotState.Possible)
            {
                Instantiate(BoomParticle, dot.transform.position, Quaternion.identity);

                if (dot.GetComponent<Dot>() != null)
                {
                    columnList.Add(dot.GetComponent<Dot>().column);
                    ObjectPool.ReturnObject(dot.gameObject);
                }
                else if (dot.GetComponent<Obstruction_Abstract>() != null)
                    dot.GetComponent<Obstruction_Abstract>().GetDamage(Damage);
                else if (dot.GetComponent<Mystic_Abstract>() != null)
                    dot.GetComponent<Mystic_Abstract>().Destroy_Mystic();


                //if (goalManager != null)
                //{
                //    goalManager.Update_CurrentGage(1);
                //    goalManager.Update_CurrentScore((int)scoreManager.GetScore(0.4f));
                //}
            }
        }

        columnList = columnList.Distinct().ToList();

        foreach (int i in columnList)
        {
            if (Board.Instance.DecreaseRowArray[i] != null)
            {
                Board.Instance.StopCoroutine(Board.Instance.DecreaseRowArray[i]);
                Debug.Log("코루틴 정지");
            }

            Board.Instance.DecreaseRowArray[i] = Board.Instance.StartCoroutine(Board.Instance.DecreaseRowCo(i)); // 행 내리기
        }
    }


    Queue<State> GetBoomDots_Lv1(int column, int row)
    {
        Queue<State> dots = new Queue<State>();

        //GetDot(column, row, dots);

        if (0 < column) { GetDot(column - 1, row, dots); }
        if (Board.Instance.width - 1 > column) { GetDot(column + 1, row, dots); }
        if (Board.Instance.height - 1 > row) { GetDot(column, row + 1, dots); }
        if (0 < row) { GetDot(column, row - 1, dots); }
        if (0 < column && Board.Instance.height - 1 > row) { GetDot(column - 1, row + 1, dots); }
        if (Board.Instance.width - 1 > column && Board.Instance.height - 1 > row) {GetDot(column + 1, row + 1, dots); }
        if (0 < row && 0 < column) { GetDot(column - 1, row - 1, dots); }
        if (0 < row && Board.Instance.width - 1 > column) { GetDot(column + 1, row - 1, dots); }

        return dots;
    }

    void GetDot(int column, int row, Queue<State> dots)
    {
        if (Board.Instance.allDots[column, row] != null)
        {
            dots.Enqueue(Board.Instance.allDots[column, row]);
        }
        else if (Board.Instance.MysticDots[column, row] != null)
        {
            dots.Enqueue(Board.Instance.MysticDots[column, row]);
        }
        else if (Board.Instance.ObstructionDots[column, row] != null)
        {
            dots.Enqueue(Board.Instance.ObstructionDots[column, row]);
        }

    }
}
