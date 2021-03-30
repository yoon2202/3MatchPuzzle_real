using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{

    private int BaseScore = 20;

    [HideInInspector]
    public float Rate = 1.0f;

    [HideInInspector]
    private int streakValue = 1;


    public double GetScore(float Multiple = 1)
    {
        return Math.Floor(BaseScore * Rate * Multiple);
    }

}
