using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Debug : MonoBehaviour
{
    
    [SerializeField] private List<ItemClass> inventoryList;
    public Inventory inv;

    public void SetInventory(Inventory inventory)
    {
        inv = inventory;
        print("je créer l'inventaire");
    }
    
    private void Start() {
        InfoGlobalExel inf = new InfoGlobalExel();
        liseurExel.LesDatas.FindObjectInfo("Planche", out inf);
        inv.AddItem(new ItemClass{itemType = ResumeExelForObject.Type.ArmeMelee, amount = 1, spriteId = 1}, inf, inf.Name);
    }

    private void Update()
    {
        inventoryList = inv.GetItemList();
    }
}
