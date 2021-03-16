using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructionManager : MonoBehaviour
{
    private int block_Count;
    public int Block_Count
    {
        get => block_Count;
        set
        {
            block_Count = value;
            Debug.Log(block_Count);
            if (block_Count == 5)
            {
                Block_Count = 0;
                Obstruction_Create();
            }
        }
    }

    private float timer_Max = 5;
    private float timer_Current;

    public List<GameObject> ObstructionBlock = new List<GameObject>();
    

    void Update()
    {
        if(Board.Instance.b_PlayStart == true)
        {
            if(timer_Current < timer_Max)
            {
                timer_Current += Time.unscaledDeltaTime;
            }
            else
            {
                Obstruction_Create();
                timer_Current = 0;
            }
        }
    }

    void Obstruction_Create()
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

                var block = Instantiate(ObstructionBlock[0], new Vector2(RandomXPick, RandomYPick), Quaternion.identity);
                Board.Instance.ObstructionDots[RandomXPick, RandomYPick] = block.GetComponent<Obstruction_Abstract>();
                return;
            }

            i++;
        }

        Debug.LogError("Obstruction_Create 횟수 초과");
    }
}
