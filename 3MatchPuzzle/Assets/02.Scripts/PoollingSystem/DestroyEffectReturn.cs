using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffectReturn : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("PoolingReturn", 2.0f);
    }

    void PoolingReturn()
    {
        DestroyEffectPool.ReturnObject(gameObject);
    }
}
