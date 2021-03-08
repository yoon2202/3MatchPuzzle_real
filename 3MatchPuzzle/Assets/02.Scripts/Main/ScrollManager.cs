using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform contentTr;

    [SerializeField]
    private Slider TabSlider;
    [SerializeField]
    private RectTransform[] BtnRect;
    [SerializeField]
    private RectTransform[] BtnImageRect;

    const int SIZE = 4;
    float[] pos = new float[SIZE];
    float distance, targetPos, currentPos;
    private int targetIndex;

    bool b_Drag;

    void Start()
    {
        distance = 1f / (SIZE - 1);

        for (int i = 0; i < SIZE; i++)
            pos[i] = distance * i;
    }

    float setPos()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        }
        return 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentPos = setPos();
    }

    public void OnDrag(PointerEventData eventData)
    {
        b_Drag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        b_Drag = false;
        targetPos = setPos();

        // 절반 거리를 드레그 하지 않았을 경우
        if(currentPos == targetPos)
        {
            if(eventData.delta.x > 18 && currentPos - distance >= 0) // 왼쪽으로 드레그
            {
                --targetIndex;
                targetPos = currentPos - distance;
            }
            else if(eventData.delta.x < -18 && currentPos + distance <= 1.01f) // 오른쪽으로 드레그
            {
                ++targetIndex;
                targetPos = currentPos + distance;
            }
        }

        for(int i = 0; i< SIZE; i++)
        {
            if (contentTr.GetChild(i).GetComponent<VerticalScroll>() && currentPos != pos[i] && targetPos == pos[i])
            {
                contentTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
            }
        }

    }

    void Update()
    {
        TabSlider.value = scrollbar.value;

        if (b_Drag == false)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);

            for(int i = 0; i< SIZE; i++)
            {
                BtnRect[i].sizeDelta = new Vector2(i == targetIndex ? 360 : 180, BtnRect[i].sizeDelta.y);
            }

        }

        if (Time.time < 0.1f) return;

        for(int i = 0; i < SIZE; i ++)
        {
            Vector3 btnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 btntargetScale = Vector3.one;
            bool Textarea = false;

            if( i == targetIndex)
            {
                btnTargetPos.y = -23f;
                btntargetScale = new Vector3(1.2f, 1.2f, 1);
                Textarea = true;
;            }

            BtnImageRect[i].anchoredPosition3D = Vector3.Lerp(BtnImageRect[i].anchoredPosition3D, btnTargetPos, 0.25f);
            BtnImageRect[i].localScale = Vector3.Lerp(BtnImageRect[i].localScale, btntargetScale, 0.25f);
            BtnImageRect[i].transform.GetChild(0).gameObject.SetActive(Textarea);
        }

    }


    public void TabClick(int n)
    {
        targetIndex = n;
        targetPos = pos[n];
    }


}
