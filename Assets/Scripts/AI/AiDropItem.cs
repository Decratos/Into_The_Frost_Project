using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDropItem : MonoBehaviour
{
    [SerializeField] private int itemAmount;
    [SerializeField] private int amountToInstantiate;
    public void DropItem()
    {
        List<ItemClass> items = GetComponent<BanditInventory>().GetInventory();
        foreach (ItemClass item in items)
        {
            ItemWorld.DropItem(transform.position, item);
        }
    }
}
