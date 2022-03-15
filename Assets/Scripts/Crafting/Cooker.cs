using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    [SerializeField] private Transform cookingWindow;
    [SerializeField] private float fuel;
    private ItemSlot ComburantSlot;
    private ItemSlot ToCookSlot;
    private ItemSlot ResultSlot;
    [SerializeField] private float maxFuel;
    private float cookingTime;
    private bool hasAnObjectToCook = false;
    private Transform playerInventory;
    public ItemClass resultItem;

    // Start is called before the first frame update
    void Start()
    {
        cookingWindow = GameObject.Find("CookingWindow").transform;
        ToCookSlot = cookingWindow.transform.Find("ToCook").GetComponent<ItemSlot>();
        ComburantSlot = cookingWindow.transform.Find("Comburant").GetComponent<ItemSlot>();
        ResultSlot = cookingWindow.transform.Find("Result").GetComponent<ItemSlot>();
        playerInventory = cookingWindow.transform.Find("PlayerInventoryWithCooker").transform;
        cookingWindow.gameObject.SetActive(false);
        ToCookSlot.equippedItemStat = null;
        ComburantSlot.equippedItemStat = null;
        ResultSlot.equippedItemStat = null;
    }

    private void Update()
    {
        if(ToCookSlot.equippedItemStat != null && !hasAnObjectToCook)
        {
            SetCookObject();
            hasAnObjectToCook = true;
        }
        else
        {
            //print("Je cherche un objet à cuire");
        }
        if(ComburantSlot.equippedItemStat != null && ToCookSlot.equippedItemStat != null && ResultSlot.equippedItemStat != null)
        {
            AddFuel(ComburantSlot.equippedItemStat.globalInfo.inflammability);
            Cook();
        }
    }

    public void OpenWindow()
    {
        UIInventory UIi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().mainInventory;
        Transform bUI = UIi.BasicUI;
        cookingWindow.gameObject.SetActive(true);
        playerInventory.GetComponent<UIInventory>().OpenHideInventory(true);
        bUI.GetComponent<BasicUIGestion>().contextWindow = cookingWindow;
        bUI.GetComponent<BasicUIGestion>().SetLastWindow(cookingWindow);
        /*PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = !isOpen;
        PlayerSingleton.playerInstance.GetComponent<InventoryManager>().SetInventoryOpen(isOpen);
        PlayerSingleton.playerInstance.GetComponent<CharacterController>().enabled = !isOpen;
        MouseCursorHiderShower.instance.ManageCursor(isOpen);*/
    }

    public void AddFuel(float fuelToAdd)
    {
        if(fuel < maxFuel && fuelToAdd > 0)
        {
            fuel += fuelToAdd;
            fuel = Mathf.Clamp(fuel, 0, maxFuel);
            if(ComburantSlot.equippedItemStat.amount > 1)
            {
                ComburantSlot.equippedItemStat.amount--;
            }
            else
            {
                ComburantSlot.equippedItemStat = null;
            }
            
        }
        
    }

    public void AddFuel(int fuelToAdd)
    {
        if (fuel < maxFuel && fuelToAdd > 0)
        {
            fuel += fuelToAdd;
            fuel = Mathf.Clamp(fuel, 0, maxFuel);
            if (ComburantSlot.equippedItemStat.amount > 1)
            {
                ComburantSlot.equippedItemStat.amount--;
            }
            else
            {
                ComburantSlot.equippedItemStat = null;
            }
        }
    }

    public void SetCookObject()
    {
        cookingTime = ToCookSlot.equippedItemStat.globalInfo.TempsCuisson;
    }

    private void Cook()
    {
        cookingTime--;
        fuel--;
        if(cookingTime <= 0)
        {
            ResultSlot.equippedItemStat = resultItem;
            hasAnObjectToCook = false;
        }
    }
}
