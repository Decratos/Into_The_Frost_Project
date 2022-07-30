using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MesFonctions
{

    //Public
    public GameObject joueur;
    
    public Vector2 MaxAngle = Vector2.zero;
    
    public bool canLook;
    //public Vector2 MaxAngleInJump = Vector2.zero;


    //private
    Vector2 ManetteDirection;
    bool ManetteActivated;
    GestionDesScipt ScriptGestion;

    float RotationSurX = 0;
    float RotationSurY = 0;

    bool horsSol=false;
    Vector3 PositionLorsSaut;
    Vector2 Sensibilite = Vector2.zero;


    void Start()
    {
        FindGestionDesScripts(joueur, out ScriptGestion);
        PositionLorsSaut = transform.localPosition;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Sensibilite = ScriptGestion.InputGestion.SensibiliteSouris;
    }

    void Update()
    {

        if (ManetteActivated)
        {
            ReceptionDonnerInput(ManetteDirection);
        }
        if (horsSol)
        {
            transform.position = joueur.transform.position + PositionLorsSaut;
        }

    }

    public void ReceptionDataManette(Vector2 Data) 
    {

        ManetteActivated = true;
        if (Data != Vector2.zero)
        {
            Sensibilite = ScriptGestion.InputGestion.SensibiliteManette;
            ManetteDirection = Data.normalized;
        }
        else
        {
            Sensibilite = ScriptGestion.InputGestion.SensibiliteSouris;
            ManetteActivated = false;
            ManetteDirection = Vector2.zero;
        }
        
    }


    public void ReceptionDonnerInput(Vector2 Data)
    {
        if (canLook)
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
                    transform.localRotation = Quaternion.Euler(new Vector3(RotationSurX, 0, 0));
                    transform.parent.rotation = Quaternion.AngleAxis(RotationSurY, transform.parent.up);
                    
                }
                else //lors du saut
                {
                    transform.rotation = Quaternion.Euler(new Vector3(RotationSurX, RotationSurY, 0));
                }
            }
        }
        
        

    }

    public void resetAtLanding()
    {
        horsSol = false;
        joueur.transform.rotation = Quaternion.Euler(joueur.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, joueur.transform.rotation.eulerAngles.z);
        transform.parent = joueur.transform;
        
    }

    public void detachCam() 
    {
        transform.parent = null;
        horsSol = true;
    
    }
}
/*  Quaternion cameraRotat = transform.rotation;

        transform.parent.rotation = Quaternion.Euler(0,transform.rotation.eulerAngles.y,0);
        transform.localRotation = Quaternion.Euler(cameraRotat.eulerAngles.x, 0, 0);
*/