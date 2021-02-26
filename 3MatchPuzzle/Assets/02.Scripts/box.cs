using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
   public virtual void Test()
    {
        Debug.Log("box");
    }

    private void Start()
    {
        Test();
    }
}
