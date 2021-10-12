using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorHiderShower : MonoBehaviour
{

    public static MouseCursorHiderShower instance;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void ManageCursor(bool state)
    {
        switch (state)
        {
            case true:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            break;
            case false:
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            break;
            default:
        }
        
    }
}
