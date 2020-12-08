using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ActiveItem", menuName = "Active")]
public class ActiveItem : ScriptableObject
{
    public string ActiveName;
    public Sprite Image;
    public string ActiveContent;
    public int CurrentLevel;
    public int MaxLevel;
    public int Baseprice;

}
