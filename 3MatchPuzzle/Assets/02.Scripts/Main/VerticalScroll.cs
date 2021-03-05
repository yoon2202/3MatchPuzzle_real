using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VerticalScroll : ScrollRect
{
    bool forParent;
    ScrollManager scrollManager;
    ScrollRect parent_ScrollRect;   

    protected override void Start()
    {
        scrollManager = FindObjectOfType<ScrollManager>();
        parent_ScrollRect = scrollManager.GetComponent<ScrollRect>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        forParent = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);

        if (forParent)
        {
            scrollManager.OnBeginDrag(eventData);
            parent_ScrollRect.OnBeginDrag(eventData);
        }
        else
        {
            base.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if(forParent)
        {
            scrollManager.OnDrag(eventData);
            parent_ScrollRect.OnDrag(eventData);
        }
        else
        {
            base.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (forParent)
        {
            scrollManager.OnEndDrag(eventData);
            parent_ScrollRect.OnEndDrag(eventData);
        }
        else
        {
            base.OnEndDrag(eventData);
        }
    }
}
