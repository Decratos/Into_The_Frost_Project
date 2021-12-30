using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Container : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] private List<ItemClass> inventoryList;
    [SerializeField] protected ItemClass startInventory;
    bool isOpen = false;
    // Start is called before the first frame update
    void Awake()
    {
        inventory = new Inventory(); 
    }

    void Start()
    {
        inventory.AddItem(startInventory);
    }

    [Button("ResetInventory")]
    public void ResetInventory()
    {
        print("Reset l'inventaire du contenaire");
        inventory = new Inventory();
    }

    private void Update()
    {
        inventoryList = inventory.GetItemList();
    }

    public void OpenHideContainer()
    {
        var playerWindowUi = PlayerSingleton.playerInstance.GetComponent<InitiateInventory>().PlayerInventoryContainerWindow.GetComponent<UIInventory>();
        var ContainerWindowUi = PlayerSingleton.playerInstance.GetComponent<InitiateInventory>().ContainerWindow.GetComponent<UIInventory>();
        //PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = !isOpen;
        if(!isOpen)
        {
            ContainerWindowUi.SetInventory(inventory);
            ContainerWindowUi.OpenHideInventory(true);
            playerWindowUi.OpenHideInventory(true);
            isOpen = true;
        }
        else
        {
            ContainerWindowUi.OpenHideInventory(false);
            playerWindowUi.OpenHideInventory(false);
            isOpen = false;
        }
        
    }
}
