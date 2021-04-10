using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticCrystal_Penetrate : MysticCrystal_Abstract
{
    private GoalManager goalManager;

    [SerializeField]
    private GameObject PenetrateParticle;

    [SerializeField]
    private GameObject PenetrateSkill;

    public override void Level_1()
    {
        StartCoroutine(Penetrate_Destroy());
    }

    public override void Level_2()
    {
    }

    public override void Level_3()
    {
    }

    IEnumerator Penetrate_Destroy()
    {
        b_Effectprogress = true;
        yield return new WaitForEndOfFrame();

        goalManager = FindObjectOfType<GoalManager>();

        Instantiate(PenetrateSkill, new Vector2(-1, row), Quaternion.identity);

        for (int i = 0; i < blocks.Count; i++)
        {
            State dot = blocks[i];

            if (dot != null && dot.dotState == DotState.Possible)
            {
                Instantiate(PenetrateParticle, dot.transform.position, Quaternion.identity);

                if (dot.GetComponent<Dot>() != null)
                {
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
        }

        for(int i = 0; i < Board.Instance.width; i++)
        { 
            if (Board.Instance.DecreaseRowArray[i] != null)
            {
                Board.Instance.StopCoroutine(Board.Instance.DecreaseRowArray[i]);
                Debug.Log("코루틴 정지");
            }

            Board.Instance.DecreaseRowArray[i] = Board.Instance.StartCoroutine(Board.Instance.DecreaseRowCo(i)); // 행 내리기
        }
    }




    List<State> GetPenetrateDots_Lv1(int row)
    {
        List<State> dots = new List<State>();

        for (int i = 0; i < Board.Instance.width; i++)
        {
            if (Board.Instance.allDots[i, row] != null)
            {
                dots.Add(Board.Instance.allDots[i, row]);
            }
            else if (Board.Instance.MysticDots[i, row] != null)
            {
                dots.Add(Board.Instance.MysticDots[i, row]);
            }
            else if (Board.Instance.ObstructionDots[i, row] != null)
            {
                dots.Add(Board.Instance.ObstructionDots[i, row]);
            }
        }

        return dots;
    }



}
