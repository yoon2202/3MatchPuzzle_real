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
        Boom_Destroy(GetBoomDots_Lv1((int)transform.position.x, (int)transform.position.y));
    }

    public override void Level_2()
    {
    }

    public override void Level_3()
    {
    }

    private void Boom_Destroy(List<State> blocks)
    {
        goalManager = FindObjectOfType<GoalManager>();

        List<int> columnList = new List<int>();

        for (int i = 0; i < blocks.Count; i++)
        {
            State dot = blocks[i];

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
                else if(dot.GetComponent<Mystic_Abstract>() != null)
                    dot.GetComponent<Mystic_Abstract>().Destroy_Mystic();


                if (goalManager != null)
                {
                    goalManager.Update_CurrentGage(1);
                    goalManager.Update_CurrentScore((int)scoreManager.GetScore(0.4f));
                }
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


    List<State> GetBoomDots_Lv1(int column, int row)
    {
        List<State> dots = new List<State>();

        if (Board.Instance.allDots[column, row] != null) // 중앙
        {
            dots.Add(Board.Instance.allDots[column, row]);
        }
        else if (Board.Instance.MysticDots[column, row] != null)
        {
            dots.Add(Board.Instance.MysticDots[column, row]);
        }
        else if (Board.Instance.ObstructionDots[column, row] != null)
        {
            dots.Add(Board.Instance.ObstructionDots[column, row]);
        }

        if (0 < column)
        {
            if (Board.Instance.allDots[column - 1, row] != null) //왼쪽
            {
                dots.Add(Board.Instance.allDots[column - 1, row]);
            }
            else if (Board.Instance.MysticDots[column - 1, row] != null)
            {
                dots.Add(Board.Instance.MysticDots[column - 1, row]);
            }
            else if (Board.Instance.ObstructionDots[column - 1, row] != null)
            {
                dots.Add(Board.Instance.ObstructionDots[column - 1, row]);
            }
        }

        if (Board.Instance.width - 1 > column)
        {
            if (Board.Instance.allDots[column + 1, row] != null) //오른쪽
            {
                dots.Add(Board.Instance.allDots[column + 1, row]);
            }
            else if (Board.Instance.MysticDots[column + 1, row] != null)
            {
                dots.Add(Board.Instance.MysticDots[column + 1, row]);
            }
            else if (Board.Instance.ObstructionDots[column + 1, row] != null)
            {
                dots.Add(Board.Instance.ObstructionDots[column + 1, row]);
            }
        }
        if (Board.Instance.height - 1 > row)
        {
            if (row + 1 < Board.Instance.width && Board.Instance.allDots[column, row + 1] != null) //위쪽
            {
                dots.Add(Board.Instance.allDots[column, row + 1]);
            }
            else if (Board.Instance.MysticDots[column, row + 1] != null)
            {
                dots.Add(Board.Instance.MysticDots[column, row + 1]);
            }
            else if (Board.Instance.ObstructionDots[column, row + 1] != null)
            {
                dots.Add(Board.Instance.ObstructionDots[column, row + 1]);
            }
        }

        if (0 < row)
        {
            if (Board.Instance.allDots[column, row - 1] != null) //아래쪽
            {
                dots.Add(Board.Instance.allDots[column, row - 1]);
            }
            else if (Board.Instance.MysticDots[column, row - 1] != null)
            {
                dots.Add(Board.Instance.MysticDots[column, row - 1]);
            }
            else if (Board.Instance.ObstructionDots[column, row - 1] != null)
            {
                dots.Add(Board.Instance.ObstructionDots[column, row - 1]);
            }
        }

        if (0 < column && Board.Instance.height - 1 > row)
        {
            if (Board.Instance.allDots[column - 1, row + 1] != null) // 왼쪽 위 대각선
            {
                dots.Add(Board.Instance.allDots[column - 1, row + 1]);
            }
            else if (Board.Instance.MysticDots[column - 1, row + 1] != null)
            {
                dots.Add(Board.Instance.MysticDots[column - 1, row + 1]);
            }
            else if (Board.Instance.ObstructionDots[column - 1, row + 1] != null)
            {
                dots.Add(Board.Instance.ObstructionDots[column - 1, row + 1]);
            }
        }

        if (Board.Instance.width - 1 > column && Board.Instance.height - 1 > row)
        {
            if (Board.Instance.allDots[column + 1, row + 1] != null) //오른쪽 위 대각선
            {
                dots.Add(Board.Instance.allDots[column + 1, row + 1]);
            }
            else if (Board.Instance.MysticDots[column + 1, row + 1] != null)
            {
                dots.Add(Board.Instance.MysticDots[column + 1, row + 1]);
            }
            else if (Board.Instance.ObstructionDots[column + 1, row + 1] != null)
            {
                dots.Add(Board.Instance.ObstructionDots[column + 1, row + 1]);
            }
        }
        if (0 < row && 0 < column)
        {
            if (Board.Instance.allDots[column - 1, row - 1] != null) //왼쪽 아래 대각선
            {
                dots.Add(Board.Instance.allDots[column - 1, row - 1]);
            }
            else if (Board.Instance.MysticDots[column -1, row - 1] != null)
            {
                dots.Add(Board.Instance.MysticDots[column - 1, row - 1]);
            }
            else if (Board.Instance.ObstructionDots[column - 1, row - 1] != null)
            {
                dots.Add(Board.Instance.ObstructionDots[column - 1, row - 1]);
            }
        }

        if (0 < row && Board.Instance.width - 1 > column)
        {
            if (Board.Instance.allDots[column + 1, row - 1] != null) //오른쪽 아래 대각선
            {
                dots.Add(Board.Instance.allDots[column + 1, row - 1]);
            }
            else if (Board.Instance.MysticDots[column + 1, row - 1] != null)
            {
                dots.Add(Board.Instance.MysticDots[column + 1, row - 1]);
            }
            else if (Board.Instance.ObstructionDots[column + 1, row - 1] != null)
            {
                dots.Add(Board.Instance.ObstructionDots[column + 1, row - 1]);
            }
        }

        return dots;
    }

}
