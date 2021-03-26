using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Obstruction_Abstract : State
{
    private Text TimeLimit_UI;
    [SerializeField]
    protected int Time_Limit;
    private float Time_Current = 0;

    [SerializeField]
    protected int Life;

    protected ObstructionManager obstructionManager;

    public abstract void Init();

    public virtual void Start()
    {
        obstructionManager = FindObjectOfType<ObstructionManager>();
        TimeLimit_UI = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        Time_Current = Time_Limit;
        Init();


        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
 
        var Timeruns = new WaitForSeconds(1.0f);

        while (Time_Current > 0)
        {
            TimeLimit_UI.text = string.Format("{0}", Time_Current);

            if(TimeLimit_UI.gameObject.activeSelf == false)
                TimeLimit_UI.gameObject.SetActive(true);

            --Time_Current;

            yield return Timeruns;
        }

        yield return new WaitUntil(() => dotState != DotState.Moving);

        Effect();
        yield return null;
    }

    public void GetDamage(int damage)
    {
        if (Life > damage)
            Life -= damage;
        else
            Destroy_Obj();
    }

    public virtual void Effect()
    {
        Destroy_Obj();
    }


    private void Destroy_Obj()
    {
        Board.Destroy_DecreaseRow(transform);
    }

    //private void OnDestroy()
    //{
    //    Board.Instance.ObstructionDots[(int)transform.position.x, (int)transform.position.y] = null;
    //    Board.Instance.Obstruction_Queue.Dequeue();
    //}

}
