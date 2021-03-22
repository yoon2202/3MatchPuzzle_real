using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Mystic_Abstract : MonoBehaviour, IPointerUpHandler
{
    protected int Level = 1;

    protected ScoreManager scoreManager;

    public abstract void Init();
    public abstract void Level_1();
    public abstract void Level_2();
    public abstract void Level_3();

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        Init();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Effect();
    }

    public virtual void Effect()
    {
        switch(Level)
        {
            case 1:
                Level_1();
                break;
            case 2:
                Level_2();
                break;
            case 3:
                Level_3();
                break;
        }

        Destroy_Mystic(gameObject);
    }

    private void Destroy_Mystic(GameObject Mystic)
    {
        Board.DestroyMystic(Mystic.transform);
    }

    private void OnDestroy()
    {
       Board.Instance.MysticDots[(int)transform.position.x, (int)transform.position.y] = null;
    }
}
