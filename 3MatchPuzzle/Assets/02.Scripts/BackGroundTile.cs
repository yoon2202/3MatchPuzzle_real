using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundTile : MonoBehaviour
{
    //public GameObject[] dots;
    public int hitPoints;
    private SpriteRenderer sprite;
    private GoalManager goalManager;
    private void Start()
    {
        goalManager = FindObjectOfType<GoalManager>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (hitPoints <= 0)
        {
            if(goalManager != null)
            {
                goalManager.CompareGoal(this.gameObject.name);
                goalManager.UpdateGoals();
            }
            Destroy(this.gameObject);
        }
    }


    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
    }
    //void Initalize()
    //{
    //    int dotTouse = Random.Range(0, dots.Length);
    //    GameObject dot = Instantiate(dots[dotTouse], transform.position, Quaternion.identity);
    //    dot.transform.parent = this.transform;
    //    dot.name = this.gameObject.name;
    //}

    void MakeLighter()
    {
        Color color = sprite.color;
        float newAlpha = color.a * 0.5f;
        sprite.color = new Color(color.r, color.g, color.b, newAlpha);
    }
}
