using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUIGestion : MonoBehaviour
{
    public Transform LastWindowOpened;
    public Transform contextWindow;
    public void CloseLastWindow()
    {
        if(LastWindowOpened)
        {
            if(LastWindowOpened.GetComponent<UIInventory>())
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
        if (open == "Open")
        {
            contextWindow.gameObject.SetActive(true);
        }
        else
        {
            contextWindow.gameObject.SetActive(false);
        }
    }

    public void CloseBasicUI()
    {
        gameObject.SetActive(false);
    }
}
