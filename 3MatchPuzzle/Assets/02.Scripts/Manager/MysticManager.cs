using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticManager : MonoBehaviour
{
    public GameObject Obj;
    private IngameGetMissionInfo ingameGetMission;

    public List<GameObject> MysticBlock = new List<GameObject>();

    void Start()
    {
        ingameGetMission = FindObjectOfType<IngameGetMissionInfo>();
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

}
