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
                Instantiate(MysticBlock[0], new Vector2(RandomXPick, RandomYPick), Quaternion.identity);
                ObjectPool.ReturnObject(currentdots[RandomXPick, RandomYPick].gameObject);
                return;
            }
            i++;
        }
        Debug.LogError("Mystic_Create 횟수 초과");
    }



    //public void Createobj()
    //{
    //    Vector2 pos = ingameGetMission.gageUI_Icon.transform.position;
    //    var GameObj = Instantiate(Obj);
    //    GameObj.transform.position = pos;

    //    Vector2 target = new Vector2(Random.Range(0, 9), Random.Range(0, 9));

    //    BezierMove.Move_Function(GameObj.transform, GameObj.transform);
    //}

}
