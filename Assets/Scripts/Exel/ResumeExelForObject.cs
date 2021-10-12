using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ResumeExelForObject : MesFonctions
{

    

    public InfoGlobalExel MesInfos = new InfoGlobalExel();

    public enum Type 
    {
    Nourriture,
    Materials,
    Soins,
    ArmeAfeu,
    ArmeMelee
    }
    public Type MainType;

    public int ID = 0;
   
    
    liseurExel Data;
    
    

    
    
    private void Start()
    {
        Data = liseurExel.LesDatas;      
        check();
        setType();
        Data.FindObjectInfo(ID, out MesInfos);
        
        
        
    }
    void check() 
    {
        if (ID!=0)
        {
            string newName ;
            Data.findObjectNameByID(ID,out newName);
            transform.name = newName;
        }
        if (ID==0)
        {
            
            Data.findObjectIDByName(transform.name, out ID);
            if (ID == 0)
            {
                print("Didnt find" + ID + " " + transform.name);
            }
            
        }

    
    }

    void setType() 
    {
        foreach (InfoGeneral MonItem in Data.MesListe.LesItems)
        {
            if (ID == MonItem.ID)
            {
                print(System.Enum.Parse(typeof(Type), MonItem.Type)); // vérifier
                MainType = (Type)System.Enum.Parse(typeof(Type), MonItem.Type);
            }
        }
        
    
    }
 
    
    


}

/*  void ChargeDataSelonType() 
   {

       correctionType(letype , out letype);


       if (letype == "Nourriture")
       {

           MainType = Type.nourriture;
           Nourriture = new TypeFood();
           ChargeNourriture();
       }
       else if (letype == "Materials")
       {
           MainType = Type.material;
           Materiel = new TypeMaterial();
           ChargeMaterials();
       }
       else if (letype == "Soins")
       {
           MainType = Type.soins;
           Soins = new TypeSoins();
           ChargeSoins();
       }
       else if (letype == "Arme à feu")
       {
           MainType = Type.ArmeAFeu;
           print("pas d'action pour le moment");
       }
       else if (letype == "Arme melee")
       {
           MainType = Type.Arme;
           print("pas d'action pour le moment");
       }
       else 
       {
           print("je fais rien");

       }



   }
   void ChargeNourriture() 
   {
       int index = 0 ;
       for (int i = 0; i < Data.MesListe.LaBouffe.Length-1; i++)
       {
           if (ID == Data.MesListe.LaBouffe[i].ID)
           {
               index = i;
               break;
           }
       }
       Nourriture = new TypeFood();
       Nourriture.values.x = Data.MesListe.LaBouffe[index].Nutrition;
       Nourriture.values.y = Data.MesListe.LaBouffe[index].Eau;
       Nourriture.values.z = Data.MesListe.LaBouffe[index].Chaleur;
       Nourriture.values.w = Data.MesListe.LaBouffe[index].Vie;
   }
   void ChargeMaterials() 
   {
       int index = 0;
       for (int i = 0; i < Data.MesListe.LesMat.Length - 1; i++)
       {
           if (ID == Data.MesListe.LesMat[i].ID)
           {
               index = i;
               break;
           }
       }
       Materiel = new TypeMaterial();
       Materiel.Type = Data.MesListe.LesMat[index].Type;
       if (Materiel.Type == "Bois")
       {
           Materiel.MonSousType = TypeMaterial.SousType.Bois;
       }
       else if (Materiel.Type == "pierre")
       {
           Materiel.MonSousType = TypeMaterial.SousType.Pierre;
       }
       else if (Materiel.Type == "beton")
       {
           Materiel.MonSousType = TypeMaterial.SousType.Beton;
       }
       else if (Materiel.Type == "fer brute")
       {
           Materiel.MonSousType = TypeMaterial.SousType.ferbrute;
       }
       else if (Materiel.Type == "fer raffine")
       {
           Materiel.MonSousType = TypeMaterial.SousType.ferraffine;
       }
       Materiel.value = Data.MesListe.LesMat[index].value;

   }
   void ChargeSoins() 
   {

       int index = 0;
       for (int i = 0; i < Data.MesListe.LesSoins.Length - 1; i++)
       {
           if (ID == Data.MesListe.LesSoins[i].ID)
           {
               index = i;
               break;
           }
       }


       Soins = new TypeSoins();
       Soins.valueSoins = Data.MesListe.LesSoins[index].value;

   }

   */
/*[System.Serializable] public class TypeFood 
{

    public Vector4 values;
   
}

[System.Serializable] public class TypeMaterial 
{

    public string Type;
    public int value;
    public enum SousType 
    {
    Bois,
    Pierre,
    Beton,
    ferbrute,
    ferraffine
    }
    public SousType MonSousType;
}

[System.Serializable] public class TypeSoins
{
    public int valueSoins;
}*/