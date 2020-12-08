using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ActiveList", menuName = "ActiveList")]
public class ActiveList : ScriptableObject
{
    public ActiveItem[] activeList;

}
