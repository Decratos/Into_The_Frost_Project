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
        var playerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().PlayerInventoryContainerWindow.GetComponent<UIInventory>();
        if(!isOpen && !PlayerSingleton.playerInstance.GetComponent<InventoryManager>().CheckInventoryOpen())
        {
            CraftUI.instance.OpenHideCraftUI((int)tableLevel);
            playerWindowUi.OpenHideInventory(true);
            PlayerSingleton.playerInstance.GetComponent<InventoryManager>().SetInventoryOpen(true);
            isOpen = true;
        }
        else
        {
            CraftUI.instance.OpenHideCraftUI((int)tableLevel);
            playerWindowUi.OpenHideInventory(false); 
            PlayerSingleton.playerInstance.GetComponent<InventoryManager>().SetInventoryOpen(false);
            isOpen = false;
        }
    }
}
