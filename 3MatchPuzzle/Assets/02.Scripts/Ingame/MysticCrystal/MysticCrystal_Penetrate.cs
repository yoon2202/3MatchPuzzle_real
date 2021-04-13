using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticCrystal_Penetrate : MysticCrystal_Abstract
{
    private GoalManager goalManager;

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

        var Row = (int)transform.position.y;
        yield return new WaitForEndOfFrame();

        goalManager = FindObjectOfType<GoalManager>();

        Instantiate(PenetrateSkill, new Vector2(-1, Row), Quaternion.identity);

        for (int i = 0; i < Board.Instance.width; i++)
        {
            if (Board.Instance.DecreaseRowArray[i] != null)
            {
                Board.Instance.StopCoroutine(Board.Instance.DecreaseRowArray[i]);
                Debug.Log("코루틴 정지");
            }


            if (Board.Instance.allDots[i, Row] != null && Board.Instance.allDots[i, Row].dotState == DotState.Possible)
            {
                if (Board.Instance.allDots[i, Row].gameObject.activeSelf)
                {
                    ObjectPool.ReturnObject(Board.Instance.allDots[i, Row].gameObject);
                }
            }
            else if (Board.Instance.MysticDots[i, Row] != null && Board.Instance.MysticDots[i, Row].dotState == DotState.Possible)
            {
                Board.Instance.MysticDots[i, Row].GetComponent<Mystic_Abstract>().Destroy_Mystic();
            }
            else if (Board.Instance.ObstructionDots[i, Row] != null && Board.Instance.ObstructionDots[i, Row].dotState == DotState.Possible)
            {
                Board.Instance.ObstructionDots[i, Row].GetComponent<Obstruction_Abstract>().GetDamage(Damage);
            }

            if (goalManager != null)
            {
                goalManager.Update_CurrentGage(1);
                goalManager.Update_CurrentScore((int)scoreManager.GetScore(0.4f));
            }

            yield return null;
        }

        yield return null;

        for (int i = 0; i < Board.Instance.width; i++)
        {
            Board.Instance.DecreaseRowArray[i] = Board.Instance.StartCoroutine(Board.Instance.DecreaseRowCo(i)); // 행 내리기
            yield return null;
        }

        b_Effectprogress = false;
      
    }

}
