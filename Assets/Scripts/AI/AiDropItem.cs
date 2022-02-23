using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDropItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int itemAmount;
    [SerializeField] private int amountToInstantiate;
    [SerializeField] private ResumeExelForObject.Type itemType;
    public void DropItem()
    {
        for (int i = 0; i < amountToInstantiate; i++)
        {
            InfoGlobalExel globalExel = new InfoGlobalExel();
            liseurExel.LesDatas.FindObjectInfo(itemName, out globalExel);
            ItemWorld.DropItem(transform.position, new ItemClass{amount = itemAmount, globalInfo = globalExel});
        }
    }
}
