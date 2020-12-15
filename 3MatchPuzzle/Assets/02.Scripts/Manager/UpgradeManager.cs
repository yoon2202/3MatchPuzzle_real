using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public ActiveList ActiveList;
    public GameObject item;
    public Transform Content;

    void Start()
    {
        for (int i = 0; i < ActiveList.activeList.Length; i++)
        {
            GameObject ItemObj = Instantiate(item, Content, false);
            Item Item = ItemObj.GetComponent<Item>();
            Item.itemName.text = ActiveList.activeList[i].ActiveName;
            Item.img.sprite = ActiveList.activeList[i].Image;
            Item.Context.text = ActiveList.activeList[i].ActiveContent.Replace("\\n", "\n");
            Item.level.text = ActiveList.activeList[i].CurrentLevel.ToString();
            Item.Price.text = ActiveList.activeList[i].Baseprice.ToString();

            Item.ItemNumber = i;
            Item.CurrentLevel = ActiveList.activeList[i].CurrentLevel;
            Item.MaxLevel = ActiveList.activeList[i].MaxLevel;
        }
    }


}
