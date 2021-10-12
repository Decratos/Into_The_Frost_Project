using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Item 
{
    public int ID;
    public string Nom;
    public int nombre;

    public  Item(Item d) 
    {
        ID = d.ID;
        Nom = d.Nom;
        nombre = d.nombre;


    }
}
