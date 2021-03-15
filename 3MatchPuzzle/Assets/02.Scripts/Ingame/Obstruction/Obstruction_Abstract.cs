using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class Obstruction_Abstract : MonoBehaviour
{
    private Text TimeLimit_UI;

    [SerializeField]
    protected int Time_Limit;

    private float Time_Current = 0;

    public abstract void Init();
    public abstract void Effect();

    public virtual void Start()
    {
        TimeLimit_UI = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        Time_Current = Time_Limit;
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

        Effect();
        yield return null;
    }
}
