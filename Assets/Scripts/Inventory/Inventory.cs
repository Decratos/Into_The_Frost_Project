using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory
{
    public event EventHandler OnItemListChanged;
    private List<ItemClass> itemList;
    public int capability;

    public Inventory()
    {
        itemList = new List<ItemClass>();
    }

    public void AddItem(ItemClass item) //Ajoute un item � l'inventaire
    {
        ItemClass newItem = new ItemClass();
        InfoGlobalExel inf = new InfoGlobalExel();
        liseurExel.LesDatas.FindObjectInfo(item.globalInfo.Name, out inf);
        newItem = item;
        /*newItem.itemType = item.itemType;
        newItem.amount = item.amount;
        newItem.spriteId = item.spriteId;
        newItem.durability = item.durability;
        newItem.inflamability = item.inflamability;*/
        if (item.isStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (ItemClass inventoryItem in itemList)
            {
                if (inventoryItem.globalInfo.Name == inf.Name)
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
        newItem.globalInfo.Name = inf.Name;
        newItem.globalInfo = inf;
        newItem.name = inf.Name;
        OnItemListChanged?.Invoke(this,EventArgs.Empty);
    }

    public void CheckCapability(ItemClass itemToPass)
    {
        if(itemList.Count < capability)
        {
            AddItem(itemToPass);
        }
        else
        {
            ItemWorld.SpawnItemWorld(itemToPass, PlayerSingleton.playerInstance.transform.position);
            Debug.Log("No more space in inventory");
        }
    }

    public void CheckCapability(ItemClass itemToPass, GameObject obj)
    {
        if (itemList.Count < capability)
        {
            AddItem(itemToPass);
            GameObject.Destroy(obj.transform.gameObject);
        }
    }
    public void RemoveItem(ItemClass item)// Supprime un item de l'inventaire
    {
        Debug.Log(item.name);
        ItemClass newItem = new ItemClass();
        InfoGlobalExel inf = new InfoGlobalExel();
        liseurExel.LesDatas.FindObjectInfo(item.name, out inf);
        newItem.amount = item.amount;
        newItem.globalInfo.ID = item.globalInfo.ID;
        if (item.isStackable())
        {
            ItemClass itemAlreadyInInventory = null;
            foreach (ItemClass inventoryItem in itemList)
            {
                if (inventoryItem.globalInfo.Name == item.globalInfo.Name)
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
            itemList.Remove(item);  
        }
        OnItemListChanged?.Invoke(this,EventArgs.Empty);
    }
    public void RemoveItem(ItemClass item, InfoExelCraft infoCraft, int step)
    {
        ItemClass newItem = new ItemClass();
        InfoGlobalExel inf = new InfoGlobalExel();
        liseurExel.LesDatas.FindObjectInfo(item.globalInfo.Name, out inf);
        newItem.amount = infoCraft.LeNombreNecessaire[step];
        newItem.globalInfo.ID = item.globalInfo.ID;
        if (item.isStackable())
        {
            ItemClass itemAlreadyInInventory = null;
            foreach (ItemClass inventoryItem in itemList)
            {
                if (inventoryItem.globalInfo.Name == infoCraft.NomdesRessourcesNecessaire[step])
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
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void TransferItem(Inventory newInventory, ItemClass item)// Transfert un item
    {
        newInventory.AddItem(item);
        this.RemoveItem(item);
    }

    public List<ItemClass> GetItemList() //permet de r�cupp�rer la liste
    {
        return itemList;
    }

    public void UseItem(ItemClass item) //
    {
        switch (item.globalInfo.TypeGeneral)
        {
            case InfoGlobalExel.Type.Nourriture:
            case InfoGlobalExel.Type.Soins:
                ItemsActions.itemsActionsInstance.ItemAction(item);
                if (item.amount > 1)
                    item.amount--;
                else
                    RemoveItem(item);
                break;
            default:
                Debug.LogError("Je n'ai pas p� utiliser l'objet, cela vient s�rement du fait qu'il ne s'agit pas d'un consommable");
                break;
        }
        
    }

    public int CheckItemOnList(ItemClass item)// v�rifie le nombre existant
    {
        int amount = 0;
        foreach (var items in itemList)
        {
            if (items.globalInfo.Name == item.globalInfo.Name)
            {
                amount = items.amount;
            }
        }

        return amount;
    }
}
