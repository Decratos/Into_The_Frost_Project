using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Debug : MonoBehaviour
{
    
    [SerializeField] private List<ItemClass> inventoryList;
    [SerializeField] private List<string> startInventory;
    [SerializeField] private List<string> startEquipement;
    public Inventory inv;
    public int playerInventoryCapacity;

    public void SetInventory(Inventory inventory) // set l'inventory
    {
        inv = inventory;
        CreateStartItem();
    }
    
    private void CreateStartItem() // ajoute les items ? l'inventaire 
    {
        foreach(string name in startInventory)
        {
            InfoGlobalExel infoGlobal;
            liseurExel.LesDatas.FindObjectInfo(name, out infoGlobal);
            var startItem = new ItemClass {globalInfo = infoGlobal, amount = 1 };
            inv.CheckCapability(startItem);
        }

        foreach(string name in startEquipement)
        {
            InfoGlobalExel infoGlobal;
            liseurExel.LesDatas.FindObjectInfo(name, out infoGlobal);
            var startItem = new ItemClass { globalInfo = infoGlobal, amount = 1 };
            print(startItem.globalInfo.Name + " " + startItem.globalInfo.TypeGeneral);
            CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Add(startItem);
        }
    }

    private void Update() //r?cup?re la liste
    {
        inventoryList = inv.GetItemList();
        playerInventoryCapacity = inv.capability;
    }
}
