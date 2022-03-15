using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    [SerializeField] private Transform cookingWindow;
    [SerializeField] private float fuel;
    [SerializeField]  private ItemSlot ComburantSlot;
    [SerializeField]  private ItemSlot ToCookSlot;
    [SerializeField]  private ItemSlot ResultSlot;
    [SerializeField] private float maxFuel;
    [SerializeField] private float cookingTime;
    private bool hasAnObjectToCook = false;
    private Transform playerInventory;

    // Start is called before the first frame update
    void Start()
    {
        cookingWindow = GameObject.Find("CookingWindow").transform;
        ToCookSlot = cookingWindow.transform.GetChild(1).Find("ToCook").GetComponent<ItemSlot>();
        ComburantSlot = cookingWindow.transform.GetChild(1).Find("Comburant").GetComponent<ItemSlot>();
        ResultSlot = cookingWindow.transform.GetChild(1).Find("Result").GetComponent<ItemSlot>();
        playerInventory = cookingWindow.transform.Find("PlayerInventoryWithCooker").transform;
        cookingWindow.gameObject.SetActive(false);
        ToCookSlot.equippedItemStat = null;
        ComburantSlot.equippedItemStat = null;
        ResultSlot.equippedItemStat = null;
    }

    private void Update()
    {
        if(ToCookSlot.equippedItemStat.amount != 0 && !hasAnObjectToCook)
        {
            SetCookObject();
            hasAnObjectToCook = true;
        }
        else
        {
            //print("Je cherche un objet ‡ cuire");
        }
        if(ComburantSlot.equippedItemStat != null && ToCookSlot.equippedItemStat != null && resultItem.amount != 0)
        {
            AddFuel(ComburantSlot.equippedItemStat.globalInfo.inflammability);
            
        }
        if(fuel > 0 && ToCookSlot.equippedItemStat.amount != 0)
        {
            print("Je commence la cuisson");
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
                ComburantSlot.GetComponentInChildren<UnityEngine.UI.Image>().sprite = null;
            }
        }
    }

    public void SetCookObject()
    {
        cookingTime = ToCookSlot.equippedItemStat.globalInfo.TempsCuisson;
    }

    private void Cook()
    {
        cookingTime -= 1 * Time.deltaTime;
        fuel -= 1 * Time.deltaTime;
        if(cookingTime <= 0)
        {
<<<<<<< HEAD
            ResultSlot.equippedItemStat = resultItem;
            ResultSlot.GetComponentInChildren<UnityEngine.UI.Image>().sprite = ResultSlot.equippedItemStat.GetSprite();
            ToCookSlot.equippedItemStat = new ItemClass();
            ToCookSlot.GetComponentInChildren<UnityEngine.UI.Image>().sprite = null;
=======
            //L'objet est cuit
>>>>>>> parent of 47e3587 (Merge branch 'Th√©o')
            hasAnObjectToCook = false;
        }
    }
}
