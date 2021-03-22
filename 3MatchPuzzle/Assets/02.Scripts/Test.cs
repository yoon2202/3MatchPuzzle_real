using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private int[,] it = new int[4, 4];
    private List<int> Test23 = new List<int>();

    private void Start()
    {
        it[0, 0] = 5;
        it[1, 1] = 5;

        //StartCoroutine(Testtest());
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
