using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesFonctions : MonoBehaviour
{
    public static void FindGestionDesScripts(GameObject Object, out GestionDesScipt ScriptGestion) 
    {
        
        if (Object.GetComponent<GestionDesScipt>())
        {
            ScriptGestion = Object.GetComponent<GestionDesScipt>();
        }
        else 
        {
            ScriptGestion = null;
            print("Le composant n'est pas sur l'objet envoyer");
        }
    }
    public static void FindDataExelForObject(out liseurExel Data) // à relier
    {
        Data = GameObject.Find("DataBasis").GetComponent<liseurExel>();
     
    }

    public static void correctionType(string datas , out string correction)// corrige les espace invisibles tiré des exel
    {

        char[] array = datas.ToCharArray();
        char[] arrayCorrected = new char[array.Length - 1];
        bool hasBeenCorrected=false;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == '\r')
            {
                for (int j = 0; j < arrayCorrected.Length; j++)
                {
                    arrayCorrected[j] = array[j];
                    
                }
                hasBeenCorrected = true;
                
            }
        }
        if (hasBeenCorrected)
        {
            correction = new string(arrayCorrected);
        }
        else 
        {
            correction = datas;
        }
        




    }

    public static string ParseForEnum(string ToCorrecte)
    {
        string corrected;
        char[] array = ToCorrecte.ToCharArray();
        List<char> NewString = new List<char>();
        for (int i = 0; i < array.Length - 1; i++)
        {
            if (array[i] != ' ' || array[i] != '\r')
            {

                NewString.Add(array[i]);
            }

        }

        corrected = new string(NewString.ToArray());

        return corrected;
    }
}
public struct InfoGlobalExel // créer une struct qui me permet de récupérer toutes les infos sur un objet 
{
    
    public int ID;
    public string Name;
    public int Durability;
    public int CraftingLevel;
    public enum Type
    {
        Nourriture,
        Materials,
        Soins,
        ArmeAfeu,
        ArmeMelee
    }

    public Type TypeGeneral;
    public InfoExelSoins exelSoins;
    public InfoExelNourriturre exelNourriturre;
    public InfoExelCraft exelCraft;
    public InfoExelArme exelArme;
    public InfoExelGun exelGun;
    



}
public struct InfoExelSoins 
{

    public float value;

}
public struct InfoExelNourriturre 
{
    public float Nourritture;
    public float Eau;
    public float Chaleur;
    public float Vie;
    public Vector4 resume;
}
public struct InfoExelCraft 
{
    
    public int[] IDdesRessourcesNecessaire;
    public string[] NomdesRessourcesNecessaire;
    public int[] LeNombreNecessaire;
    

}
public struct InfoExelGun 
{

    public float Degat;
    public float Speed;
    public int Chargeur;
    public float Rechargement;
    public float Power;

}

public struct InfoExelArme 
{

    public float Degat;
    public float Speed;

}