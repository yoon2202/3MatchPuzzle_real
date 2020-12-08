using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartRechargeManager : MonoBehaviour
{
    /// <summary>
    /// 보유 하트 개수
    /// </summary>
    private int m_HeartAmount = 0;
    /// <summary>
    /// 하트 최대값
    /// </summary>
    private const int MAX_HEART = 5;
    /// <summary>
    /// 하트 충전 간격(단위:초)
    /// </summary>
    public int HeartRechargeInterval = 30;
    private DateTime m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
    private Coroutine m_RechargeTimerCoroutine = null;
    /// <summary>
    /// 하트 재충전 남은 시간
    /// </summary>
    private int m_RechargeRemainTime = 0;

    public Text Wing_reprod;
    public Text Wing_Remains;
    public Button StartStage;

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        Debug.Log("ToLocalTime()" + DateTime.Now.ToLocalTime());
        Debug.Log("ToBinary()" + DateTime.Now.ToLocalTime().ToBinary());
        Debug.Log("ToBinary().ToString()" + DateTime.Now.ToLocalTime().ToBinary().ToString());
        StartStage.onClick.AddListener(() => OnClickUseHeart());
    }

    public void Init()
    {
        m_HeartAmount = 0;
        m_RechargeRemainTime = 0;
        m_AppQuitTime = new DateTime(1970, 1, 1).ToLocalTime();
        Debug.Log("heartRechargeTimer : " + m_RechargeRemainTime + "s");

    }
    /// <summary>
    /// 게임 초기화, 중간 이탈, 중간 복귀 시 실행되는 함수
    /// </summary>
    public void OnApplicationFocus(bool value)
    {
        Debug.Log("OnApplicationFocus() : " + value);
        if (value)
        {
            LoadHeartInfo();
            LoadAppQuitTime();
            SetRechargeScheduler();
        }
        else
        {
            SaveHeartInfo();
            SaveAppQuitTime();
        }
    }
    /// <summary>
    /// 게임 종료 시 실행되는 함수
    /// </summary>
    public void OnApplicationQuit()
    {
        Debug.Log("GoodsRechargeTester: OnApplicationQuit()");
        SaveHeartInfo();
        SaveAppQuitTime();
    }
    /// <summary>
    /// 버튼 이벤트에 이 함수를 연동한다.
    /// </summary>
    public void OnClickUseHeart()
    {
        Debug.Log("OnClickUseHeart");
        UseHeart();
    }

    public void UseHeart(Action onFinish = null)
    {
        if (m_HeartAmount <= 0)
        {
            return;
        }

        m_HeartAmount--;
        Wing_Remains.text = m_HeartAmount + "/" + MAX_HEART;
        //heartAmountLabel.text = string.Format("Hearts : {0}", m_HeartAmount.ToString());
        if (m_RechargeTimerCoroutine == null)
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(HeartRechargeInterval));
        }
        onFinish?.Invoke();
    }



    public bool LoadHeartInfo()
    {
        Debug.Log("LoadHeartInfo");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("HeartAmount"))
            {
                Debug.Log("PlayerPrefs has key : HeartAmount");
                m_HeartAmount = PlayerPrefs.GetInt("HeartAmount");
                if (m_HeartAmount < 0)
                {
                    m_HeartAmount = 0;
                }
            }
            else
            {
                m_HeartAmount = MAX_HEART;
            }
            Debug.Log("Loaded HeartAmount : " + m_HeartAmount);
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadHeartInfo Failed (" + e.Message + ")");
        }
        return result;
    }

    public bool SaveHeartInfo()
    {
        Debug.Log("SaveHeartInfo");
        bool result = false;
        try
        {
            PlayerPrefs.SetInt("HeartAmount", m_HeartAmount);
            PlayerPrefs.Save();
            Debug.Log("Saved HeartAmount : " + m_HeartAmount);
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveHeartInfo Failed (" + e.Message + ")");
        }
        return result;
    }

    public bool LoadAppQuitTime()
    {
        Debug.Log("LoadAppQuitTime");
        bool result = false;
        try
        {
            if (PlayerPrefs.HasKey("AppQuitTime"))
            {
                Debug.Log("PlayerPrefs has key : AppQuitTime");
                var appQuitTime = string.Empty;
                appQuitTime = PlayerPrefs.GetString("AppQuitTime");
                m_AppQuitTime = DateTime.FromBinary(Convert.ToInt64(appQuitTime)); // string을 Datetime으로 변환
            }
            Debug.Log(string.Format("Loaded AppQuitTime : {0}", m_AppQuitTime.ToString()));
            //appQuitTimeLabel.text = string.Format("AppQuitTime : {0}", m_AppQuitTime.ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("LoadAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }

    public bool SaveAppQuitTime()
    {
        Debug.Log("SaveAppQuitTime");
        bool result = false;
        try
        {
            var appQuitTime = DateTime.Now.ToLocalTime().ToBinary().ToString();
            PlayerPrefs.SetString("AppQuitTime", appQuitTime);
            PlayerPrefs.Save();
            Debug.Log("Saved AppQuitTime : " + DateTime.Now.ToLocalTime().ToString());
            result = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("SaveAppQuitTime Failed (" + e.Message + ")");
        }
        return result;
    }

    public void SetRechargeScheduler(Action onFinish = null)
    {
        if (m_RechargeTimerCoroutine != null)
        {
            StopCoroutine(m_RechargeTimerCoroutine);
        }
        var timeDifferenceInSec = (int)((DateTime.Now.ToLocalTime() - m_AppQuitTime).TotalSeconds);
        Debug.Log("TimeDifference In Sec :" + timeDifferenceInSec + "s");
        var heartToAdd = timeDifferenceInSec / HeartRechargeInterval;
        Debug.Log("Heart to add : " + heartToAdd);
        var remainTime = timeDifferenceInSec % HeartRechargeInterval;
        Debug.Log("RemainTime : " + remainTime);
        m_HeartAmount += heartToAdd;
        if (m_HeartAmount >= MAX_HEART)
        {
            m_HeartAmount = MAX_HEART;
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(remainTime, onFinish));
        }
        //heartAmountLabel.text = string.Format("Hearts : {0}", m_HeartAmount.ToString());
        Wing_Remains.text = m_HeartAmount + "/" + MAX_HEART;
        //Debug.Log("HeartAmount : " + m_HeartAmount);
    }

    private IEnumerator DoRechargeTimer(int remainTime, Action onFinish = null)
    {
        Debug.Log("DoRechargeTimer");
        if (remainTime <= 0)
        {
            m_RechargeRemainTime = HeartRechargeInterval;
        }
        else
        {
            m_RechargeRemainTime = remainTime;
        }
       TimeSpan resultremainTime = TimeSpan.FromSeconds(m_RechargeRemainTime);
        Debug.Log("heartRechargeTimer : " + m_RechargeRemainTime + "s");
        Wing_reprod.text = resultremainTime.Minutes+":"+resultremainTime.Seconds;
        //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);

        while (m_RechargeRemainTime > 0)
        {
            resultremainTime = TimeSpan.FromSeconds(m_RechargeRemainTime);
            Debug.Log("heartRechargeTimer : " + m_RechargeRemainTime + "s");
            Wing_reprod.text = resultremainTime.Minutes + ":" + resultremainTime.Seconds;
            //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);
            m_RechargeRemainTime -= 1;
            yield return new WaitForSeconds(1f);
        }

        m_HeartAmount++;
        if (m_HeartAmount >= MAX_HEART)
        {
            m_HeartAmount = MAX_HEART;
            m_RechargeRemainTime = 0;
            Wing_reprod.text = resultremainTime.Minutes + ":" + resultremainTime.Seconds;
            //heartRechargeTimer.text = string.Format("Timer : {0} s", m_RechargeRemainTime);
            Debug.Log("HeartAmount reached max amount");
            m_RechargeTimerCoroutine = null;
        }
        else
        {
            m_RechargeTimerCoroutine = StartCoroutine(DoRechargeTimer(HeartRechargeInterval, onFinish));
        }
        //heartAmountLabel.text = string.Format("Hearts : {0}", m_HeartAmount.ToString());
        Wing_Remains.text = m_HeartAmount + "/" + MAX_HEART;
        Debug.Log("HeartAmount : " + m_HeartAmount);
    }
}
