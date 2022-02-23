using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(ItemClass item, Vector3 position) //Spawn un item
    {
        Transform itemToInst = Instantiate(ItemAssets.ItemAssetsInstance.itemWorldPrefab, position, Quaternion.identity);
        itemToInst.name = item.globalInfo.Name;
        ItemWorld itemWorld = itemToInst.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
        return itemWorld;
    }
    
    [SerializeField] private ItemClass item;

    private void Awake()
    {
    }
    
    public void SetItem(ItemClass item)
    {
        this.item = item; 
    }

    public ItemClass GetItem() //récupére l'item
    {
        return item;
    }

    public void DestroySelf() // se détruit sois même
    {
        Destroy(gameObject);
    }

    public static ItemWorld DropItem(Vector3 DropPosition,ItemClass item) // met l'item dans le monde 
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        ItemWorld ItemWorld = SpawnItemWorld(item, DropPosition + (randomDir + Vector3.up) * 5f );
        ItemWorld.transform.name = item.globalInfo.Name;
        ItemWorld.item.amount = item.amount;
        ItemWorld.GetComponent<Rigidbody>().AddForce(randomDir * 1.5f, ForceMode.Impulse);
        return ItemWorld;
    }
    
}
