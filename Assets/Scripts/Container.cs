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
        inventory.CheckCapability(startInventory); //ajoute à l'inventaire
    }

    [Button("ResetInventory")]
    public void ResetInventory() // reset l'inventaire
    {
        print("Reset l'inventaire du contenaire");
        inventory = new Inventory();
    }

    private void Update()
    {
        inventoryList = inventory.GetItemList();// set l'inventaire dans ce script
    }

    public void OpenContainer()// ouvre la fenêtre du container
    {
        //recup les varibles pour les avoir ceq 'uil faut
        var playerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().PlayerInventoryContainerWindow.GetComponent<UIInventory>();
        var ContainerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().ContainerWindow.GetComponent<UIInventory>();
        //PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = !isOpen;
        print("jouvre la fenetre");
        //set les transforms
        ContainerWindowUi.BasicUI.GetComponent<BasicUIGestion>().contextWindow = ContainerWindowUi.transform;
        ContainerWindowUi.BasicUI.GetComponent<BasicUIGestion>().LastWindowOpened = ContainerWindowUi.transform;
        ContainerWindowUi.BasicUI.GetComponent<BasicUIGestion>().contextObject = this.transform;
        ContainerWindowUi.SetInventory(inventory); // set l'inventaire
        ContainerWindowUi.gameObject.SetActive(true); // active l'objet
        ContainerWindowUi.OpenHideInventory("Open", false);
        playerWindowUi.OpenHideInventory("Open", false);
        
    }

    public void CloseContainer() // ferme le container
    {
        var playerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().PlayerInventoryContainerWindow.GetComponent<UIInventory>();
        var ContainerWindowUi = PlayerSingleton.playerInstance.GetComponent<InventoryManager>().ContainerWindow.GetComponent<UIInventory>();
        print("je ferme la fenetre");
        ContainerWindowUi.OpenHideInventory("Close", false);
        playerWindowUi.OpenHideInventory("Close", false);
        isOpen = false;
    }
}
