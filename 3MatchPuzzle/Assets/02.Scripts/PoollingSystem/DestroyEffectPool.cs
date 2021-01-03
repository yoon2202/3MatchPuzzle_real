using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class QueueList
{
    public Queue<GameObject> queue;
    public string name;
    public GameObject RootObj;

    public QueueList(GameObject obj) // 생성자
    {
        queue = new Queue<GameObject>();
        RootObj = new GameObject();
        name = obj.tag;
        RootObj.name = name;
        RootObj.transform.SetParent(DestroyEffectPool.Instance.transform);
    }
}

public class DestroyEffectPool : MonoBehaviour
{
    public static DestroyEffectPool Instance;
    public GameObject[] DestroyEffectObj;

    private QueueList[] queueLists;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Initialize(30);
    }

    void Initialize(int count)
    {
        if (DestroyEffectObj.Length <= 0)
            return;
        else
            queueLists = new QueueList[DestroyEffectObj.Length];

        for (int index = 0; index < DestroyEffectObj.Length; index++)
        {
            queueLists[index] = new QueueList(DestroyEffectObj[index]);

            for (int i = 0; i < count; i++)
            {
                queueLists[index].queue.Enqueue(CreatenewObject(index, queueLists[index].RootObj));
            }
        }
    }

    GameObject CreatenewObject(int index,GameObject parent)
    {
        GameObject piece = Instantiate(DestroyEffectObj[index], parent.transform);
        return piece;
    }

    public static GameObject GetObject(int i, int j,GameObject Dot)
    {
        Vector2 tempPosition = new Vector2(i, j);
            
        for(int index =0; index < Instance.queueLists.Length; index++)
        {
            if(Dot.CompareTag(Instance.queueLists[index].name))
            {
                var obj = Instance.queueLists[index].queue.Dequeue();
                obj.transform.SetParent(null);
                obj.transform.position = tempPosition;
                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }

    public static void ReturnObject(GameObject DestroyEffect)
    {
        for (int index = 0; index < Instance.queueLists.Length; index++)
        {
            if (DestroyEffect.CompareTag(Instance.queueLists[index].name))
            {
                Instance.queueLists[index].queue.Enqueue(DestroyEffect);
                DestroyEffect.transform.SetParent(Instance.queueLists[index].RootObj.transform);
                DestroyEffect.SetActive(false);
                return;
            }
        }
    }
}
