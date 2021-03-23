using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();
    private List<GameObject> TempStorage = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize(50);
    }

    public static void EnqueueObject(GameObject dot)
    {
        Instance.poolingObjectQueue.Enqueue(dot);
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
        obj.SetActive(false);
        Instance.TempStorage.Add(obj);
        Instance.ShuffleObject();
    }
    void ShuffleObject()
    {
        if (TempStorage.Count > 20)
        {
            ShuffleList(TempStorage);
            ShuffleList(TempStorage);

            for (int i = 0; i < TempStorage.Count; i++)
            {
                Instance.poolingObjectQueue.Enqueue(TempStorage[i]);
                TempStorage[i].transform.SetParent(Instance.transform);
                TempStorage[i].transform.localPosition = Vector3.zero;
            }
            TempStorage.Clear();
        }
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
