using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionDesScipt : MesFonctions
{
    // sert à stocker les scripts sur le joueur pour éviter le getComponent
    public GestionInput InputGestion;
    public CameraMouvement MouvementDeCamera;
    public SurvivalSysteme SurvieScript;
    public MouvementPlayer LesMouvements;
    public GestionRessources LaGestionDesRessources;
    public Inventory Inventory;
    public UIInventory uiInventory;
    public PlayerConstruct PlayerConstruct;

    // pour faire de se script un singelton
    [HideInInspector] public static GestionDesScipt ScriptGestion;
    void Awake()
    {
        if (ScriptGestion != this)
        {
            ScriptGestion = this;
        }
    }

}
