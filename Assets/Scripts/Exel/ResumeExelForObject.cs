using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class ResumeExelForObject : MesFonctions
{

    

    public InfoGlobalExel MesInfos = new InfoGlobalExel();

    public enum Type // 
    {
    Nourriture,
    Materials,
    Soins,
    ArmeAfeu,
    ArmeMelee,
    Utilitaire,
    Sac,
    Vetements,
    
    }
    public Type MainType;

    public int ID = 0;
   
    
    liseurExel Data;
    
    

    
    
    private void Start()
    {
        Data = liseurExel.LesDatas;
        print("Commentaire à résoudre"); //voir pour mettre enum de liseur exel plutôt
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

