using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    private Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        Initialize(90);
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

    public static GameObject GetObject(int i, int j,int offset)
    {
        Vector2 tempPosition = new Vector2(i, j + offset);
        var obj = Instance.poolingObjectQueue.Dequeue();
        obj.transform.SetParent(null);
        obj.transform.position = tempPosition;
        obj.GetComponent<Dot>().row = j;
        obj.GetComponent<Dot>().column = i;
        obj.SetActive(true);
        return obj;
        //else
        //{
        //    var newobj = Instance.CreatenewObject();
        //    newobj.transform.SetParent(null);
        //    newobj.gameObject.SetActive(true);
        //    return newobj;
        //}
    }

    public static void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        obj.transform.localPosition = Vector3.zero;
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
