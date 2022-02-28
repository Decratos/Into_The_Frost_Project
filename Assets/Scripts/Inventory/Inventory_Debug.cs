using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Debug : MonoBehaviour
{
    
    [SerializeField] private List<ItemClass> inventoryList;
    [SerializeField] private List<string> startInventory;
    public Inventory inv;

    public void SetInventory(Inventory inventory) // set l'inventory
    {
        inv = inventory;
        CreateStartItem();
    }
    
    private void CreateStartItem() // ajoute les items � l'inventaire 
    {
        foreach(string name in startInventory)
        {
            InfoGlobalExel infoGlobal;
            liseurExel.LesDatas.FindObjectInfo(name, out infoGlobal);
            var startItem = new ItemClass {globalInfo = infoGlobal, amount = 1 };
            inv.CheckCapability(startItem);
        }
    }

    private void Update() //r�cup�re la liste
    {
        inventoryList = inv.GetItemList();
    }
}
