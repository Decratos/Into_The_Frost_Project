using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RessourcesPourConstruction
{
    public RessourcesType.TypeDeRessources typeRessources;
    public float ValueNeeded;

}
public class ConstructionTest : MonoBehaviour
{

    public RessourcesPourConstruction[] hey;

    public void Construction(GestionRessources script) 
    {
        bool canConstruct = false;
       
        for (int i = 0; i < hey.Length; i++)
        {

            if (script.MesRessources[(int)hey[i].typeRessources] - hey[i].ValueNeeded <0)
            {
                canConstruct = false;
                print("pas assez de ressources");
                break;
            }
            else 
            {
                canConstruct = true;
            }


        }
        if (canConstruct)
        {
            for (int i = 0; i < hey.Length ; i++)
            {

                script.SupprimeInventaire((int)hey[i].typeRessources, hey[i].ValueNeeded);



            }
        }

        


        
    }
}
