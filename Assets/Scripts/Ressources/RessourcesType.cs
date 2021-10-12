using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesType : MesFonctions
{
    public enum TypeDeRessources 
    {
    
        Bois,
        Pierre,
        FerBrut,
        Beton,
        Food
    
    }

    public TypeDeRessources TypeOfRessouces;
    public Vector4 Foodvalue;
    public float ValueRessources;
    
}
