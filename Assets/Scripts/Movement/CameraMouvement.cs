using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MesFonctions
{

    //Public
    public GameObject joueur;
    public Vector2 Sensibilite = Vector2.zero;
    public Vector2 MaxAngle = Vector2.zero;
    public bool canLook;
    //public Vector2 MaxAngleInJump = Vector2.zero;


    //private

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
        if (ScriptGestion.LesMouvements.StateDeDeplacement != MouvementPlayer.StateDeplacement.Atteri)
        {
            RotationSurX -= Data.y * Sensibilite.x;
            RotationSurY += Data.x * Sensibilite.y;
            RotationSurX = Mathf.Clamp(RotationSurX, -MaxAngle.x, MaxAngle.x);
            if (ScriptGestion.LesMouvements.StateDeDeplacement != MouvementPlayer.StateDeplacement.Saute && ScriptGestion.LesMouvements.StateDeDeplacement != MouvementPlayer.StateDeplacement.Fall)
            {
                transform.parent.rotation = Quaternion.AngleAxis(RotationSurY, transform.parent.up);
                transform.localRotation = Quaternion.Euler(new Vector3(RotationSurX, 0, 0));
            }
            else //lors du saut
            {
                transform.localRotation = Quaternion.Euler(new Vector3(RotationSurX, transform.localEulerAngles.y, 0));
            }
        }
        

    }

    public void resetAtLanding ()
    {
        Quaternion cameraRotat = transform.rotation;

        transform.parent.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
        transform.localRotation = Quaternion.Euler(cameraRotat.eulerAngles.x, 0, 0);

    }
}
