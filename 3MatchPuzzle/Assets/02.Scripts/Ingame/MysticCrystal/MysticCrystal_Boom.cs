using System.Collections;
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
        StartCoroutine(Boom_Destroy());
    }

    public override void Level_2()
    {
    }

    public override void Level_3()
    {
    }

    IEnumerator Boom_Destroy()
    {
        b_Effectprogress = true;
        yield return new WaitForEndOfFrame();

        goalManager = FindObjectOfType<GoalManager>();

        Queue<State> blocks = new Queue<State>();

        var column = (int)transform.position.x;
        var row = (int)transform.position.y;

        if (0 < column)
        { blocks.Enqueue(GetDot(column - 1, row)); }
        yield return null;

        if (Board.Instance.width - 1 > column)
        { blocks.Enqueue(GetDot(column + 1, row)); }
        yield return null;

        if (Board.Instance.height - 1 > row)
        { blocks.Enqueue(GetDot(column, row + 1)); }
        yield return null;

        if (0 < row)
        { blocks.Enqueue(GetDot(column, row - 1)); }
        yield return null;

        if (0 < column && Board.Instance.height - 1 > row)
        { blocks.Enqueue(GetDot(column - 1, row + 1)); }
        yield return null;

        if (Board.Instance.width - 1 > column && Board.Instance.height - 1 > row)
        { blocks.Enqueue(GetDot(column + 1, row + 1)); }
        yield return null;

        if (0 < row && 0 < column)
        { blocks.Enqueue(GetDot(column - 1, row - 1)); }
        yield return null;

        if (0 < row && Board.Instance.width - 1 > column)
        { blocks.Enqueue(GetDot(column + 1, row - 1)); }
        yield return null;


        List<int> columnList = new List<int>();

        while (blocks.Count > 0)
        {
            State dot = blocks.Dequeue();

            if (dot != null)
            {
                dot.dotState = DotState.Targeted;
                Instantiate(BoomParticle, dot.transform.position, Quaternion.identity);

                if (dot.GetComponent<Dot>() != null && dot.gameObject.activeSelf)
                {
                    columnList.Add(dot.GetComponent<Dot>().column);
                    ObjectPool.ReturnObject(dot.gameObject);
                }
                else if (dot.GetComponent<Obstruction_Abstract>() != null)
                    dot.GetComponent<Obstruction_Abstract>().GetDamage(Damage);
                else if (dot.GetComponent<Mystic_Abstract>() != null)
                    dot.GetComponent<Mystic_Abstract>().Destroy_Mystic();


                if (goalManager != null)
                {
                    goalManager.Update_CurrentGage(1);
                    goalManager.Update_CurrentScore((int)scoreManager.GetScore(0.4f));
                }
            }

            yield return null;
        }

        columnList = columnList.Distinct().ToList();

        for (int i = 0; i < columnList.Count; i++)
        {
            if (Board.Instance.DecreaseRowArray[columnList[i]] != null)
            {
                Board.Instance.StopCoroutine(Board.Instance.DecreaseRowArray[columnList[i]]);
                Debug.Log("코루틴 정지");
            }

            Board.Instance.DecreaseRowArray[columnList[i]] = Board.Instance.StartCoroutine(Board.Instance.DecreaseRowCo(columnList[i])); // 행 내리기
            yield return null;
        }

        b_Effectprogress = false;
    }


    //Queue<State> GetBoomDots_Lv1(int column, int row)
    //{
    //    Queue<State> dots = new Queue<State>();

    //    if (0 < column) 
    //    { GetDot(column - 1, row, dots); }
    //    if (Board.Instance.width - 1 > column) 
    //    { GetDot(column + 1, row, dots); }
    //    if (Board.Instance.height - 1 > row) 
    //    { GetDot(column, row + 1, dots); }
    //    if (0 < row) 
    //    { GetDot(column, row - 1, dots); }
    //    if (0 < column && Board.Instance.height - 1 > row) 
    //    { GetDot(column - 1, row + 1, dots); }
    //    if (Board.Instance.width - 1 > column && Board.Instance.height - 1 > row)
    //    {GetDot(column + 1, row + 1, dots); }
    //    if (0 < row && 0 < column) 
    //    { GetDot(column - 1, row - 1, dots); }
    //    if (0 < row && Board.Instance.width - 1 > column) 
    //    { GetDot(column + 1, row - 1, dots); }

    //    return dots;
    //}

    State GetDot(int column, int row)
    {
        if (Board.Instance.allDots[column, row] != null && Board.Instance.allDots[column, row].dotState == DotState.Possible)
        {
           return Board.Instance.allDots[column, row];
        }
        else if (Board.Instance.MysticDots[column, row] != null && Board.Instance.MysticDots[column, row].dotState == DotState.Possible)
        {
            return Board.Instance.MysticDots[column, row];
        }
        else if (Board.Instance.ObstructionDots[column, row] != null && Board.Instance.ObstructionDots[column, row].dotState == DotState.Possible)
        {
            return Board.Instance.ObstructionDots[column, row];
        }

        return null;

    }
}
