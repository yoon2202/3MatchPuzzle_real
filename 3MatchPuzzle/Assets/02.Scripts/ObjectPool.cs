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
        //Initialize(10);
    }

    public static void EnqueueObject(GameObject dot)
    {
        Instance.poolingObjectQueue.Enqueue(dot);
    }

    //void Initialize(int count)
    //{
    //    for(int i = 0; i < count; i++)
    //    {
    //        poolingObjectQueue.Enqueue(CreatenewObject());
    //    }
    //}

    public static GameObject GetObject()
    {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
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
