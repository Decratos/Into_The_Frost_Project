using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform ContainerWindow;
    public Transform PlayerInventoryContainerWindow;
    [SerializeField] private bool hasInventoryOpen = false;
    public UIInventory mainInventory;
    public int startingInventoryCapability;
    public void Initiate()
    {
        GestionDesScipt gestion;
        Inventory inventory = GetComponent<GestionDesScipt>().Inventory = new Inventory();
        inventory.capability = startingInventoryCapability;
        GetComponent<Inventory_Debug>().SetInventory(inventory);
        MesFonctions.FindGestionDesScripts(this.gameObject,out gestion);
        gestion.uiInventory.SetInventory(GetComponent<GestionDesScipt>().Inventory);
        ContainerWindow = CanvasReference._canvasReference.GetCanva().transform.Find("ContainerInventory");
        PlayerInventoryContainerWindow = ContainerWindow.transform.Find("PlayerInventoryWithContainer");

    }

    public bool CheckInventoryOpen()// permet de savoir si l'inventaire est ouvert
    {
        return hasInventoryOpen;
    }
    public void SetInventoryOpen(bool newBool) // set l'inventaire en open/non
    {
        hasInventoryOpen = newBool;
    }

}
