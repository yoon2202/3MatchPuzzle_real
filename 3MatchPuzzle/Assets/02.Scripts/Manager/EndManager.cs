using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameType
{
    Odd,Even,Odd_5,Even_10
}

public enum MatchType
{
    Odd, Even, Odd_5, Even_10
}

[System.Serializable]
public class EndGameRequirements
{
    public GameType gameType;
    public int counterValue;
    public int scoreGoals;
    public MatchType matchType;
    //매치 관련 변수 추가 예정
}

public class EndManager : MonoBehaviour
{
    public GameObject movesLabel;
    public GameObject timeLabel;
    public Text counter;
    public EndGameRequirements requirements;
    public int currentCounterValue;
    private Board board;
    private float timeSecond;

    void Start()
    {
        board = FindObjectOfType<Board>();
        SetgameType();
        SetupGame();
    }

    void SetgameType()
    {
        if (board.world != null)
        {
            if (board.level < board.world.levels.Length)
            {
                if (board.world.levels[board.level] != null)
                {
                    requirements = board.world.levels[board.level].endGameRequirements;
                }
            }
        }
    }

    void SetupGame() // 게임 세팅할때 활용
    {
        currentCounterValue = requirements.counterValue;
        if(requirements.gameType == GameType.Odd)
        {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);
        }
        else
        {
            timeSecond = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }

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
    void Update()
    {
        if(requirements.gameType == GameType.Even && currentCounterValue > 0)
        {
            timeSecond -= Time.deltaTime;
            if(timeSecond <= 0)
            {
                DecreaseCounterValue();
                timeSecond = 1;
            }
        }
    }
}
