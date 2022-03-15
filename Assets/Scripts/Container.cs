using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Container : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] private List<ItemClass> inventoryList;
    [SerializeField] protected ItemClass startInventory;
    [SerializeField] private bool isOpen = false;
    // Start is called before the first frame update
    void Awake()
    {
        inventory = new Inventory(); 
    }

    void Start()
    {
        inventory.CheckCapability(startInventory);
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

    public void OpenContainer()
    {
        var playerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().PlayerInventoryContainerWindow.GetComponent<UIInventory>();
        var ContainerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().ContainerWindow.GetComponent<UIInventory>();
        //PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = !isOpen;
        print("jouvre la fenetre");
        ContainerWindowUi.BasicUI.GetComponent<BasicUIGestion>().contextWindow = ContainerWindowUi.transform;
        ContainerWindowUi.BasicUI.GetComponent<BasicUIGestion>().LastWindowOpened = ContainerWindowUi.transform;
        ContainerWindowUi.BasicUI.GetComponent<BasicUIGestion>().contextObject = this.transform;
        ContainerWindowUi.SetInventory(inventory);
        ContainerWindowUi.gameObject.SetActive(true);
        ContainerWindowUi.OpenHideInventory("Open", false);
        playerWindowUi.OpenHideInventory("Open", false);
        
    }

    public void CloseContainer()
    {
        var playerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().PlayerInventoryContainerWindow.GetComponent<UIInventory>();
        var ContainerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().ContainerWindow.GetComponent<UIInventory>();
        print("je ferme la fenetre");
        ContainerWindowUi.OpenHideInventory("Close", false);
        playerWindowUi.OpenHideInventory("Close", false);
        isOpen = false;
    }
}
