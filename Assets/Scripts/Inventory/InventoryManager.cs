using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform ContainerWindow;
    public Transform PlayerInventoryContainerWindow;
    private bool hasInventoryOpen = false;
    public UIInventory mainInventory;
    public void Initiate()
    {
        GestionDesScipt gestion;
        Inventory inventory = GetComponent<GestionDesScipt>().Inventory = new Inventory();
        GetComponent<Inventory_Debug>().SetInventory(inventory);
        MesFonctions.FindGestionDesScripts(this.gameObject,out gestion);
        gestion.uiInventory.SetInventory(GetComponent<GestionDesScipt>().Inventory);
    }

    public bool CheckInventoryOpen()
    {
        return hasInventoryOpen;
    }
    public void SetInventoryOpen(bool newBool)
    {
        hasInventoryOpen = newBool;
    }

}
