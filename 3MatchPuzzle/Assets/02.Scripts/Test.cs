using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Test : MonoBehaviour
{
    private Queue<int> list = new Queue<int>();

    private void Start()
    {
        //list.Enqueue(null);
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
