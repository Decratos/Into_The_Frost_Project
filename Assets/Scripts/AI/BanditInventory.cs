using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditInventory : MonoBehaviour
{
    [SerializeField] private List<ItemClass> inventory;
    public void AddInventoryItem(string itemName)
    {
        InfoGlobalExel infoGlobalItem;
        liseurExel.LesDatas.FindObjectInfo(itemName, out infoGlobalItem);
        inventory.Add(new ItemClass {globalInfo = infoGlobalItem, amount = 1, name = itemName });
    }

    public List<ItemClass> GetInventory()
    {
        return inventory;
    }
}
