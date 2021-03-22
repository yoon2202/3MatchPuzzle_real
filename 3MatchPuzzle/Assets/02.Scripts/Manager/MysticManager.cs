using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticManager : MonoBehaviour
{
    public GameObject Obj;
    private IngameGetMissionInfo ingameGetMission;

    private ScoreManager scoreManager;

    public List<GameObject> MysticBlock = new List<GameObject>();

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        ingameGetMission = FindObjectOfType<IngameGetMissionInfo>();
    }

    void Mystic_Create()
    {
        Dot[,] currentdots = Board.Instance.allDots;
        var i = 0;

        while (i < 10)
        {
            int RandomXPick = Random.Range(0, currentdots.GetLength(0));
            int RandomYPick = Random.Range(0, currentdots.GetLength(1));

            if (currentdots[RandomXPick, RandomYPick] != null) // 1. 해당 블록의 존재 유무 판단.
            {

                ObjectPool.ReturnObject(currentdots[RandomXPick, RandomYPick].gameObject);

                Instantiate(MysticBlock[0], new Vector2(RandomXPick, RandomYPick), Quaternion.identity);
                return;
            }
            i++;
        }
        Debug.LogError("Mystic_Create 횟수 초과");
    }



    public void Createobj()
    {
        Vector2 pos = ingameGetMission.gageUI_Icon.transform.position;
        var GameObj = Instantiate(Obj);
        GameObj.transform.position = pos;

        Vector2 target = new Vector2(Random.Range(0, 9), Random.Range(0, 9));

        BezierMove.Move_Function(GameObj.transform, GameObj.transform);
    }

}
