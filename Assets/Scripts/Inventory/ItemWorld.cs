using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class ItemWorld : MonoBehaviour
{
    public static ItemWorld SpawnItemWorld(ItemClass item, Vector3 position)
    {
        Transform itemToInst = Instantiate(ItemAssets.ItemAssetsInstance.itemWorldPrefab, position, Quaternion.identity);
        itemToInst.name = item.itemName;
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

    public ItemClass GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public static ItemWorld DropItem(Vector3 DropPosition,ItemClass item)
    {
        Vector3 randomDir = UtilsClass.GetRandomDir();
        ItemWorld ItemWorld = SpawnItemWorld(item, DropPosition + (randomDir + Vector3.up) * 5f );
        ItemWorld.transform.name = item.itemName;
        ItemWorld.item.amount = item.amount;
        ItemWorld.GetComponent<Rigidbody>().AddForce(randomDir * 1.5f, ForceMode.Impulse);
        return ItemWorld;
    }
    
}
