using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Image img;
    public Text itemName;
    public Text level;
    public Text Context;
    public Button button;
    public Text Price;

    public static explicit operator Item(GameObject v)
    {
        throw new NotImplementedException();
    }
}
