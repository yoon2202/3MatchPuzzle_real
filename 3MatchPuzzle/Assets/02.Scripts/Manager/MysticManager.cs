using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticManager : MonoBehaviour
{
    public GameObject Obj;
    private IngameGetMissionInfo ingameGetMission;

    public List<GameObject> MysticBlock = new List<GameObject>();

    public GameObject Nomal_MysticBlock;

    #region 지원사격
    private bool b_Support_shooting;
    public bool B_Support_shooting
    {
        get => b_Support_shooting;
        set
        {
            b_Support_shooting = value;
            Support_CurrentTime = 0;

            if (b_Support_shooting == true)
            {
                Debug.Log("지원사격 효과 시작");
            }
            else
            {
                Debug.Log("지원사격 효과 끝");
            }
        }
    }

    private int Support_Maxtime = 4;
    private float Support_CurrentTime = 0;
    #endregion


    void Start()
    {
        ingameGetMission = FindObjectOfType<IngameGetMissionInfo>();
    }

    private void Update()
    {
        Support_shooting_Effect();
    }

    public void Mystic_Create()
    {
        Dot[,] currentdots = Board.Instance.allDots;
        var i = 0;

        while (i < 10)
        {
            int RandomXPick = Random.Range(0, currentdots.GetLength(0));
            int RandomYPick = Random.Range(0, currentdots.GetLength(1));

            if (currentdots[RandomXPick, RandomYPick] != null && currentdots[RandomXPick, RandomYPick].dotState == DotState.Possible && FindMatches.currentMatches.Contains(currentdots[RandomXPick, RandomYPick]) == false)
            {
                currentdots[RandomXPick, RandomYPick].dotState = DotState.Targeted;

                var RandomNum = Random.Range(0, MysticBlock.Count);
                Mystic_Abstract Obj = Instantiate(MysticBlock[RandomNum], new Vector2(RandomXPick, RandomYPick), Quaternion.identity).GetComponent<Mystic_Abstract>();
                Board.Instance.MysticDots[RandomXPick, RandomYPick] = Obj;
                ObjectPool.ReturnObject(currentdots[RandomXPick, RandomYPick].gameObject);
                return;
            }
            i++;
        }
        Debug.LogError("Mystic_Create 횟수 초과");
    }

    void Support_shooting_Effect()
    {
        if (B_Support_shooting == true)
        {
            if (Support_CurrentTime < Support_Maxtime)
            {
                Support_CurrentTime += Time.unscaledDeltaTime;
            }
            else
            {
                B_Support_shooting = false;
            }
        }
    }
}
