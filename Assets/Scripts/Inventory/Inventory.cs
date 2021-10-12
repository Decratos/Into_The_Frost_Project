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

    public void AddItem(ItemClass item, InfoGlobalExel infos, string name)
    {
        if (item.isStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (ItemClass inventoryItem in itemList)
            {
                if (inventoryItem.itemName == name)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
                
            }

            if (!itemAlreadyInInventory)
            {
                
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);  
        }
        item.itemName += name;
        item.globalInfo = infos;
        OnItemListChanged?.Invoke(this,EventArgs.Empty);
    }

    public void RemoveItem(ItemClass item)
    {
        if (item.isStackable())
        {
            ItemClass itemAlreadyInInventory = null;
            foreach (ItemClass inventoryItem in itemList)
            {
            
                if (inventoryItem.itemName == item.itemName)
                {
                    inventoryItem.amount -= item.amount;
                    itemAlreadyInInventory = inventoryItem;
                    Debug.Log("Nombre d'item à enlever " + item.amount);
                }
                
            }

            if (itemAlreadyInInventory != null && itemAlreadyInInventory.amount <= 0)
            {
                itemList.Remove(itemAlreadyInInventory);
            }
        }
        else
        {
            itemList.Remove(item);  
        }
        OnItemListChanged?.Invoke(this,EventArgs.Empty);
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
