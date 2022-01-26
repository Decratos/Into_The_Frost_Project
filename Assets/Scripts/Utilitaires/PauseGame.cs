using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    private bool paused = false;

    // Update is called once per frame
    void Update()
    {
        // Changer l'update par un simple void qui s'enclenche dans l'input system
        // je dois revoir les anciens doc
        print("Commentaire à résoudre");
        if (Input.GetKeyDown(KeyCode.F1))
        {
            paused = !paused;
            if (paused)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
