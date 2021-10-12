using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionDesScipt : MesFonctions
{
    public GestionInput InputGestion;
    public CameraMouvement MouvementDeCamera;
    public SurvivalSysteme SurvieScript;
    public MouvementPlayer LesMouvements;
    public GestionRessources LaGestionDesRessources;
    public Inventory Inventory;
    public UIInventory uiInventory;
    public PlayerConstruct PlayerConstruct;

    [HideInInspector] public static GestionDesScipt ScriptGestion;
    void Awake()
    {
        if (ScriptGestion != this)
        {
            ScriptGestion = this;
        }
    }

}
