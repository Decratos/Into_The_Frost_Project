using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    [SerializeField] private Transform cookingWindow;
    public bool isOpen = false;
    [SerializeField] private float fuel;
    private ItemSlot ComburantSlot;
    [SerializeField] private float maxFuel;

    // Start is called before the first frame update
    void Start()
    {
        cookingWindow = GameObject.Find("CookingWindow").transform;
        ComburantSlot = cookingWindow.transform.Find("Comburant").GetComponent<ItemSlot>();
        cookingWindow.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(ComburantSlot.equippedItemStat != null)
        {
            AddFuel(ComburantSlot.equippedItemStat.globalInfo.inflammability); 
        }
    }

    public void OpenHideWindow()
    {
        UIInventory UIi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().mainInventory;
        Transform bUI = UIi.BasicUI;
        isOpen = !isOpen;
        cookingWindow.gameObject.SetActive(isOpen);
        bUI.gameObject.SetActive(isOpen);
        bUI.GetComponent<BasicUIGestion>().contextWindow = cookingWindow;
        bUI.GetComponent<BasicUIGestion>().SetLastWindow(cookingWindow);
        PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = !isOpen;
        PlayerSingleton.playerInstance.GetComponent<InventoryManager>().SetInventoryOpen(isOpen);
        PlayerSingleton.playerInstance.GetComponent<CharacterController>().enabled = !isOpen;
        MouseCursorHiderShower.instance.ManageCursor(isOpen);
    }

    public void AddFuel(float fuelToAdd)
    {
        if(fuel < maxFuel && fuelToAdd > 0)
        {
            fuel += fuelToAdd;
            ComburantSlot.equippedItemStat = new ItemClass();
        }
        
    }

    public void AddFuel(int fuelToAdd)
    {
        if (fuel < maxFuel && fuelToAdd > 0)
        {
            fuel += fuelToAdd;
            ComburantSlot.equippedItemStat = new ItemClass();
        }
    }
}
