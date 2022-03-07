using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUIGestion : MonoBehaviour
{
    public Transform LastWindowOpened;
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
        }
            
    }

    public void SetLastWindow(Transform window)
    {
        LastWindowOpened = window;
    }
}
