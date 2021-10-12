using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{

    public ItemClass item;
    
    // Start is called before the first frame update
    void Start()
    {
        var myItem = ItemWorld.SpawnItemWorld(item, transform.position);
        myItem.transform.parent = transform.parent;
        Destroy(this);
    }
    
}
