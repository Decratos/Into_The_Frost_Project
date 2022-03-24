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

    public void UseSlot()
    {
        if(item.amount > 0)
        {
            PlayerSingleton.playerInstance.GetComponent<Inventory_Debug>().inv.UseItem(item);
            if (item.amount > 0)
            {
                GetComponent<UnityEngine.UI.Image>().sprite = item.GetSprite();
            }
            else
            {
                GetComponent<UnityEngine.UI.Image>().sprite = null;
            }
        }
       
    }
}
