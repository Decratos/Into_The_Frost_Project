using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Debug : MonoBehaviour
{
    
    [SerializeField] private List<ItemClass> inventoryList;
    [SerializeField] private List<ItemClass> startInventory;
    public Inventory inv;

    public void SetInventory(Inventory inventory) // set l'inventory
    {
        inv = inventory;
        CreateStartItem();
    }
    
    private void CreateStartItem() // ajoute les items à l'inventaire 
    {
        foreach(ItemClass item in startInventory)
        {
            inv.AddItem(item);
        }
    }

    private void Update() //récupére la liste
    {
        inventoryList = inv.GetItemList();
    }
}
