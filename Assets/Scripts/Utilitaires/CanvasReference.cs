using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasReference : MonoBehaviour
{
    private Canvas playerCanva;
    public static CanvasReference _canvasReference;
    // Start is called before the first frame update
    void Awake()
    {
        if (_canvasReference != null)
        {
            Destroy(this);
        }
        else
        {
            _canvasReference = this;
        }
        
        if(GameObject.Find("PlayerCanva"))
            playerCanva = GameObject.Find("PlayerCanva").GetComponent<Canvas>();
        else
        {
            print("Le canva n'a pas été trouvé");
            playerCanva = null;
        }
    }

    public Canvas GetCanva()
    {
        if (playerCanva == null)
        {
            print("Le canva n'a pas été trouvé en amont");
        }
        return playerCanva;
    }
}
