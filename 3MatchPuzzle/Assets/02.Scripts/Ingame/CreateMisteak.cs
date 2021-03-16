using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMisteak : MonoBehaviour
{
    public GameObject Obj;

    private IngameGetMissionInfo ingameGetMission;

    private void Start()
    {
        ingameGetMission = FindObjectOfType<IngameGetMissionInfo>();
    }

    public void Createobj()
    {
        Vector2 pos = ingameGetMission.gageUI_Icon.transform.position;
        var GameObj = Instantiate(Obj);
        GameObj.transform.position = pos;

        Vector2 target = new Vector2(Random.Range(0, 9), Random.Range(0, 9));

        BezierMove.Move_Function(GameObj.transform, target);
    }
}
