using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMove : MonoBehaviour
{
    public static BezierMove Instance;

    [SerializeField]
    private float PosA = 0.55f;
    [SerializeField]
    private float PosB = 0.45f;


    private void Awake()
    {
        Instance = this;
    }

    public static void Move_Function(Transform origin, Transform target, int speed = 5)
    {
       Instance.StartCoroutine(Instance.Move_Function_Co(origin, target, speed));
    }

    IEnumerator Move_Function_Co(Transform origin, Transform target,int speed)
    {
        float t = 0;

        Vector2[] point = new Vector2[4];
        point[0] = origin.position;
        point[1] = PointSetting(origin.position);
        point[2] = PointSetting(target.position);
        point[3] = target.position;

        while(t <= 1)
        {
            Vector2 changePos = new Vector2(FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x,t),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y,t));

            origin.position = changePos;

            t += Time.deltaTime * speed;
            yield return null;
        }
        origin.position = target.position;
        yield return null;
    }

    Vector2 PointSetting(Vector2 origin)
    {
        float x, y;

        x = PosA * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.x;
        y = PosB * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad) + origin.y;

        return new Vector2(x, y);
    }

    float FourPointBezier(float p0, float p1, float p2, float p3, float t)
    {
        return Mathf.Pow((1 - t), 3) * p0 + Mathf.Pow((1 - t), 2) * 3 * t * p1 + Mathf.Pow(t, 2) * 3 * (1 - t) * p2 + Mathf.Pow(t, 3) * p3;
    }
}
