using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInvetaire : MonoBehaviour
{

    private Inventory inv;
    public List<ItemClass> inventoryDebug;
    // Start is called before the first frame update
    void Start()
    {
        inv = new Inventory();
    }

    // Update is called once per frame
    void Update()
    {
        inventoryDebug = inv.GetItemList();
    }
}
