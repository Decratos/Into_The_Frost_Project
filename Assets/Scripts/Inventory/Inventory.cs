using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<ItemClass> itemList;
    

    public Inventory()
    {
        itemList = new List<ItemClass>();
    }

    public void AddItem(ItemClass item)
    {
        ItemClass newItem = new ItemClass();
        Debug.Log("J'ajoute l'item'");
        InfoGlobalExel inf = new InfoGlobalExel();
        liseurExel.LesDatas.FindObjectInfo(item.itemName, out inf);
        newItem.amount = item.amount;
        newItem.spriteId = item.spriteId;
        if (item.isStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (ItemClass inventoryItem in itemList)
            {
                if (inventoryItem.itemName == inf.Name)
                {
                    inventoryItem.amount += newItem.amount;
                    itemAlreadyInInventory = true;
                }
                
            }

            if (!itemAlreadyInInventory)
            {
                
                itemList.Add(newItem);
            }
        }
        else
        {
            itemList.Add(newItem);  
        }
        newItem.itemName = inf.Name;
        newItem.globalInfo = inf;
        OnItemListChanged?.Invoke(this,EventArgs.Empty);
    }

    public void RemoveItem(ItemClass item)
    {
        ItemClass newItem = new ItemClass();
        InfoGlobalExel inf = new InfoGlobalExel();
        liseurExel.LesDatas.FindObjectInfo(item.itemName, out inf);
        newItem.amount = item.amount;
        newItem.spriteId = item.spriteId;

        Debug.Log("Je supprime l'item");
        if (item.isStackable())
        {
            ItemClass itemAlreadyInInventory = null;
            foreach (ItemClass inventoryItem in itemList)
            {
                if (inventoryItem.itemName == item.itemName)
                {
                    inventoryItem.amount -= newItem.amount;
                    itemAlreadyInInventory = inventoryItem;
                }
                
            }

            if (itemAlreadyInInventory != null && itemAlreadyInInventory.amount <= 0)
            {
                itemList.Remove(itemAlreadyInInventory);
            }
        }
        else
        {
            itemList.Remove(newItem);  
        }
        OnItemListChanged?.Invoke(this,EventArgs.Empty);
    }

    public void TransferItem(Inventory newInventory, ItemClass item)
    {
        newInventory.AddItem(item);
        this.RemoveItem(item);
    }

    public List<ItemClass> GetItemList()
    {
        return itemList;
    }

    public void UseItem(ItemClass item)
    {
        ItemsActions.itemsActionsInstance.ItemAction(item.itemName, item);
    }

    public int CheckItemOnList(ItemClass item)
    {
        int amount = 0;
        foreach (var items in itemList)
        {
            if (items.itemName == item.itemName)
            {
                amount = items.amount;
            }
        }

        return amount;
    }
}
