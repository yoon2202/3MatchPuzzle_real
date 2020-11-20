using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlankGoal
{
   public int numberNeeded;
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;
}


public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoals;
    public List<GoalPanel> currentGoals = new List<GoalPanel>();
    public GameObject goalPrefabs;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;
    private EndManager endGame;
    private Board board;

    void Start()
    {
        board = FindObjectOfType<Board>();
        endGame = FindObjectOfType<EndManager>();
        GetGoals();
        SetUpIntroGoals();
    }
    void GetGoals()
    {
        if(board != null)
        {
            if(board.world != null)
            {
                if(board.world.levels[board.level] != null)
                {
                    levelGoals = board.world.levels[board.level].levelGoals;
                }
            }
        }
    }
    void SetUpIntroGoals()
    {
        for(int i = 0;  i < levelGoals.Length; i++)
        {
            //GameObject goal = Instantiate(goalPrefabs, goalIntroParent.transform.position, Quaternion.identity);
            //goal.transform.SetParent(goalIntroParent.transform);

            //GoalPanel panel = goal.GetComponent<GoalPanel>();
            //panel.thisSprite = levelGoals[i].goalSprite;
            //panel.thisString = "0/" + levelGoals[i].numberNeeded;

            GameObject gameGoal = Instantiate(goalPrefabs, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform,false);
            GoalPanel panel = gameGoal.GetComponent<GoalPanel>();
            currentGoals.Add(panel);
            panel.thisSprite = levelGoals[i].goalSprite;

            panel.thisString = "0/" + levelGoals[i].numberNeeded;
        }
    }

    // Update is called once per frame
    public void UpdateGoals()
    {
        int goalsCompleted = 0;
        for (int i = 0; i < levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;

            if (levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                currentGoals[i].thisText.text = levelGoals[i].numberNeeded + "/" + levelGoals[i].numberNeeded;

            }
        }
        if (goalsCompleted >= levelGoals.Length)
        {
            //if(endGame != null)
                // 게임 클리어시 이벤트 필요.
            //Debug.Log("미션 클리어");
        }
    }

    public void CompareGoal(string goalToCompare)  // 게임 목표에 있는 tag인 지 확인후 갯수를 업데이트시킨다.
    {
        for(int i = 0; i< levelGoals.Length; i ++)
        {
            if (goalToCompare == levelGoals[i].matchValue)
                levelGoals[i].numberCollected++;
        }
    }
    
}
