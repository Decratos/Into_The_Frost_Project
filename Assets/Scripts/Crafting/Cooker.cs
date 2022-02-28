using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    [SerializeField] private Transform cookingWindow;
    public bool isOpen = true;
    [SerializeField] private float fuel;
    private ItemSlot ComburantSlot;
    [SerializeField] private float maxFuel;

    // Start is called before the first frame update
    void Start()
    {
        cookingWindow = GameObject.Find("CookingWindow").transform;
        ComburantSlot = cookingWindow.transform.Find("Comburant").GetComponent<ItemSlot>();
        OpenHideWindow();
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
        isOpen = !isOpen;
        cookingWindow.gameObject.SetActive(isOpen);
        PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = !isOpen;
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
