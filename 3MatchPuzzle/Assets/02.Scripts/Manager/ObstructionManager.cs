using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructionManager : MonoBehaviour
{
    private int block_Count;
    public int Block_Count
    {
        get => block_Count;
        set
        {
            block_Count = value;
            Debug.Log(block_Count);
            if (block_Count == 5)
            {
                Block_Count = 0;
                Obstruction_Create();
            }
        }
    }

    private float timer_Max = 5;
    private float timer_Current;

    #region 더스트
    private bool b_Dust_EffectStart;
    public bool B_Dust_EffectStart
    {
        get => b_Dust_EffectStart;
        set
        {
            b_Dust_EffectStart = value;
            Dust_CurrentTime = 0;

            if (b_Dust_EffectStart == true)
            {
                scoreManager.Rate = 0.7f;
                Debug.Log("더스트 효과 시작");
            }
            else
            {
                scoreManager.Rate = 1.0f;
                Debug.Log("더스트 효과 끝");
            }
        }
    }

    private int Dust_Maxtime = 3;
    private float Dust_CurrentTime = 0;
    #endregion

    #region 엑시드 젤리
    private bool b_Acidjelly_EffectStart;
    public bool B_Acidjelly_EffectStart
    {
        get => b_Acidjelly_EffectStart;
        set
        {
            b_Acidjelly_EffectStart = value;
            Acidjelly_CurrentTime = 0;

            if (b_Dust_EffectStart == true)
            {
                Debug.Log("엑시드 젤리 효과 시작");
            }
            else
            {
                Debug.Log("엑시드 젤리 효과 끝");
            }
        }
    }

    private int Acidjelly_Maxtime = 3;
    private float Acidjelly_CurrentTime = 0;
    #endregion

    #region 퀸 비
    private bool b_Queenbee_EffectStart;
    public bool B_Queenbee_EffectStart
    {
        get => b_Queenbee_EffectStart;
        set
        {
            b_Queenbee_EffectStart = value;
            Queenbee_CurrentTime = 0;

            if (b_Queenbee_EffectStart == true)
            {
                Debug.Log("퀸 비 효과 시작");
            }
            else
            {
                Debug.Log("퀸 비 효과 끝");
            }
        }
    }

    private int Queenbee_Maxtime = 5;
    private float Queenbee_CurrentTime = 0;
    #endregion 

    public List<GameObject> ObstructionBlock = new List<GameObject>();
    private int Countnum = 0;

    ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        if(Board.Instance.b_PlayStart == true)
        {
            if(timer_Current < timer_Max)
            {
                timer_Current += Time.unscaledDeltaTime;
            }
            else
            {
                Obstruction_Create();
                timer_Current = 0;
            }
        }

        DustEffect();
        AcidjellyEffect();
        QueenbeeEffect();
    }

    void Obstruction_Create()
    {
        Dot[,] currentdots = Board.Instance.allDots;
        var i = 0;

        while (i < 10)
        {
            int RandomXPick = Random.Range(0, currentdots.GetLength(0));
            int RandomYPick = Random.Range(0, currentdots.GetLength(1));

            if (currentdots[RandomXPick, RandomYPick] != null) // 1. 해당 블록의 존재 유무 판단.
            {
                if (Countnum == ObstructionBlock.Count)
                    Countnum = 0;

                ObjectPool.ReturnObject(currentdots[RandomXPick, RandomYPick].gameObject);

                var block = Instantiate(ObstructionBlock[Countnum], new Vector2(RandomXPick, RandomYPick), Quaternion.identity);
                Board.Instance.ObstructionDots[RandomXPick, RandomYPick] = block.GetComponent<Obstruction_Abstract>();
                Countnum++;
                return;
            }

            i++;
        }

        Debug.LogError("Obstruction_Create 횟수 초과");
    }

    void DustEffect()
    {
        if(B_Dust_EffectStart == true)
        {
            if(Dust_CurrentTime < Dust_Maxtime)
            {
                Dust_CurrentTime += Time.unscaledDeltaTime;
            }
            else
            {
                B_Dust_EffectStart = false;
            }
        }
    }

    void AcidjellyEffect()
    {
        if (B_Acidjelly_EffectStart == true)
        {
            if (Acidjelly_CurrentTime < Acidjelly_Maxtime)
            {
                Acidjelly_CurrentTime += Time.unscaledDeltaTime;
            }
            else
            {
                B_Acidjelly_EffectStart = false;
            }
        }
    }

    void QueenbeeEffect()
    {
        if (B_Queenbee_EffectStart == true)
        {
            if (Queenbee_CurrentTime < Queenbee_Maxtime)
            {
                Queenbee_CurrentTime += Time.unscaledDeltaTime;
            }
            else
            {
                B_Queenbee_EffectStart = false;
            }
        }
    }



}
