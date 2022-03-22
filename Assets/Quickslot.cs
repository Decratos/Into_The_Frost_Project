using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quickslot : MonoBehaviour
{
    public ItemClass item;

    public void AssignItem(ItemClass newItem)
    {
        item = newItem;
        GetComponent<UnityEngine.UI.Image>().sprite = newItem.GetSprite();
    }
}
