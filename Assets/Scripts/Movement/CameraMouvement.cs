using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MesFonctions
{

    //Public
    public GameObject joueur;

    //private
    [SerializeField] public Vector2 Sensibilite = Vector2.zero;
    [SerializeField] public Vector2 MaxAngle = Vector2.zero;
    GestionDesScipt ScriptGestion;

    float RotationSurX = 0;
    float RotationSurY = 0;

   


    void Start()
    {
        FindGestionDesScripts(joueur, out ScriptGestion);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }


    public void ReceptionDonnerInput(Vector2 Data) 
    {
         Data *= 0.4f;
         Data *= 0.1f;
         RotationSurX -= Data.y * Sensibilite.x ;
         RotationSurY += Data.x * Sensibilite.y ;
         RotationSurX = Mathf.Clamp(RotationSurX, -MaxAngle.x, MaxAngle.x);
        
        if (!ScriptGestion.LesMouvements.Jumping)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(RotationSurX, 0, 0));
            transform.parent.rotation = Quaternion.AngleAxis(RotationSurY, transform.parent.up);
        }
        else 
        {
            transform.localRotation = Quaternion.Euler(new Vector3(RotationSurX, RotationSurY, 0));
        }
        // voir si c'est déxaxé et corriger 

    }


}
