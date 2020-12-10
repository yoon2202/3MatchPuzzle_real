using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public ActiveList ActiveList;
    public GameObject Item;
    public Transform Content;

    void Start()
    {
        for(int i=0; i< ActiveList.activeList.Length; i++)
        {
          var ItemObj = Instantiate(Item, Content,false);

        }
    }


}
