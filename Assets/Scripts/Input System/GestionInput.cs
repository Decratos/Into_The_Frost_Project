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

        FindGestionDesScripts(this.gameObject, out ScriptGestion); // trouve le script gestion des scripts
       
    }
   

    public void movement(InputAction.CallbackContext context) //lance le mouvement 
    {
       
        
        
        movementInput = context.ReadValue<Vector2>(); //récupére la valeur vecteur 2 des touches
        ScriptGestion.LesMouvements.Mouvement(movementInput); // envois au scipt des mouvements les données necessaire

    }
    public void Saut(InputAction.CallbackContext context) // lance le saut
    {
        
        if (context.started) // si le joueur appuie sur l'input
        {
            
            ScriptGestion.LesMouvements.Saut();  // envois au script l'instruction de sauter

        }
        
    }

    public void Action(InputAction.CallbackContext context) // lance l'action
    {
        if (context.started) // si le joueur appuie sur l'input
        {
            if(ScriptGestion.PlayerConstruct.constructionMode)// si le joueur est en mode construction
            {
                ScriptGestion.PlayerConstruct.Construct(); // envois au script l'instruction de construire
            }
            else
            {
                RaycastHit hit; // créer une valeur raycast
                Vector2 centerCamera = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2); // centre la souris
                Ray ray = Camera.main.ScreenPointToRay(centerCamera); // créer le ray 
            
                if (Physics.Raycast(ray, out hit, Distance)) // si le raycast touche
                {
                    if (hit.transform.tag=="Ressources")// si le joueur touche une ressources
                    {
                        GetComponent<PlayerActions>().Collect(hit, ScriptGestion);
                        //ScriptGestion.LaGestionDesRessources.AjouteAInventaire(hit.transform.gameObject);
                    }
                    else if (hit.transform.tag == "Construction") // si le joueur touche une construction
                    {

                    }
                    else if(hit.transform.tag == "RawRessources") // si le joueur touche ce truc
                    {
                        print("Commentaire à résoudre"); //mettre players action dans la gestion des scripts
                        Debug.Break();//à retirer une fois le commentaire résolue
                        GetComponent<PlayerActions>().Gather(hit, ScriptGestion); // récupére le script Player action
                    }
                    else if(hit.transform.tag == "NPC")
                    {
                        GetComponent<PlayerActions>().Attack(hit); // récupére le script Player action
                    }
                    
                }
            }

        }



    }

    public void SourisMouvement(InputAction.CallbackContext context) // Récupére les déplacements de souris
    {

      
       
        MouvementDeSouris = context.ReadValue<Vector2>();//lis la valeur reçue.
        //faire un debug du raycast de la souris
        if(Time.timeScale != 0)//si le jeu n'est pas en pause
            ScriptGestion.MouvementDeCamera.ReceptionDonnerInput(MouvementDeSouris); // lance le mouvement de caméras

    }
    
    public void Sprint(InputAction.CallbackContext context) // lance le sprint
    {
        
        if (context.started) // si le joueur appuie sur l'input
        {
            ScriptGestion.LesMouvements.GoSprint(); // envois l'instruction au script de courir
        }
        if (context.canceled) // si le joueur n'appuie plus sur l'input
        {
            ScriptGestion.LesMouvements.Walk(); //envois l'instruction au script de marcher
        }
       

    }

    public void BuildMode(InputAction.CallbackContext context) // lance le mode de construction
    {
        if (context.started) // si le joueur appuie sur l'input
        {
            ScriptGestion.PlayerConstruct.constructionMode = !ScriptGestion.PlayerConstruct.constructionMode; // dis au script qu'il doit inverser sa valeur booléene
            ScriptGestion.PlayerConstruct.ChangeBuildMode(ScriptGestion.PlayerConstruct.constructionMode); // envois au script l'instrcution quand à la construction
        }
    }

    public void InventoryMode(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ScriptGestion.uiInventory.OpenHideInventory(false);
            CraftUI.instance.OpenHideCraftUI();
        }
    }

    public void SwitchStructure(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PlayerSingleton.playerInstance.GetComponent<PlayerConstruct>().increaseIdStructure();
        }
    }
}
