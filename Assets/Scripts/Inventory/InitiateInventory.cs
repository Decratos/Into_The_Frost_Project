using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiateInventory : MonoBehaviour
{
    public void Initiate()
    {
        GestionDesScipt gestion;
        Inventory inventory = GetComponent<GestionDesScipt>().Inventory = new Inventory();
        GetComponent<Inventory_Debug>().SetInventory(inventory);
        MesFonctions.FindGestionDesScripts(this.gameObject,out gestion);
        gestion.uiInventory.SetInventory(GetComponent<GestionDesScipt>().Inventory);
    }

    public void Start()
    {
    }

}
