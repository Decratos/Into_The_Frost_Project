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

    public levels tableLevel;
    private bool isOpen = false;
    void Start()
    {
        
    }

    public void OpenHideTableWindow()
    {
        var playerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().PlayerInventoryContainerWindow.GetComponent<UIInventory>();
        if(!isOpen && !PlayerSingleton.playerInstance.GetComponent<InventoryManager>().CheckInventoryOpen())
        {
            CraftUI.instance.myTable = this;
            CraftUI.instance.OpenHideCraftUI((int)tableLevel);
            playerWindowUi.OpenHideInventory(true);
            PlayerSingleton.playerInstance.GetComponent<InventoryManager>().SetInventoryOpen(true);
            isOpen = true;
        }
        else
        {
            print("Je ferme la table de craft");
            CraftUI.instance.myTable = null;
            CraftUI.instance.OpenHideCraftUI((int)tableLevel);
            playerWindowUi.OpenHideInventory(false); 
            PlayerSingleton.playerInstance.GetComponent<InventoryManager>().SetInventoryOpen(false);
            isOpen = false;
        }
    }
    public void CloseTableWindow()
    {
        var playerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().PlayerInventoryContainerWindow.GetComponent<UIInventory>();
        print("Je ferme la table de craft");
        CraftUI.instance.myTable = null;
        CraftUI.instance.OpenHideCraftUI((int)tableLevel);
        playerWindowUi.OpenHideInventory(false);
        PlayerSingleton.playerInstance.GetComponent<InventoryManager>().SetInventoryOpen(false);
        isOpen = false;
    }
}
