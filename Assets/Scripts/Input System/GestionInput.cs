using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestionInput : MesFonctions
{

    [SerializeField] float Distance;


    private Vector2 movementInput = Vector2.zero;
    private Vector2 MouvementDeSouris = Vector2.zero;
    private bool sprint = true;
    GestionDesScipt ScriptGestion;

    void Awake()
    {

        FindGestionDesScripts(this.gameObject, out ScriptGestion);
       
    }
   

    public void movement(InputAction.CallbackContext context)
    {
       
        
        
        movementInput = context.ReadValue<Vector2>();
        ScriptGestion.LesMouvements.Mouvement(movementInput);

    }
    public void Saut(InputAction.CallbackContext context)
    {
        
        if (context.started)
        {
            
            ScriptGestion.LesMouvements.Saut(); 

        }
        
    }

    public void Action(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(ScriptGestion.PlayerConstruct.constructionMode)
            {
                ScriptGestion.PlayerConstruct.Construct();
            }
            else
            {
                RaycastHit hit; // créer une valeur raycast
                Vector2 centerCamera = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2); // centre la souris
                Ray ray = Camera.main.ScreenPointToRay(centerCamera); // créer le ray 
            
                if (Physics.Raycast(ray, out hit, Distance)) // si le raycast touche
                {
                    if (hit.transform.tag == "Construction")
                    {
                    }
                    else if(hit.transform.tag == "RawRessources")
                    {
                        GetComponent<PlayerActions>().Gather(hit, ScriptGestion);
                    }
                    
                }
                else
                {
                    GetComponent<PlayerActions>().Attack();
                }
            }

        }



    }

    public void Use(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            RaycastHit hit; // créer une valeur raycast
                Vector2 centerCamera = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2); // centre la souris
                Ray ray = Camera.main.ScreenPointToRay(centerCamera); // créer le ray 
            
                if (Physics.Raycast(ray, out hit, Distance)) // si le raycast touche
                {
                    if (hit.transform.tag=="Ressources")
                    {
                        GetComponent<PlayerActions>().Collect(hit, ScriptGestion);
                        //ScriptGestion.LaGestionDesRessources.AjouteAInventaire(hit.transform.gameObject);
                    }
                    else if(hit.transform.tag == "Container")
                    {
                        hit.transform.GetComponent<Container>().OpenHideContainer();
                    }
                    else if(hit.transform.tag == "CraftTable")
                    {
                        hit.transform.GetComponent<CraftingTable>().OpenHideTableWindow();
                    }
                }
        }
    }

    public void SourisMouvement(InputAction.CallbackContext context) 
    {

      
       
        MouvementDeSouris = context.ReadValue<Vector2>();
        //faire un debug du raycast de la souris
        if(Time.timeScale != 0)
            ScriptGestion.MouvementDeCamera.ReceptionDonnerInput(MouvementDeSouris);

    }
    
    public void Sprint(InputAction.CallbackContext context) 
    {
        
        if (context.started)
        {
            ScriptGestion.LesMouvements.GoSprint();
        }
        if (context.canceled)
        {
            ScriptGestion.LesMouvements.Walk();
        }
       

    }

    public void BuildMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ScriptGestion.PlayerConstruct.constructionMode = !ScriptGestion.PlayerConstruct.constructionMode;
            ScriptGestion.PlayerConstruct.ChangeBuildMode(ScriptGestion.PlayerConstruct.constructionMode);
        }
    }

    public void InventoryMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ScriptGestion.uiInventory.OpenHideInventory(true);
            CraftUI.instance.OpenHideCraftUI(0);
        }
    }

    public void SwitchStructure(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerSingleton.playerInstance.GetComponent<PlayerConstruct>().increaseIdStructure();
        }
    }

    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerSingleton.playerInstance.GetComponentInChildren<WeaponSystem>().ChangeWeaponMode();
        }
    }
}
