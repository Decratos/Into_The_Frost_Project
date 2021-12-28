using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] private List<ItemClass> inventoryList;
    // Start is called before the first frame update
    void Awake()
    {
        inventory = new Inventory(); 
    }

    private void Update()
    {
        inventoryList = inventory.GetItemList();
    }
}
