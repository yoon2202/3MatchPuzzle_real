using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements22 : MonoBehaviour
{
    public static Achievements22 instance = null;
    public List<AchievementsCheck> Achievements = new List<AchievementsCheck>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
  


}

