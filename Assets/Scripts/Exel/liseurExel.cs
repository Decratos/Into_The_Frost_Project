using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

#region Les scripts necessaire
[System.Serializable]
public class InfoGeneral // Liste de tous les items
{
    public int ID;
    public string Name;
    public string Type;
    public int Durability;
    public int CraftingLevel;
    public float Rarity;
    public float Inflamability;
}
[System.Serializable]
public class InfoMateriaux // Liste de tous les matériaux
{
    public int ID;
    public string Name;
    
   
}
[System.Serializable]
public class InfoNourriture // Liste de tous les aliments
{
    public int ID;
    public string Name;
    public int Nutrition;
    public int Eau;
    public int Chaleur;
    public int Vie;
}
[System.Serializable]
public class InfoSoins // Liste de tous les items de soins
{
    public int ID;
    public string Name;
    public int value;
}
[System.Serializable]
public class InfoCraft // Liste de tous les items de soins
{
    public int ID;
    public string Name;
    public string[] RessourcesNecessaire;
    public int[] LeNombreNecessaire;
    public int[] IDDesressources;
    public int CraftingLevel;

}
[System.Serializable]
public class InfoWeapon // info de la page arme de mélée
{
    public int ID;
    public string Name;
    public float degat;
    public float Speed;
}

[System.Serializable]
public class InfoGun // Info de la page arme à feu
{
    public int ID;
    public string Name;
    public float degat;
    public float Power;
    public float Speed;
    public int Chargeur;
    public float Rechargement;
}

[System.Serializable]
public class InfoVetements //Les infos de la page vetements
{
    public int ID;
    public string Name;
    public string Categorie;
    public float ChaleurResistance;
    public float DegatResistance;
}

[System.Serializable]
public class InfoSac // Les infos de la page sac
{
    public int ID;
    public string Name;
    public int NbrEmplacement;
}

[System.Serializable]
public class InfoUtilitaire //Les infos de la page utilitaire
{
    public int ID;
    public string Name;
    public int TheLevelForCraft;

}


[System.Serializable]
public class Info // regroupement  de toutes les infos
{

    public InfoGeneral[] LesItems;
    public InfoMateriaux[] LesMat;
    public InfoNourriture[] LaBouffe;
    public InfoSoins[] LesSoins;
    public InfoCraft[] LesCrafts;
    public InfoGun[] LesFlingues;
    public InfoWeapon[] LesArmes;
    public InfoVetements[] LesVetements;
    public InfoSac[] LesSacs;
    public InfoUtilitaire[] LesUtilitaires;

}


[System.Serializable]
public class PageExel //info de page exel
{
    public TextAsset MaPage;
    public int nombreDeColonne;
    public enum TypeDePageExel 
    {
    
    General,
    nourriture,
    Materiaux,
    soins,
    Craft,
    Weapon,
    Gun,
    Utilitaire,
    SacADos,
    Vetements


    }
    public TypeDePageExel LesInfoPagesExel;

}

#endregion

public class liseurExel : MesFonctions // ce script vas chercher toutes les infos dans les excel et les stocker, pour permettre d'aller chercher toute les donnés
{
    
    [SerializeField] PageExel[] MesPages; // contient toute les pages exel

    
    [HideInInspector] public static liseurExel LesDatas; //singelton

    //
    public Info MesListe = new Info(); // crée la valeur contenant toute mes infos


    void Awake() // charge les infos
    {
        if (LesDatas!=this || LesDatas== null)//singeltonisation
        {
            LesDatas = this;
        }
        DontDestroyOnLoad(this.gameObject); // Permet de garder les infos entre chaque scéne 
        readEachCSV(); // lance la lecture des exel
    }

    #region setDatas

    [Button(ButtonSizes.Medium)]
    private void testRemplissage() 
    {
        readEachCSV();
    }

    void readEachCSV() 
    {
        //print("LiseurExel readEachCSV");
        for (int i = 0; i < MesPages.Length; i++) // pour chaque page exel
        {
            string[] data = MesPages[i].MaPage.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);// split les données dans un tableau
            int tableSize = data.Length / MesPages[i].nombreDeColonne - 1;// divise par le nombre de colonne dans l'exel
            setInfo(MesPages[i].LesInfoPagesExel, MesPages[i].nombreDeColonne, data, tableSize); // Vas set toutes les informations dans les tableau

        }

    } // lis chaque page
    void setInfo (PageExel.TypeDePageExel mon_type, int nombreDeColonne, string[] data,int size) //set les données dans info 
    {
       
        // mon_type est réglée dans l'inspecteur
        // Nombre de colonne que la page comporte (tjr dans l'inspecteur)
        // data est la valeur en string qui a été séparré
        // size est le nombre de data au total

        if (mon_type == PageExel.TypeDePageExel.General)
        {
            MesListe.LesItems = new InfoGeneral[size]; // créer un tableau qui contiendra toute les valeurs
            for (int i = 0; i < size; i++)
            {
                MesListe.LesItems[i] = new InfoGeneral();// set la variable
                MesListe.LesItems[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]); // récuppére les ID
                correctionType(data[nombreDeColonne * (i + 1) + 1],out MesListe.LesItems[i].Name); // correction type est une fontion qui enléve tout caractére invisible ici utiliser pour récupérer le nom
                correctionType(data[nombreDeColonne * (i + 1) + 2],out MesListe.LesItems[i].Type); // correction type est une fontion qui enléve tout caractére invisible ici utiliser pour récupérer le type
                MesListe.LesItems[i].Durability = int.Parse(data[nombreDeColonne * (i + 1)+3]); // récupére la durability de l'objet
                MesListe.LesItems[i].CraftingLevel = int.Parse(data[nombreDeColonne * (i + 1) + 4]); // Récupére le crafting level
                MesListe.LesItems[i].Rarity = MonParse(data[nombreDeColonne * (i + 1) + 5]); // récupére la rareté de base 
                MesListe.LesItems[i].Inflamability = MonParse(data[nombreDeColonne * (i + 1) + 6]);// récupére l'inflammabitité de de l'objet
            }
        }// si la page est de type général
        else if (mon_type == PageExel.TypeDePageExel.Materiaux)
        {
            MesListe.LesMat = new InfoMateriaux[size];// créer un tableau qui contiendra toute les valeurs
            for (int i = 0; i < size; i++)
            {
                MesListe.LesMat[i] = new InfoMateriaux();// set la variable
                MesListe.LesMat[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]);// récuppére les ID
                correctionType(data[nombreDeColonne * (i + 1) + 1], out MesListe.LesMat[i].Name); // récupére le nom
                
            }
        }// si la page est de type materiaux
        else if (mon_type == PageExel.TypeDePageExel.nourriture)
        {
            MesListe.LaBouffe = new InfoNourriture[size];// créer un tableau qui contiendra toute les valeurs
            for (int i = 0; i < size; i++)
            {
               
                    MesListe.LaBouffe[i] = new InfoNourriture();// set la variable
                    MesListe.LaBouffe[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]);// récuppére les ID
                    correctionType(data[nombreDeColonne * (i + 1) + 1], out MesListe.LaBouffe[i].Name) ;// récupére le nom
                    MesListe.LaBouffe[i].Nutrition = int.Parse(data[nombreDeColonne * (i + 1) + 2]); // récupére la nutrition que cela vas donner
                    MesListe.LaBouffe[i].Eau = int.Parse(data[nombreDeColonne * (i + 1) + 3]); // récupére la soif que vas donner l'objet
                    MesListe.LaBouffe[i].Chaleur = int.Parse(data[nombreDeColonne * (i + 1) + 4]); // récupére la chaleur que la nourriture vas donner
                    MesListe.LaBouffe[i].Vie = int.Parse(data[nombreDeColonne * (i + 1) + 5]); // Récupére la vie que l'objet vas donner
                
            }
        }// si la page est de type nourriture
        else if (mon_type == PageExel.TypeDePageExel.soins)
        {
            MesListe.LesSoins = new InfoSoins[size];// créer un tableau qui contiendra toute les valeurs
            for (int i = 0; i < size; i++)
            {
                MesListe.LesSoins[i] = new InfoSoins();// set la variable
                MesListe.LesSoins[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]);// récuppére les ID
                correctionType(data[nombreDeColonne * (i + 1) + 1], out MesListe.LesSoins[i].Name );// récupére le nom
                MesListe.LesSoins[i].value = int.Parse(data[nombreDeColonne * (i + 1) + 2]); // récuppére la valeur de soin
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.Craft)
        {
           
            MesListe.LesCrafts = new InfoCraft[size];// créer un tableau qui contiendra toute les valeurs
            for (int i = 0; i < size; i++)
            {
                MesListe.LesCrafts[i] = new InfoCraft();// set la variable
                MesListe.LesCrafts[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]);// récuppére les ID
                correctionType(data[nombreDeColonne * (i + 1) + 1], out MesListe.LesCrafts[i].Name);// récupére le nom
                MesListe.LesCrafts[i].RessourcesNecessaire = data[nombreDeColonne * (i + 1) + 2].Split(char.Parse("/")); // split les ressources
                MesListe.LesCrafts[i].LeNombreNecessaire= ParseArray(data[nombreDeColonne * (i + 1) + 3].Split(char.Parse("/")));// split le nombre necessaire
                MesListe.LesCrafts[i].CraftingLevel = int.Parse(data[nombreDeColonne * (i + 1) + 4]); // récupére le crafting level necessaire pour créer l'objet
                MesListe.LesCrafts[i].IDDesressources = new int[MesListe.LesCrafts[i].RessourcesNecessaire.Length];// recupére les ID des ressources necessaire
                for (int j  = 0; j < MesListe.LesCrafts[i].RessourcesNecessaire.Length; j++)
                {
                                      
                    findObjectIDByName(MesListe.LesCrafts[i].RessourcesNecessaire[j], out MesListe.LesCrafts[i].IDDesressources[j]);
                          
                }
              


            }
        }// si la page est craft
        else if (mon_type == PageExel.TypeDePageExel.Weapon)
        {
            MesListe.LesArmes = new InfoWeapon[size]; // créer un tableau qui contiendra toute les valeurs
            for (int i = 0; i < size; i++)
            {
                
                MesListe.LesArmes[i] = new InfoWeapon();
                MesListe.LesArmes[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]); // récuppére les ID
                correctionType(data[nombreDeColonne * (i + 1) + 1], out MesListe.LesArmes[i].Name); // récupére le nom de l'objet
                MesListe.LesArmes[i].degat = MonParse(data[nombreDeColonne * (i + 1) + 2]); // récupére les dégats de l'arme
                MesListe.LesArmes[i].Speed = MonParse(data[nombreDeColonne * (i + 1) + 3]); // récupére la vitesse de l'arme
                

            }

        }// si la page est celle des arme de mélée
        else if (mon_type == PageExel.TypeDePageExel.Gun)
        {
            MesListe.LesFlingues = new InfoGun[size];// créer un tableau qui contiendra toute les valeurs
            for (int i = 0; i < size; i++)
            {

                MesListe.LesFlingues[i] = new InfoGun();// set la variable
                MesListe.LesFlingues[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]); // récuppére les ID
                correctionType(data[nombreDeColonne * (i + 1) + 1], out MesListe.LesFlingues[i].Name); // récupére le nom de l'objet
                MesListe.LesFlingues[i].degat= MonParse(data[nombreDeColonne * (i + 1) + 2]); // récupére les dégats de l'arme
                MesListe.LesFlingues[i].Power = MonParse(data[nombreDeColonne * (i + 1) + 3]); // récupére la puissance de l'arme
                MesListe.LesFlingues[i].Speed = MonParse(data[nombreDeColonne * (i + 1) + 4]); // récupére la vitesse de l'arme
                MesListe.LesFlingues[i].Chargeur = int.Parse(data[nombreDeColonne * (i + 1)] + 5); // récupére la taille du chargeur
                MesListe.LesFlingues[i].Rechargement= MonParse(data[nombreDeColonne * (i + 1) + 6]); // récupére le temps de rechargement

               



            }
            

        }// si la page est celle des armes à feu
        else if (mon_type == PageExel.TypeDePageExel.SacADos)
        {
            MesListe.LesSacs = new InfoSac[size];
            for (int i = 0; i < size; i++)
            {
                MesListe.LesSacs[i] = new InfoSac();
                MesListe.LesSacs[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]);// récuppére les ID
                correctionType(data[nombreDeColonne * (i + 1) + 1], out MesListe.LesSacs[i].Name);// récupére le nom du sac
                MesListe.LesSacs[i].NbrEmplacement = int.Parse(data[nombreDeColonne * (i + 1) + 2]); // récupére la capacité du sac
            }
        } // si la page est celle du sac à dos
        else if (mon_type == PageExel.TypeDePageExel.Utilitaire)
        {
            MesListe.LesUtilitaires = new InfoUtilitaire[size];
            for (int i = 0; i < size; i++)
            {
                MesListe.LesUtilitaires[i] = new InfoUtilitaire();
                
                MesListe.LesUtilitaires[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]);// récuppére les ID
                correctionType(data[nombreDeColonne * (i + 1) + 1], out MesListe.LesUtilitaires[i].Name);// récupére le nom de l'objet 
                MesListe.LesUtilitaires[i].TheLevelForCraft = int.Parse(data[nombreDeColonne * (i + 1)+2]); // récupére le niveau de craft qu'il donne

            }
        }
        else if (mon_type == PageExel.TypeDePageExel.Vetements)
        {
            MesListe.LesVetements = new InfoVetements[size];
            for (int i = 0; i < size; i++)
            {
                MesListe.LesVetements[i] = new InfoVetements();
                MesListe.LesVetements[i].ID = int.Parse(data[nombreDeColonne * (i + 1)]);
                correctionType(data[nombreDeColonne * (i + 1) + 1], out MesListe.LesVetements[i].Name);
                correctionType(data[nombreDeColonne * (i + 1) + 2], out MesListe.LesVetements[i].Categorie);
                MesListe.LesVetements[i].ChaleurResistance = MonParse(data[nombreDeColonne * (i + 1) + 3]);
                MesListe.LesVetements[i].DegatResistance = MonParse(data[nombreDeColonne * (i + 1) + 4]);
            }

        }// si la page est de type vetements 
    }

    #endregion

    #region mesFonctionsInterne
    float MonParse (string value) //fonction pour le tableur
    {
        //print("Parse float ");
        char[] array = value.ToCharArray();
        for (int i = 0; i < array.Length-1; i++)
        {
            if (array[i] == '.')
            {
                array[i] = ',';
            }
        }
        string Resultat = new string(array);
       //print("la valuer modifié" + Resultat);

        return float.Parse(Resultat);
    } 
    bool ParseBool(string value) 
    {
        //print("Parse bool");
        correctionType(value, out value);
        if (value == "FALSE" )
        {
            return false;
        }
        else if (value == "TRUE")
        {
            return true;
        }
        else 
        {
           
            return false;
        }

    
    }//fonction pour transformer string en bool

    bool[] ParseArrayBool(string[] value) 
    {
        //print("ParseArrayBool");
        bool[] valueBool = new bool[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            
            valueBool[i] = ParseBool(value[i]);
        }

        return valueBool;

    }

    int[] ParseArray(string[] value) 
    {
        //print("parse ARRAY");
        int[] valueInt = new int[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            valueInt[i] = int.Parse(value[i]);
        }
        
        return valueInt;
    }// lorsqu'il y'a plusieurs variables dans la case exel
    
    // bool pour vérifier si dans liste des crafts
    
   

    #endregion
    #region methode Accessible à tous
        
    #region findObjectMethod

    #region byName

    public void FindObjectInfo(string name, out InfoGlobalExel InfoGlobal) 
    {
        //print("FindObjectInfo name out Infoglobal");
        InfoGlobalExel lesInfos = new InfoGlobalExel(); // créer une nouvelle struct
        string correctedType; // enregistre le type d'objet
        foreach (InfoGeneral info in MesListe.LesItems) // pour chaque objet dans la liste
        {
            if (name == info.Name) // je regarde si le nom est bon
            {
                lesInfos.ID = info.ID;// set l'ID
                lesInfos.Name = info.Name; // set le nom
                lesInfos.Durability = info.Durability; // set la durabilité
                lesInfos.CraftingLevel = info.CraftingLevel;// set le niveau de crafting
                lesInfos.rarity = info.Rarity;
                lesInfos.inflammability = info.Inflamability;
                correctionType(info.Type , out correctedType); // corrige le type
                string [] EnumType = System.Enum.GetNames(typeof(InfoGlobalExel.Type));// créer un tableau de string des types
                for (int i = 0; i < EnumType.Length-1; i++) // pour chaque type dans l'enum
                {
                    correctionType(EnumType[i],out EnumType[i]); // corrige les enums au cas ou
                    
                    if (EnumType[i] == correctedType) // si l'enum est égale au type
                    {
                        lesInfos.TypeGeneral = (InfoGlobalExel.Type)i; // set le type général
                        break; // break le for
                    }
                }
                if (correctedType == InfoGlobalExel.Type.Nourriture.ToString()) // si c'est de la bouffe
                {
                    lesInfos.exelNourriturre = new InfoExelNourriturre(); // je créer une type bouffe
                    FindObjectInfo(name,out lesInfos.exelNourriturre);
                }
                else if (correctedType == InfoGlobalExel.Type.Soins.ToString())
                {
                    lesInfos.exelSoins = new InfoExelSoins();
                    FindObjectInfo(name ,out lesInfos.exelSoins);
                }
                else if (correctedType == InfoGlobalExel.Type.Materials.ToString())
                {

                    //print("pas d'action pour le moment");


                    /*lesInfos.exelMaterial = new InfoExelMaterial();
                   
                        foreach (InfoMateriaux SubInfo in MesListe.LesMat)
                        {
                            if (SubInfo.Name == name)
                            {

                            lesInfos.exelMaterial.MonSousType = (InfoExelMaterial.SousType)System.Enum.Parse(typeof(InfoExelMaterial.SousType), SubInfo.Type) ;
                            lesInfos.exelMaterial.value = SubInfo.value;

                            }
                    }*/
                }
                else if (correctedType == InfoGlobalExel.Type.ArmeAfeu.ToString())
                {
                    lesInfos.exelGun = new InfoExelGun();
                    FindObjectInfo(name, out lesInfos.exelGun);
                    
                }
                else if (correctedType == InfoGlobalExel.Type.ArmeMelee.ToString())
                {
                    lesInfos.exelArme = new InfoExelArme();
                    FindObjectInfo( name , out lesInfos.exelArme);

                }
                else if (correctedType == InfoGlobalExel.Type.Vetement.ToString())
                {
                    lesInfos.Exelvetements = new InfoExelvetements();
                    FindObjectInfo( name ,out lesInfos.Exelvetements);


                }
                else if (correctedType == InfoGlobalExel.Type.Sac.ToString())
                {
                    lesInfos.ExelSac = new InfoExelSac();
                    FindObjectInfo(name ,out lesInfos.ExelSac);

                }
                else
                {
                    //print("je fais rien");

                }

                if (IsItInThisPage(PageExel.TypeDePageExel.Craft,info.ID))
                {
                   
                    FindObjectInfo( name, out lesInfos.exelCraft);
                }
               

                break; // j'ai trouvé ma cible je peux casser
            }
        }
        if (lesInfos.ID == 0) // si l'objet n'est pas dans la base de donner
        {
            print("l'item n'est pas dans la base de donnée");
            //Debug.Break();

        }
        InfoGlobal = lesInfos;
    }// trouve des infos selon le nom de l'objet
    public void FindObjectInfo(string name, out InfoExelNourriturre NourritureInfo)
    {
        //print("FindObjectInfo name out InfoNourriture");
        NourritureInfo = new InfoExelNourriturre();
        bool found = false;
        for (int i = 0; i < MesListe.LaBouffe.Length ; i++)
        {
            if (name == MesListe.LaBouffe[i].Name)
            {
                found = true;
                NourritureInfo.Nourritture = MesListe.LaBouffe[i].Nutrition;
                NourritureInfo.Eau = MesListe.LaBouffe[i].Eau;
                NourritureInfo.Chaleur = MesListe.LaBouffe[i].Chaleur;
                NourritureInfo.Vie = MesListe.LaBouffe[i].Vie;
                NourritureInfo.resume = new Vector4(MesListe.LaBouffe[i].Nutrition, MesListe.LaBouffe[i].Eau, MesListe.LaBouffe[i].Chaleur, MesListe.LaBouffe[i].Vie);
            }
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }
    }//récupérre les données de la bouffe;
    public void FindObjectInfo(string name, out InfoExelSoins SoinsInfo)
    {
        //print("FindObjectInfo name out InfoSoins");
        SoinsInfo = new InfoExelSoins();
        bool found = false;
        for (int i = 0; i < MesListe.LesSoins.Length ; i++)
        {
            if (name == MesListe.LesSoins[i].Name)
            {
                found = true;
                SoinsInfo.value = MesListe.LesSoins[i].value;
            }
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }
    }//récupérre les données des soins;
    public void FindObjectInfo(string name, out InfoExelCraft CraftInfo)
    {
        //print("FindObjectInfo name out craftInfo");
        CraftInfo = new InfoExelCraft();
        bool found = false;
        for (int i = 0; i < MesListe.LesCrafts.Length ; i++)
        {
            if (name == MesListe.LesCrafts[i].Name)
            {
                found = true;
                int[] LesID = new int[MesListe.LesCrafts[i].RessourcesNecessaire.Length];

                for (int j = 0; j < MesListe.LesCrafts[i].RessourcesNecessaire.Length ; j++)
                {
                    findObjectIDByName(MesListe.LesCrafts[i].RessourcesNecessaire[j], out LesID[j]);
                }
                CraftInfo.IDdesRessourcesNecessaire = LesID;
                CraftInfo.LeNombreNecessaire = MesListe.LesCrafts[i].LeNombreNecessaire;
                CraftInfo.NomdesRessourcesNecessaire = MesListe.LesCrafts[i].RessourcesNecessaire;
                CraftInfo.CraftingLevel = MesListe.LesCrafts[i].CraftingLevel;
                break;
            }
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }
    }//récupérre les données des craft;
    public void FindObjectInfo(string name, out InfoExelArme ArmeInfo) // récupére les données des armes de mélée
    {
        ArmeInfo = new InfoExelArme();
        bool found = false;
        foreach (InfoWeapon arme in MesListe.LesArmes)
        {
            if (name == arme.Name)
            {
                found = true;
                ArmeInfo.Degat = arme.degat;
                ArmeInfo.Speed = arme.Speed;

            }
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }


    }
    public void FindObjectInfo(string name, out InfoExelGun GunInfo) 
    {
        GunInfo = new InfoExelGun();
        bool found = false;
        foreach (InfoGun item in MesListe.LesFlingues)
        {
            if (item.Name == name)
            {
                GunInfo.Chargeur = item.Chargeur;
                GunInfo.Degat = item.degat;
                GunInfo.Power = item.Power;
                GunInfo.Rechargement = item.Rechargement;
                GunInfo.Speed = item.Speed;
            }


        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }
    
    } // récupère les données des gun
    public void FindObjectInfo(string name, out InfoExelSac SacInfo)
    {
        SacInfo = new InfoExelSac();
        bool found = false;
        foreach (InfoSac item in MesListe.LesSacs)
        {
            if (item.Name == name)
            {
                SacInfo.NbrEmplacement = item.NbrEmplacement;
                found = true;
            }


        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }
    } // récupére les données du sac
    public void FindObjectInfo(string name, out InfoExelvetements VetementInfo)
    {
        VetementInfo = new InfoExelvetements();
        bool found = false;
        foreach (InfoVetements item in MesListe.LesVetements)
        {
            if (item.Name == name)
            {
                VetementInfo.ChaleurResistance = item.ChaleurResistance;
                VetementInfo.DegatResistance = item.DegatResistance;
                string[] EnumTypeVetements = System.Enum.GetNames(typeof(InfoExelvetements.SousCategorie)); // récupére une liste des sous type
                for (int i = 0; i < EnumTypeVetements.Length; i++)
                {
                    correctionType(EnumTypeVetements[i],out EnumTypeVetements[i]);
                    if (EnumTypeVetements[i] == item.Categorie)
                    {
                       
                        VetementInfo.MaCategorie = (InfoExelvetements.SousCategorie)System.Enum.Parse(typeof(InfoExelvetements.SousCategorie), item.Categorie); ;
                        break;
                    }

                }
                
                found = true;
            }

        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }

    } // récupére les données des vêtements
    //
    #endregion

    #region ByID
    public void FindObjectInfo(int ID, out InfoGlobalExel InfoGlobal)
    {
        //print("FindObjectInfo name out Infoglobal");
        InfoGlobalExel lesInfos = new InfoGlobalExel(); // créer une nouvelle struct
        string correctedType; // enregistre le type d'objet
        foreach (InfoGeneral info in MesListe.LesItems) // pour chaque objet dans la liste
        {
            if (ID == info.ID) // je regarde si le nom est bon
            {
                lesInfos.ID = info.ID;// set l'ID
                lesInfos.Name = info.Name; // set le nom
                lesInfos.Durability = info.Durability; // set la durabilité
                lesInfos.CraftingLevel = info.CraftingLevel;// set le niveau de crafting
                lesInfos.rarity = info.Rarity;
                lesInfos.inflammability = info.Inflamability; 
                correctionType(info.Type, out correctedType); // corrige le type
                string[] EnumType = System.Enum.GetNames(typeof(InfoGlobalExel.Type));// créer un tableau de string des types
                print("Commentaire à résoudre");
                for (int i = 0; i < EnumType.Length; i++) // pour chaque type dans l'enum //Retirer le - 1 vérifier que cela fonctionne\\
                {
                    correctionType(EnumType[i], out EnumType[i]); // corrige les enums au cas ou

                    if (EnumType[i] == correctedType) // si l'enum est égale au type
                    {
                        lesInfos.TypeGeneral = (InfoGlobalExel.Type)i; // set le type général
                        break; // break le for
                    }
                }
                if (correctedType == InfoGlobalExel.Type.Nourriture.ToString()) // si c'est de la bouffe
                {
                    lesInfos.exelNourriturre = new InfoExelNourriturre(); // je créer une type bouffe
                    FindObjectInfo(ID, out lesInfos.exelNourriturre);
                }
                else if (correctedType == InfoGlobalExel.Type.Soins.ToString())
                {
                    lesInfos.exelSoins = new InfoExelSoins();
                    FindObjectInfo(ID, out lesInfos.exelSoins);
                }
                else if (correctedType == InfoGlobalExel.Type.Materials.ToString())
                {
                    //print("pas d'action pour le moment");


                    /*lesInfos.exelMaterial = new InfoExelMaterial();
                   
                        foreach (InfoMateriaux SubInfo in MesListe.LesMat)
                        {
                            if (SubInfo.Name == name)
                            {

                            lesInfos.exelMaterial.MonSousType = (InfoExelMaterial.SousType)System.Enum.Parse(typeof(InfoExelMaterial.SousType), SubInfo.Type) ;
                            lesInfos.exelMaterial.value = SubInfo.value;

                            }
                    }*/
                }
                else if (correctedType == InfoGlobalExel.Type.ArmeAfeu.ToString())
                {

                    lesInfos.exelGun = new InfoExelGun();
                    FindObjectInfo(ID, out lesInfos.exelGun);
                }
                else if (correctedType == InfoGlobalExel.Type.ArmeMelee.ToString())
                {
                    lesInfos.exelArme = new InfoExelArme();
                    FindObjectInfo(ID, out lesInfos.exelArme);
                }
                else if (correctedType == InfoGlobalExel.Type.Vetement.ToString())
                {
                    lesInfos.Exelvetements = new InfoExelvetements();
                    FindObjectInfo(ID, out lesInfos.Exelvetements);


                }
                else if (correctedType == InfoGlobalExel.Type.Sac.ToString())
                {
                    lesInfos.ExelSac = new InfoExelSac();
                    FindObjectInfo(ID, out lesInfos.ExelSac);

                }
                else
                {
                    //print("je fais rien");

                }

                if (IsItInThisPage(PageExel.TypeDePageExel.Craft, info.ID))
                {
                    FindObjectInfo(ID, out lesInfos.exelCraft);
                }

                break; // j'ai trouvé ma cible je peux casser
            }
        }
        if (lesInfos.ID == 0) // si l'objet n'est pas dans la base de donner
        {
            //print("l'item n'est pas dans la base de donnée");
            //Debug.Break();

        }
        InfoGlobal = lesInfos;
    }// trouve des infos selon le nom de l'objet
    public void FindObjectInfo(int ID, out InfoExelNourriturre NourritureInfo) 
    {
        //print("FindObjectInfo name out InfoExelNourriturre");
        NourritureInfo = new InfoExelNourriturre();
        bool found = false;
        for (int i = 0; i < MesListe.LaBouffe.Length; i++)
        {
            if (ID == MesListe.LaBouffe[i].ID)
            {
                found = true;
                NourritureInfo.Nourritture = MesListe.LaBouffe[i].Nutrition;
                NourritureInfo.Eau = MesListe.LaBouffe[i].Eau;
                NourritureInfo.Chaleur = MesListe.LaBouffe[i].Chaleur;
                NourritureInfo.Vie = MesListe.LaBouffe[i].Vie;
                NourritureInfo.resume =new Vector4 (MesListe.LaBouffe[i].Nutrition, MesListe.LaBouffe[i].Eau, MesListe.LaBouffe[i].Chaleur, MesListe.LaBouffe[i].Vie);
            }
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }
    }//récupérre les données de la bouffe;
    public void FindObjectInfo(int ID, out InfoExelSoins SoinsInfo)
    {
        //print("FindObjectInfo name out InfoExelSoins");
        SoinsInfo = new InfoExelSoins();
        bool found = false;
        for (int i = 0; i < MesListe.LesSoins.Length ; i++)
        {
            if (ID == MesListe.LesSoins[i].ID)
            {
                found = true;
                SoinsInfo.value = MesListe.LesSoins[i].value;
            }
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }
    }//récupérre les données des soins;
    public void FindObjectInfo(int ID, out InfoExelCraft CraftInfo)
    {
        //print("FindObjectInfo name out InfoExelCraft");
        CraftInfo = new InfoExelCraft();
        bool found = false;
        for (int i = 0; i < MesListe.LesCrafts.Length ; i++)
        {
            if (ID == MesListe.LesCrafts[i].ID)
            {
                found = true;
                int[] LesID = new int [MesListe.LesCrafts[i].RessourcesNecessaire.Length];
                
                for (int j = 0; j < MesListe.LesCrafts[i].RessourcesNecessaire.Length; j++)
                {
                     findObjectIDByName(MesListe.LesCrafts[i].RessourcesNecessaire[j], out LesID[j]);
                }
                CraftInfo.IDdesRessourcesNecessaire = LesID;
                CraftInfo.LeNombreNecessaire= MesListe.LesCrafts[i].LeNombreNecessaire;
                CraftInfo.NomdesRessourcesNecessaire = MesListe.LesCrafts[i].RessourcesNecessaire;
                CraftInfo.CraftingLevel = MesListe.LesCrafts[i].CraftingLevel;
                break;
            }
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }
    }//récupérre les données des craft;
    public void FindObjectInfo(int ID, out InfoExelArme ArmeInfo) // récupére les données des armes de mélée
    {
        ArmeInfo = new InfoExelArme();
        bool found = false;
        foreach (InfoWeapon arme in MesListe.LesArmes)
        {
            if (ID == arme.ID)
            {
                found = true;
                ArmeInfo.Degat = arme.degat;
                ArmeInfo.Speed = arme.Speed;

            }
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }

    
    }
    public void FindObjectInfo(int ID, out InfoExelGun GunInfo)
    {
        GunInfo = new InfoExelGun();
        bool found = false;
        foreach (InfoGun item in MesListe.LesFlingues)
        {
            if (item.ID == ID)
            {
                GunInfo.Chargeur = item.Chargeur;
                GunInfo.Degat = item.degat;
                GunInfo.Power = item.Power;
                GunInfo.Rechargement = item.Rechargement;
                GunInfo.Speed = item.Speed;
            }
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }

    } // récupère les données des gun
    public void FindObjectInfo(int ID, out InfoExelSac SacInfo)
    {
        SacInfo = new InfoExelSac();
        bool found = false;
        foreach (InfoSac item in MesListe.LesSacs)
        {
            if (item.ID == ID)
            {
                SacInfo.NbrEmplacement = item.NbrEmplacement;
                found = true;
            }


        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }
    } // récupére les données du sac
    public void FindObjectInfo(int ID, out InfoExelvetements VetementInfo) 
    {
        VetementInfo = new InfoExelvetements();
        bool found = false;
        foreach (InfoVetements item in MesListe.LesVetements) 
        {
            if (item.ID ==ID)
            {
                VetementInfo.ChaleurResistance = item.ChaleurResistance;
                VetementInfo.DegatResistance = item.DegatResistance;
                string[] EnumTypeVetements = System.Enum.GetNames(typeof(InfoExelvetements.SousCategorie)); // récupére une liste des sous type
                for (int i = 0; i < EnumTypeVetements.Length; i++)
                {
                    correctionType(EnumTypeVetements[i], out EnumTypeVetements[i]);
                    if (EnumTypeVetements[i] == item.Categorie)
                    {

                        VetementInfo.MaCategorie = (InfoExelvetements.SousCategorie)System.Enum.Parse(typeof(InfoExelvetements.SousCategorie), item.Categorie); ;
                        break;
                    }

                }

                found = true;
            }
        
        }
        if (!found)
        {
            //print("Il n'est pas dans les data");
        }

    } // récupére les données des vêtements
    #endregion

    #endregion
    public void findObjectIDByName(string name, out int ID)
    {
        //print("findObjectIDByName ");
        ID = 0;
        foreach (InfoGeneral info in MesListe.LesItems)
        {
            if (name==info.Name)
            {
                ID = info.ID;
            }
        }
        if (ID == 0)
        {
            //print("l'item n'est pas dans la base de donnée");
            //Debug.Break();

        }


    } // cherche le nom selon l'ID
    public void findObjectNameByID(int ID, out string trueName)
    {
        //print("findObjectNameByID ");
        trueName = "";
        foreach (InfoGeneral info in MesListe.LesItems)
        {
            if (ID == info.ID)
            {
                trueName = info.Name;
            }
        }
        if (trueName == "")
        {
            //print("l'item n'est pas dans la base de donnée");
            //Debug.Break();

        }

    } // cherche l'iD selon le nom
    public void findObjectsByType(PageExel.TypeDePageExel mon_type , out int[]ID)
    {
        //print("findObjectsByType");
        List<int> LesID = new List<int>();
        foreach (InfoGeneral info in MesListe.LesItems)
        {
            if (mon_type.ToString()==info.Type)
            {
                LesID.Add(info.ID);
            }
        }
        if (LesID.Count>0)
        {
            ID = LesID.ToArray();
        }
        else 
        {
            ID = new int[0];
            //print("je n'ai pas trouvé l'objet du type" + mon_type.ToString());
            //Debug.Break();
        }
        


    } // trouve les objets du même type
    public void findObjectByChar(char[] Meslettres, out int[] LesID) 
    {
        //print("findObjectByChar");
        List<int> IDFound = new List<int>();
        for (int i = 0; i < MesListe.LesItems.Length-1; i++)
        {
            int NombreDelettreCorrespondante=0;
            foreach (char ThisChar in Meslettres)
            {
                foreach (char charCompare in MesListe.LesItems[i].Name.ToCharArray())
                {
                    if (charCompare == ThisChar)
                    {
                        NombreDelettreCorrespondante++;
                        break;
                    }

                }
            }
            if (NombreDelettreCorrespondante>= Meslettres.Length)
            {
                IDFound.Add(i);
            }

        }
        if (IDFound.Count>0)
        {
            LesID = IDFound.ToArray();
        }
        else 
        {
            //print("Je n'ai pas de résultats");
            LesID = new int[0];
        }
        // prendre la liste compléte
         


    } // trouve un objet par character
    public bool IsItInThisPage(PageExel.TypeDePageExel mon_type, int ID) 
    {
        //print("IsItInPage");
        bool tosend = false;
        if (mon_type == PageExel.TypeDePageExel.General)
        {
            tosend = true;
        }
        else if (mon_type == PageExel.TypeDePageExel.Materiaux)
        {
            foreach (InfoMateriaux item in MesListe.LesMat)
            {
                if (item.ID == ID)
                {
                    tosend = true;
                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.nourriture) 
        {
            foreach (InfoNourriture item in MesListe.LaBouffe)
            {
                if (item.ID == ID)
                {
                    tosend = true;
                }
            }
        } 
        else if (mon_type == PageExel.TypeDePageExel.soins) 
        {
            foreach (InfoSoins item in MesListe.LesSoins)
            {
                if (item.ID == ID)
                {
                    tosend = true;
                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.Craft)
        {
            foreach (InfoCraft item in MesListe.LesCrafts)
            {
                if (item.ID == ID)
                {
                    tosend = true;
                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.SacADos)
        {
            foreach (InfoSac item in MesListe.LesSacs)
            {
                if (item.ID == ID)
                {
                    tosend = true;
                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.Vetements)
        {
            foreach (InfoVetements item in MesListe.LesVetements)
            {
                if (item.ID == ID)
                {

                    tosend = true;

                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.Utilitaire)
        {
            foreach (InfoUtilitaire item in MesListe.LesUtilitaires)
            {
                if (item.ID == ID)
                {
                    tosend = true;
                }
            }
        }
        return tosend;
    
    }// est ce que l'objet est dans cette page
    public bool IsItInThisPage(PageExel.TypeDePageExel mon_type, string name)
    {

        //print("IsItInPage");
        bool tosend = false;
        if (mon_type == PageExel.TypeDePageExel.General)
        {
            tosend = true;
        }
        else if (mon_type == PageExel.TypeDePageExel.Materiaux)
        {
            foreach (InfoMateriaux item in MesListe.LesMat)
            {
                if (item.Name == name)
                {
                    tosend = true;
                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.nourriture)
        {
            foreach (InfoNourriture item in MesListe.LaBouffe)
            {
                if (item.Name == name)
                {
                    tosend = true;
                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.soins)
        {
            foreach (InfoSoins item in MesListe.LesSoins)
            {
                if (item.Name == name)
                {
                    tosend = true;
                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.Craft)
        {
            foreach (InfoCraft item in MesListe.LesCrafts)
            {
                if (item.Name == name)
                {
                    tosend = true;
                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.SacADos)
        {
            foreach (InfoSac item in MesListe.LesSacs)
            {
                if (item.Name == name)
                {
                    tosend = true;
                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.Vetements)
        {
            foreach (InfoVetements item in MesListe.LesVetements)
            {
                if (item.Name == name)
                {

                    tosend = true;

                }
            }
        }
        else if (mon_type == PageExel.TypeDePageExel.Utilitaire)
        {
            foreach (InfoUtilitaire item in MesListe.LesUtilitaires)
            {
                if (item.Name == name)
                {
                    tosend = true;
                }
            }
        }
        return tosend;


    } // est ce que l'objet est dans cette page

    #endregion

}

