using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct loot
{
    public int ID;
    public string Name;
    public InfoGlobalExel.Type LeType;
    public float ChanceDeBase;
    public float ChanceChoisis;
    public Vector3 rotationSpawn;
}

public class LootingByEspace : MesFonctions
{
   
    public enum methodChoisis
    {
        Script,
        Tag
    }
    public methodChoisis LaMethod;
    //public Looter script;
    public string LeTagEnfant;
    public string LeTagSpawner;

    public bool HasAlreadyLooted;

    public loot[] lesLootPossible;

    [HideInInspector] public RaycastHit[] LesObjets;

    Vector3 rotationAuSpawn;

    private void Start()
    {
        LanceLeLooting();
    }

    public void LanceLeLooting() 
    {
        if (!HasAlreadyLooted)
        {
            BoxCollider LeBox = new BoxCollider(); 
            if (GetChildCountOfObject(this.transform) == 1) 
            {
                LeBox = transform.GetChild(0).GetComponent<BoxCollider>();// permet de récupérer le component box collider
            }
            else if (GetChildCountOfObject(this.transform) > 1) // au cas où l'objet à plus qu'un seul gosse
            {
                foreach (Transform item in this.transform) // pour chaque enfant contenu dans ce transform
                {
                    if (LeTagEnfant == item.tag) // si l'objet à le bon tag
                    {
                        LeBox = item.GetComponent<BoxCollider>(); // permet de récupérer le component box collider
                    }
                }
            }
            LesObjets = Physics.BoxCastAll(transform.position + LeBox.center, LeBox.size / 2, Vector3.down, LeBox.transform.rotation, LeBox.size.z); // fait un box cast all pour trouver toute les spawner
            ChooseMethod();
        } // Méthode qui permet de set up le spawn d'objet 
        
    }

    void ChooseMethod() 
    {
        
        if (LaMethod==methodChoisis.Script) // si la method est via script
        {
            recuperationSpawnViaScript(); // lance la méthode script
            print("je choisis la méthode script");
        }
        else if (LaMethod == methodChoisis.Tag) // Si la method est via tag
        {
            print("je choisis la méthode tag");
            recuperationSpawnViaTag(); //lance la méthode via tag
        }
        
    } // Choisis par quel procédé on récupére les spawners

    void recuperationSpawnViaScript() 
    {
        List<GameObject> SpawnPoints = new List<GameObject>();// liste des spawn
        foreach (RaycastHit item in LesObjets) // pour chaque  raycast hit dans les objets
        {
            if (item.transform.GetComponent<Looter>()) // si le script est le bon
            {
                print("j'ajoute des trucs depuis script");
                SpawnPoints.Add(item.transform.gameObject); // ajoute le spawnPoint
            }
        }
        CreateObject(SpawnPoints.ToArray());// creer les objets dans l'array
    }
    void recuperationSpawnViaTag() 
    {
        List<GameObject> SpawnPoints = new List<GameObject>(); // liste des spawn
        foreach (RaycastHit item in LesObjets) // pour chaque  raycast hit dans les objets
        {
            if (item.transform.tag == LeTagSpawner) // si le tag est le bon
            {
                print("j'ajoute des trucs depuis tag");
                SpawnPoints.Add(item.transform.gameObject); // ajoute le spawnPoint
            }
        }
        CreateObject(SpawnPoints.ToArray());// creer les objets dans l'array
    }
    
    void CreateObject(GameObject[] LesObjects) 
    {
        print("j'aurais du faire des trucs");
        
        foreach (GameObject item in LesObjects)//Pour chaque spawner
        {
            InfoGlobalExel toSpawn = ChooseItemToSpawn(); // Choisis l'objet à spawn
            item.GetComponent<Looter>().InstantiateObject(toSpawn,Quaternion.Euler(rotationAuSpawn));
            //appel la fonction 
            

        }
    


    }

    InfoGlobalExel ChooseItemToSpawn() 
    {
        InfoGlobalExel ToReturn = new InfoGlobalExel(); // créer la valeur a retourner
        int IDObjectChoose=0; 
        float AllChances=0;
       
        float[,] arrayChanceTrier = new float[lesLootPossible.Length, 2]; // [Le nombre d'array, Le nombre qu'ils contiennent]
        float ChanceMini = 0; //

        foreach (loot item in lesLootPossible) 
        {
            
            if (item.ChanceChoisis!=0f)
            {
                AllChances += item.ChanceChoisis;
                
            }
            else 
            {
                AllChances += item.ChanceDeBase;
                
            }

        } // additionne toutes les chances
        float random = Random.Range(0, AllChances); // fait un nombre random
        //A REVOIR COMPLET
        for (int i = 0; i < lesLootPossible.Length; i++) 
        {
            float chanceWinner = 0;
            float chanceActuel = 0;
            for (int j = 0; j < lesLootPossible.Length; j++)
            {
                // regarde la chance de spawn de l'objet
                if (lesLootPossible[j].ChanceChoisis!=0)
                {
                    chanceActuel = lesLootPossible[j].ChanceDeBase;
                }
                else 
                {
                    chanceActuel = lesLootPossible[j].ChanceChoisis;
                }
                //set la chance actuel

                if (chanceActuel > chanceWinner &&(i==0 || chanceWinner < arrayChanceTrier[i-1, 1] ))// si la chance actuel est supérieur à la chance winner et la chance winner est inférieur à la chance trier d'avant 
                {
                    // doit donner un truc du genre [0.5,0.4,0.3] // quoi que
                    chanceWinner = chanceActuel;
                }
            }
            arrayChanceTrier[i, 0] = lesLootPossible[i].ID;
            arrayChanceTrier[i, 1] = chanceActuel;
        }// trie les chances de la plus grande à la plus petite
        for (int i = 0; i < lesLootPossible.Length; i++)
        {
            ChanceMini+= arrayChanceTrier[i, 1];
            if (ChanceMini>random) // possiblement un problème ici 
            {
                liseurExel.LesDatas.FindObjectInfo(lesLootPossible[(int)arrayChanceTrier[i, 0]].ID,out ToReturn);
                rotationAuSpawn = lesLootPossible[(int)arrayChanceTrier[i, 0]].rotationSpawn;
                break;
            }
        }//choisis l'objet à spawn
        //

        liseurExel.LesDatas.FindObjectInfo(IDObjectChoose, out ToReturn); // tojours égale 0 prblm
        return ToReturn; // prblm
    }// à modifier //!\\
    
}
