using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();
    private List<GameObject> TempStorage = new List<GameObject>();

    private bool B_IsSuffle;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        B_IsSuffle = false;
        Initialize(100);
    }

    void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            poolingObjectQueue.Enqueue(CreatenewObject());
        }
    }

    GameObject CreatenewObject()
    {
        int dotToUse = Random.Range(0, Board.dots.Length);
        GameObject piece = Instantiate(Board.dots[dotToUse], transform);
        piece.SetActive(false);
        return piece;
    }

    public static GameObject GetObject(int i, int j, int offset)
    {
        Vector2 tempPosition = new Vector2(i, offset);
        var obj = Instance.poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = tempPosition;
        obj.GetComponent<Dot>().column = i;
        obj.GetComponent<Dot>().row = j;
        obj.SetActive(true);
        return obj;
    }

    public static void ReturnObject(GameObject obj)
    {
        Dot obj_Dot = obj.GetComponent<Dot>();
        Board.Instance.allDots[obj_Dot.column, obj_Dot.row] = null;
        obj.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        obj.transform.localPosition = Vector3.zero;

        obj_Dot.column = -1;
        obj_Dot.row = -1;
        obj_Dot.dotState = DotState.Possible;

        Instance.TempStorage.Add(obj);

        if (Instance.B_IsSuffle == false && Instance.TempStorage.Count > 30)
            Instance.StartCoroutine(Instance.ShuffleObject());
    }

    IEnumerator ShuffleObject()
    {
        B_IsSuffle = true;

        ShuffleList(TempStorage);

        for (int i = 0; i < TempStorage.Count; i++)
        {
            Instance.poolingObjectQueue.Enqueue(TempStorage[i]);
            yield return null;
        }

        TempStorage.Clear();
        Debug.Log("셔플");

        B_IsSuffle = false;
        yield return null;
    }

    void ShuffleList<T>(List<T> list)
    {
        int random1;
        int random2;

        T tmp;

        var i = 0;
        while (i < 3)
        {
            for (int index = 0; index < list.Count; ++index)
            {
                random1 = Random.Range(0, list.Count);
                random2 = Random.Range(0, list.Count);

                tmp = list[random1];
                list[random1] = list[random2];
                list[random2] = tmp;
            }
            i++;
        }
    }

}
