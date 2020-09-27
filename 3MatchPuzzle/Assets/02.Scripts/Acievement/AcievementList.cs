using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AcievementList", menuName = "AcievementList")]
public class AcievementList : ScriptableObject
{
    public AcievementScriptable[] AcievementScriptables;
}