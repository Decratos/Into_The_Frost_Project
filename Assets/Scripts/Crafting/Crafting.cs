using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting
{
    private List<Craft> craftsList;
    public Crafting()
    {
        craftsList = new List<Craft>();
    }

    public void CraftItem(Craft craft, liseurExel Excel, GestionDesScipt gestion, InfoGlobalExel global) // Craft l'item 
    {
        bool canCraft = true;
        
        for (int i = 0; i < craft.infos.LeNombreNecessaire.Length; i++)
        {
            if(gestion.Inventory.CheckItemOnList(new ItemClass {globalInfo = global}) >= craft.infos.LeNombreNecessaire[i])
            {
                continue;
            }
            else
            {
                Debug.Log("Je n'ai pas les matériaux");
                canCraft = false;
                break;
            }
        }
        if(canCraft)
        {

            gestion.Inventory.CheckCapability(new ItemClass{globalInfo = global, amount = 1});
            for (int i = 0; i < craft.infos.LeNombreNecessaire.Length; i++)
            {
               gestion.Inventory.RemoveItem(new ItemClass {globalInfo = global}, craft.infos, i); 
            }   
            
        }
        
    }
}
