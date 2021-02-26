using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public boxbox boxbox;

    List<box> boxs = new List<box>();

    private void Start()
    {
        boxs.Add(boxbox);

        boxs[0].Test();
    }
}
