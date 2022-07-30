using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUIGestion : MonoBehaviour
{
    public Transform LastWindowOpened;
    public Transform contextWindow;
    public Transform contextObject;
    public void CloseLastWindow(bool playerInput)
    {
        if(LastWindowOpened)//si la derni�re fen�tre est ouverte
        {
            print("Je ferme la derni�re fen�tre");
            if (LastWindowOpened.GetComponent<UIInventory>() && LastWindowOpened.transform.name != "ContainerInventory")
            {
                LastWindowOpened.GetComponent<UIInventory>().OpenHideInventory("Close");// ferme l'inventaire
            }
            else if(LastWindowOpened.GetComponent<CraftUI>())
            {
                LastWindowOpened.GetComponent<CraftUI>().OpenHideCraftUI(false, 0);// ferme la fen�tre de craft
            }
            else if(LastWindowOpened.transform.name == "CookingWindow")
            {
                OpenCloseContextWindow("Close");//ferme la fen�tre contextuelle
            }
            else if (LastWindowOpened.transform.name == "ContainerInventory")
            {
                print("Je ferme le container");
                OpenCloseContextWindow("Close");//ferme la fen�tre contextuelle
            }
            else
            {
                print("Je n'ai pas trouv� la fen�tre correspondante : " + LastWindowOpened.transform.name);
            }
            if(playerInput) // reset les valeurs
            {
                LastWindowOpened = null;
                contextWindow = null;
                contextObject = null;
            }
            
        }
            
    }

    public void SetLastWindow(Transform window)// set la last window avec un transform
    {
        LastWindowOpened = window;
    }
    public void SetLastWindow(bool isContextWindow)
    {
        if(isContextWindow)
            LastWindowOpened = contextWindow;
    }
    public void OpenCloseContextWindow(string open) // ouvre ou ferme la fenetre contextuel
    {
        if(contextWindow)
        {
            if (open == "Open")
            {
                contextWindow.gameObject.SetActive(true);
            }
            else
            {
                contextWindow.gameObject.SetActive(false);
            }
        }
        
    }

    public void CloseBasicUI() // ferme se gameobject
    {
        gameObject.SetActive(false);
    }
}
