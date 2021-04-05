using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MysticCrystal_Abstract : MonoBehaviour
{
    protected int Level = 1;

    private float PosA = 2f;
    private float PosB = 2f;

    protected int Damage = 1;

    [SerializeField]
    protected GameObject EffectParticle;

    [SerializeField]
    protected int Speed = 2;

    public abstract void Level_1();
    public abstract void Level_2();
    public abstract void Level_3();

    protected ScoreManager scoreManager;
    protected Transform target;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        target = TargetInit();

        if (target != null)
            StartCoroutine(Move_Function());
    }

    public virtual Transform TargetInit() // 타겟을 정하는 함수 재정의
    {
        return find_Block();
    }
    

    protected Transform find_Block(bool FirstFind_Obstruction = false)
    {
        if (FirstFind_Obstruction == true && Board.Instance.Obstruction_Queue.Count != 0)
        {
            return Board.Instance.Obstruction_Queue.Peek().transform;
        }
        else
        {
            Dot[,] currentdots = Board.Instance.allDots;
            Obstruction_Abstract[,] obstructiondots = Board.Instance.ObstructionDots;
            Mystic_Abstract[,] mystic_Abstracts = Board.Instance.MysticDots;

            var i = 0;

            while (i < 10)
            {
                int RandomXPick = Random.Range(0, currentdots.GetLength(0));
                int RandomYPick = Random.Range(0, currentdots.GetLength(1));

                if ((RandomXPick == (int)transform.position.x && RandomYPick == (int)transform.position.y) || Board.Instance.DecreaseRowArray[RandomXPick] != null)
                    continue;

                if (currentdots[RandomXPick, RandomYPick] != null && currentdots[RandomXPick, RandomYPick].dotState == DotState.Possible && FindMatches.currentMatches.Contains(currentdots[RandomXPick, RandomYPick]) == false) // 해당 블록의 존재 유무 판단.
                {
                    currentdots[RandomXPick, RandomYPick].dotState = DotState.Targeted;
                    return currentdots[RandomXPick, RandomYPick].transform;
                }
                else if (obstructiondots[RandomXPick, RandomYPick] != null && obstructiondots[RandomXPick, RandomYPick].dotState == DotState.Possible)
                {
                    obstructiondots[RandomXPick, RandomYPick].dotState = DotState.Targeted;
                    return obstructiondots[RandomXPick, RandomYPick].transform;
                }
                else if(mystic_Abstracts[RandomXPick, RandomYPick] != null && mystic_Abstracts[RandomXPick, RandomYPick].dotState == DotState.Possible)
                {
                    mystic_Abstracts[RandomXPick, RandomYPick].dotState = DotState.Targeted;
                    return mystic_Abstracts[RandomXPick, RandomYPick].transform;
                }

                i++;
            }
        }

        return null;
    }

    private void destroy_block()
    {
        if (target == null)
        {
            Debug.LogError("destroy_block 오류");
        }
        else
        {
            if (target.GetComponent<Dot>() != null)
            {
                ObjectPool.ReturnObject(target.gameObject);
                Board.Destroy_DecreaseRow(transform);
            }
            else if (target.GetComponent<Obstruction_Abstract>() != null)
                target.GetComponent<Obstruction_Abstract>().GetDamage(Damage);
            else if(target.GetComponent<Mystic_Abstract>() != null)
                target.GetComponent<Mystic_Abstract>().Destroy_Mystic();
        }
    }


    IEnumerator Move_Function()
    {
        yield return StartCoroutine(Move_Function_Co());


        switch (Level)
        {
            case 1:
                Level_1();
                break;
            case 2:
                Level_2();
                break;
            case 3:
                Level_3();
                break;
        }

        yield return new WaitForSeconds(0.3f);

        destroy_block();

        if(EffectParticle != null)
            Instantiate(EffectParticle, transform.position, Quaternion.identity);
    }

    #region 베지어 곡선
    IEnumerator Move_Function_Co()
    {
        float t = 0;

        Vector2[] point = new Vector2[4];
        point[0] = transform.position;
        point[1] = PointSetting(transform.position);
        point[2] = target.position;
        point[3] = target.position;

        while (t <= 1)
        {
            Vector2 changePos = new Vector2(FourPointBezier(point[0].x, point[1].x, point[2].x, point[3].x, t),
            FourPointBezier(point[0].y, point[1].y, point[2].y, point[3].y, t));

            transform.position = changePos;

            t += Time.deltaTime * Speed;
            yield return null;
        }

        if(target != null)
            transform.position = target.position;

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
    #endregion
}
