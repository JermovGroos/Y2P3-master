using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{
    public Item itemData;


    public void InitializeButton()
    {

    }
    public void Select()
    {
        print("SELECTED" + itemData.itemName);
    }
}
