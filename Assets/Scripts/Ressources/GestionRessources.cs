using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionRessources : MesFonctions
{

    //public
    public float[] MesRessources;
    public int nombreMaxDObjet;
    public int nombreDobjetMaxSurSois;
    //private
    GestionDesScipt LaGestionDesScript;
    RessourcesType DataRessource;
   
    

    void Start()
    {
        FindGestionDesScripts(this.gameObject, out LaGestionDesScript);
        MesRessources = new float[System.Enum.GetValues(typeof(RessourcesType.TypeDeRessources)).Length] ;
    }
    public void AjouteAInventaire (GameObject Objet)
    {
        
        DataRessource = Objet.GetComponent<RessourcesType>();
        if (DataRessource.TypeOfRessouces==RessourcesType.TypeDeRessources.Food)
        {
            LaGestionDesScript.SurvieScript.GetFood(DataRessource.Foodvalue);
        }
        else 
        {
            for (int i = 0; i < MesRessources.Length-1; i++)
            {
                if (DataRessource.TypeOfRessouces == (RessourcesType.TypeDeRessources)i)
                {
                    MesRessources[i] += DataRessource.ValueRessources;
                    print(" je reçois " + DataRessource.ValueRessources);
                    print("j'ai mtn" + MesRessources[i]);
                }
            }
        }
    }
    public void SupprimeInventaire(int IndexValue ,float AEnlever ) 
    {

        print("je vais enlever" + AEnlever);
        MesRessources[IndexValue] -= AEnlever;
        print("g MTN " + MesRessources[IndexValue]);



    }
}
