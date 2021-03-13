using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Testtest());
    }

    IEnumerator Testtest()
    {
        int count = 0;
        while(true)
        {
            Debug.Log(count);
            yield return new WaitForSeconds(1.0f);
            count++;
        }

        yield return null;
    }
}
