using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ActiveList ActiveList;

    [HideInInspector]
    public int ItemNumber;

    private int currentLevel;
    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }

        set
        {
            currentLevel = value;

            if (currentLevel == MaxLevel)
                level.text = "Max";
            else
            {
                level.text = currentLevel.ToString();
            }
            ActiveList.activeList[ItemNumber].CurrentLevel = currentLevel;
        }
    }
    [HideInInspector]
    public int MaxLevel;

    public Image img;
    public Text itemName;
    public Text level;
    public Text Context;
    public Button button;
    public Text Price;


    private void Start()
    {
        button.onClick.AddListener(() => ButtonEvent());
    }

    void ButtonEvent()
    {
        if (CurrentLevel < MaxLevel)
        {
            CurrentLevel++;
        }
    }
}
