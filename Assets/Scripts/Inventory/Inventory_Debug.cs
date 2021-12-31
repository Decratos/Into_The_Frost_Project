using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Debug : MonoBehaviour
{
    
    [SerializeField] private List<ItemClass> inventoryList;
    [SerializeField] private List<ItemClass> startInventory;
    public Inventory inv;

    public void SetInventory(Inventory inventory)
    {
        inv = inventory;
        CreateStartItem();
    }
    
    private void CreateStartItem() {
        foreach(ItemClass item in startInventory)
        {
            inv.AddItem(item);
        }
    }

    private void Update()
    {
        inventoryList = inv.GetItemList();
    }
}
