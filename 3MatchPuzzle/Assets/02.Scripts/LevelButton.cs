using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public GameObject flag;
    public Text levelText;
    public int level;
    public Sprite img;
    public GameObject confirmPanel;


    public void ConfirmPanel()
    {
        confirmPanel.SetActive(true);
    }

    public void SetText(int i)
    {
        if(i<10)
            levelText.text = "0"+i.ToString();
        else
            levelText.text = i.ToString();

        level = i;
    }
    public void SetFlag(bool type)
    {
        flag.SetActive(type);
    }
    public void SetDisabled()
    {
        GetComponent<Image>().sprite = img;
        GetComponent<Button>().interactable = false;
    }
}
