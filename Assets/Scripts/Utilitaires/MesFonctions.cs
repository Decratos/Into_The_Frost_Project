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
            //print("Le composant n'est pas sur l'objet envoyer");
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

    public static int GetChildCountOfObject(Transform LeTransform) 
    {
        int toReturn=0;
        foreach (Transform item in LeTransform)
        {
            toReturn++;
        }
        return toReturn;
    }


}
public struct InfoGlobalExel // créer une struct qui me permet de récupérer toutes les infos sur un objet 
{
    
    public int ID;
    public string Name;
    public int Durability;
    public int CraftingLevel;
    public float rarity;
    public float inflammability;
    public enum Type
    {
        Nourriture,
        Materials,
        Soins,
        ArmeAfeu,
        ArmeMelee,
        Sac,
        Vetement,
        Utilitaire
    }

    public Type TypeGeneral;
    public InfoExelSoins exelSoins;
    public InfoExelNourriturre exelNourriturre;
    public InfoExelCraft exelCraft;
    public InfoExelArme exelArme;
    public InfoExelGun exelGun;
    public InfoExelvetements Exelvetements;
    public InfoExelSac ExelSac;

    public void GetRightInfo()
    {
        liseurExel Datas = liseurExel.LesDatas;
        if (Datas.IsItInThisPage(PageExel.TypeDePageExel.Craft, ID))
        {
            Datas.FindObjectInfo(ID, out exelCraft);
        }

        for (int i = 0; i < System.Enum.GetValues(typeof(Type)).Length; i++)
            {

                if (i == (int)TypeGeneral)
                {
                    if (i == 0)
                    {
                        Datas.FindObjectInfo(ID, out exelNourriturre);
                    }
                    else if (i == 1)
                    {

                    }
                    else if (i == 2)
                    {
                        Datas.FindObjectInfo(ID, out exelSoins);
                    }
                    else if (i == 3)
                    {
                        Datas.FindObjectInfo(ID, out exelArme);
                    }
                    else if (i == 4)
                    {
                        Datas.FindObjectInfo(ID, out exelGun);
                    }
                    else if (i == 5) 
                    {
                    
                    }
                    else if (i == 6) 
                    {
                
                    }

                }
            }
        
        

    }


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
public struct InfoExelSac 
{
    public int NbrEmplacement; 

}
public struct InfoExelvetements 
{
    public enum SousCategorie 
    {
       Tshirt,
       pull,
       Pantalon,
       Chaussure,
       SousVetement,
       Manteau

    
    }
    public SousCategorie MaCategorie;
    public float ChaleurResistance;
    public float DegatResistance;
    public bool IsWeared;
}

