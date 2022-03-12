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
        if(LastWindowOpened)
        {
            print("Je ferme la dernière fenêtre");
            if (LastWindowOpened.GetComponent<UIInventory>() && LastWindowOpened.transform.name != "ContainerInventory")
            {
                LastWindowOpened.GetComponent<UIInventory>().OpenHideInventory("Close");
            }
            else if(LastWindowOpened.GetComponent<CraftUI>())
            {
                LastWindowOpened.GetComponent<CraftUI>().OpenHideCraftUI(false, 0);
            }
            else if(LastWindowOpened.transform.name == "CookingWindow")
            {
                OpenCloseContextWindow("Close");
            }
            else if (LastWindowOpened.transform.name == "ContainerInventory")
            {
                print("Je ferme le container");
                OpenCloseContextWindow("Close");
            }
            else
            {
                print("Je n'ai pas trouvé la fenêtre correspondante : " + LastWindowOpened.transform.name);
            }
            if(playerInput)
            {
                LastWindowOpened = null;
                contextWindow = null;
                contextObject = null;
            }
            
        }
            
    }

    public void SetLastWindow(Transform window)
    {
        LastWindowOpened = window;
    }
    public void SetLastWindow(bool isContextWindow)
    {
        if(isContextWindow)
            LastWindowOpened = contextWindow;
    }
    public void OpenCloseContextWindow(string open)
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

    public void CloseBasicUI()
    {
        gameObject.SetActive(false);
    }
}
