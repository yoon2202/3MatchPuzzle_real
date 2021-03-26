using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mystic_Abstract : State ,IPointerDownHandler, IPointerUpHandler
{
    public GameObject MysticCrystal;

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (dotState != DotState.Possible)
            return;

        Destroy_Mystic();
    }

    public void Destroy_Mystic()
    {
        Instantiate(MysticCrystal, transform.position + Vector3.up * 0.2f, Quaternion.identity);
        Board.Destroy_DecreaseRow(transform);
    }

    //private void OnDestroy()
    //{
    //    Board.Instance.MysticDots[(int)transform.position.x, (int)transform.position.y] = null;
    //} 
}
