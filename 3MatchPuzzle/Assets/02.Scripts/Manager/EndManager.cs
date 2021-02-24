using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EndManager : MonoBehaviour
{
    public GameObject movesLabel;
    public GameObject timeLabel;
    public Text counter;
    //public EndGameRequirements requirements;
    public int currentCounterValue;
    private Board board;
    private float timeSecond;

    void Start()
    {
        board = FindObjectOfType<Board>();
        //SetupGame();
    }

  
    //void SetupGame() // 게임 세팅할때 활용
    //{
    //    //currentCounterValue = requirements.counterValue;
    //    if(requirements.gameType == GameType.Odd)
    //    {
    //        movesLabel.SetActive(true);
    //        timeLabel.SetActive(false);
    //    }
    //    else
    //    {
    //        timeSecond = 1;
    //        movesLabel.SetActive(false);
    //        timeLabel.SetActive(true);
    //    }
    //    counter.text = "" + currentCounterValue;
    //}

    public void DecreaseCounterValue()
    {
        if (board.currentState != GameState.pause)
        {
            currentCounterValue--;
            counter.text = currentCounterValue.ToString();

            if (currentCounterValue <= 0)
            {
                board.currentState = GameState.lose;
                Debug.Log("you Lose");
                currentCounterValue = 0;
                counter.text = currentCounterValue.ToString();
            }
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if(requirements.gameType == GameType.Even && currentCounterValue > 0)
    //    {
    //        timeSecond -= Time.deltaTime;
    //        if(timeSecond <= 0)
    //        {
    //            DecreaseCounterValue();
    //            timeSecond = 1;
    //        }
    //    }
    //}
}
