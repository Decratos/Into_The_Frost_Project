using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GestionInput : MesFonctions
{

    PlayerInput PI; 
    [SerializeField] float Distance;


    private Vector2 movementInput = Vector2.zero;
    private Vector2 MouvementDeSouris = Vector2.zero;
    private bool sprint = true;
    public GestionDesScipt ScriptGestion;

    void Awake()
    {

        FindGestionDesScripts(this.gameObject, out ScriptGestion);
       
    }

    private void Start()
    {
        PI = GetComponent<PlayerInput>();
        
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
            
            ScriptGestion.LesMouvements.StateChange(MouvementPlayer.StateDeplacement.Saute); 

        }
        
    }

    public void Shoot(InputAction.CallbackContext context)
    {
       
        if (context.started)
        {
            GetComponent<PlayerActions>().Attack();
        
        }



    }

    public void Use(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            print("Using !");
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
                    else if(hit.transform.tag == "Container" && !PlayerSingleton.playerInstance.GetComponent<InventoryManager>().mainInventory.inventoryIsOpen)
                    {
                        hit.transform.GetComponent<Container>().OpenHideContainer();
                    }
                    else if(hit.transform.tag == "CraftTable" && !PlayerSingleton.playerInstance.GetComponent<InventoryManager>().mainInventory.inventoryIsOpen)
                    {
                        hit.transform.GetComponent<CraftingTable>().OpenHideTableWindow();
                    }
                    else if(hit.transform.tag == "Cooker")
                    {
                        hit.transform.GetComponent<Cooker>().OpenHideWindow();
                    }
                }
        }
    }

    public void SourisMouvement(InputAction.CallbackContext context) 
    {
       
       
        
        
        
        if(Time.timeScale != 0) 
        {
            if (PI.currentControlScheme == "Manette")
            {
                ScriptGestion.MouvementDeCamera.ReceptionDataManette(context.ReadValue<Vector2>());
            }
            else
            {
                MouvementDeSouris = context.ReadValue<Vector2>();
                ScriptGestion.MouvementDeCamera.ReceptionDonnerInput(MouvementDeSouris);
            }
        }
            

    }
    
    public void Sprint(InputAction.CallbackContext context) 
    {
        
        if (context.started)
        {
            print("Start Sprint");
            ScriptGestion.LesMouvements.StateChange(MouvementPlayer.StateDeplacement.Cour);
            
        }
        if (context.canceled)
        {
            print("End Sprint");
            ScriptGestion.LesMouvements.StateChange(MouvementPlayer.StateDeplacement.Marche);
        }
       

    }

    public void crounch(InputAction.CallbackContext context) 
    {

        if (context.started)
        {
            
            ScriptGestion.LesMouvements.StateChange(MouvementPlayer.StateDeplacement.Accroupie);
        }
        if (context.canceled)
        {
            ScriptGestion.LesMouvements.StateChange(MouvementPlayer.StateDeplacement.Marche);
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
        if (context.started && !PlayerSingleton.playerInstance.GetComponent<InventoryManager>().CheckInventoryOpen())
        {
            ScriptGestion.uiInventory.OpenHideInventory(false);
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
        //Atester
        if (context.ReadValue<float>()!=0)
        {
            PlayerSingleton.playerInstance.GetComponentInChildren<WeaponSystem>().ChangeWeaponMode();
        }
     
        
    }

    public void UseQuicksloth1(InputAction.CallbackContext context) 
    {

        print("Quicksloth1");

    }
    public void UseQuicksloth2(InputAction.CallbackContext context)
    {

        print("Quicksloth2");

    }
    public void UseQuicksloth3(InputAction.CallbackContext context)
    {

        print("Quicksloth3");

    }
    public void UseQuicksloth4(InputAction.CallbackContext context)
    {

        print("Quicksloth4");

    }
    public void Torch(InputAction.CallbackContext context) 
    {

        print("Allumez la lamp torch");

    }
    public void Pause(InputAction.CallbackContext context) 
    {

        print("Mettre en pause");

    }

    public void Vise(InputAction.CallbackContext context) 
    {
        print("Je vise");
    }
}
