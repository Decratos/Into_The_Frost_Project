using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    public enum levels
    {
        level1 = 1,
        level2 = 2,
        level3 = 3,
        level4 = 4
    }

    [SerializeField] private levels tableLevel;
    private bool isOpen = false;
    void Start()
    {
        
    }

    public void OpenHideTableWindow()
    {
        var playerWindowUi = PlayerSingleton.playerInstance.GetComponent<InitiateInventory>().PlayerInventoryContainerWindow.GetComponent<UIInventory>();
        if(!isOpen)
        {
            CraftUI.instance.OpenHideCraftUI((int)tableLevel);
            playerWindowUi.OpenHideInventory(true);
            isOpen = true;
        }
        else
        {
            CraftUI.instance.OpenHideCraftUI((int)tableLevel);
            playerWindowUi.OpenHideInventory(false); 
            isOpen = false;
        }
    }
}
