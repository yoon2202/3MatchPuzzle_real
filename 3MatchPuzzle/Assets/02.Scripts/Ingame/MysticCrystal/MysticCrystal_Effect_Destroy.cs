﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticCrystal_Effect_Destroy : MonoBehaviour
{
    public float DestroyTime = 1.5f;

    void Start()
    {
        Destroy(gameObject, 1.5f);
    }

}
