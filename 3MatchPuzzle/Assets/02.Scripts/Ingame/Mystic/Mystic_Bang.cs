using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mystic_Bang : Mystic_Abstract
{
    public override void Init()
    {
    }

    public override void Level_1()
    {
        destroy_block();
        scoreManager.GetScore(5);
    }

    public override void Level_2()
    {
        destroy_block(true);
        scoreManager.GetScore(7);
    }

    public override void Level_3()
    {
        destroy_block(true,100);
        scoreManager.GetScore(10);
    }

    private void destroy_block(bool FirstFind_Obstruction = false, int Damage = 1)
    {
        GameObject block = find_Block(FirstFind_Obstruction);

        if (block.GetComponent<Obstruction_Abstract>() == null)
            ObjectPool.ReturnObject(block);
        else
            block.GetComponent<Obstruction_Abstract>().GetDamage(Damage);
    }


    private GameObject find_Block(bool FirstFind_Obstruction = false)
    {
        if (FirstFind_Obstruction == true && Board.Instance.Obstruction_Queue.Count != 0)
        {
            return Board.Instance.Obstruction_Queue.Peek().gameObject;
        }
        else
        {
            Dot[,] currentdots = Board.Instance.allDots;
            Obstruction_Abstract[,] obstructiondots = Board.Instance.ObstructionDots;

            var i = 0;

            while (i < 10)
            {
                int RandomXPick = Random.Range(0, currentdots.GetLength(0));
                int RandomYPick = Random.Range(0, currentdots.GetLength(1));

                if (currentdots[RandomXPick, RandomYPick] != null) // 1. 해당 블록의 존재 유무 판단.
                {
                    return currentdots[RandomXPick, RandomYPick].gameObject;
                }
                else if (obstructiondots[RandomXPick, RandomYPick] != null)
                {
                    return currentdots[RandomXPick, RandomYPick].gameObject;
                }
                i++;
            }

            Debug.LogError("destroy_Block 횟수 초과");
        }

        return null;
    }

}
