using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private int jj = 0;

    List<Coroutine> runningCoroutine  = new List<Coroutine>();  //코루틴 변수. 1개의 루틴만 돌리기 위해 저장한다.

    private SpriteRenderer spriteRenderer;
    private Texture2D texture2D;
    //private Renderer renderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //InvokeRepeating("Testtest", 0, 5.0f);

        //Invoke("TestStop", 8.0f);
    }



    void Testtest()
    {
        runningCoroutine.Add(StartCoroutine(Test_co(jj++)));
    }

    void TestStop()
    {
        for(int i =0; i< runningCoroutine.Count; i++)
        {
            StopCoroutine(runningCoroutine[i]);
        }
    }

    IEnumerator Test_co(int i)
    {
        while (true)
        {
            Debug.Log(i);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
