using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    public Transform cookingWindow;
    public bool isOpen = true;
    // Start is called before the first frame update
    void Start()
    {
        cookingWindow = GameObject.Find("CookingWindow").transform;
        OpenHideWindow();
    }

    public void OpenHideWindow()
    {
        isOpen = !isOpen;
        cookingWindow.gameObject.SetActive(isOpen);
        PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = !isOpen;
        PlayerSingleton.playerInstance.GetComponent<CharacterController>().enabled = !isOpen;
        MouseCursorHiderShower.instance.ManageCursor(isOpen);
    }
}
