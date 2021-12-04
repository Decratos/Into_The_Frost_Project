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

    public RaycastHit[] LesObjets;

    Vector3 rotationAuSpawn;
    
    public void LanceLeLooting() 
    {
        if (!HasAlreadyLooted)
        {
            BoxCollider LeBox = new BoxCollider();
            if (GetChildCountOfObject(this.transform) == 1)
            {
                LeBox = transform.GetChild(0).GetComponent<BoxCollider>();
            }
            else if (GetChildCountOfObject(this.transform) > 1)
            {
                foreach (Transform item in this.transform)
                {
                    if (LeTagEnfant == item.tag)
                    {
                        LeBox = item.GetComponent<BoxCollider>();
                    }
                }
            }
            LesObjets = Physics.BoxCastAll(transform.position + LeBox.center, LeBox.size / 2, Vector3.down, LeBox.transform.rotation, LeBox.size.z);

            ChooseMethod();
        }
        
    }

    void ChooseMethod()
    {
        List<GameObject> SpawnPoints = new List<GameObject>();
        if (LaMethod==methodChoisis.Script)
        {
            recuperationSpawnViaScript();
        }
        else if (LaMethod == methodChoisis.Tag)
        {
            recuperationSpawnViaTag();
        }
        
    }

    void recuperationSpawnViaScript() 
    {
        List<GameObject> SpawnPoints = new List<GameObject>();
        foreach (RaycastHit item in LesObjets)
        {
            if (item.transform.GetComponent<Looter>())
            {
                SpawnPoints.Add(item.transform.gameObject);
            }
        }
        CreateObject(SpawnPoints.ToArray());
    }
    void recuperationSpawnViaTag() 
    {
        List<GameObject> SpawnPoints = new List<GameObject>();
        foreach (RaycastHit item in LesObjets)
        {
            if (item.transform.tag == LeTagSpawner)
            {
                SpawnPoints.Add(item.transform.gameObject);
            }
        }
        CreateObject(SpawnPoints.ToArray());
    }
    
    void CreateObject(GameObject[] LesObjects) 
    {
        foreach (GameObject item in LesObjects)//Pour chaque spawner
        {
            InfoGlobalExel toSpawn = ChooseItemToSpawn(); // Choisis 
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
        float ChanceMini = 0;

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

        }
        float random = Random.Range(0, AllChances);
        for (int i = 0; i < lesLootPossible.Length; i++)
        {
            float chanceWinner = 0;
            float chanceActuel = 0;
            for (int j = 0; j < lesLootPossible.Length; j++)
            {
                
                if (lesLootPossible[j].ChanceChoisis!=0)
                {
                    chanceActuel = lesLootPossible[j].ChanceDeBase;
                }
                else 
                {
                    chanceActuel = lesLootPossible[j].ChanceChoisis;
                }


                if (chanceActuel > chanceWinner &&(i==0 || chanceWinner < arrayChanceTrier[i-1, 1] ))
                {
                    chanceWinner = chanceActuel;
                }
            }
            arrayChanceTrier[i, 0] = lesLootPossible[i].ID;
            arrayChanceTrier[i, 1] = chanceActuel;
        }// trie les chances de la plus grande à la plus petite

        for (int i = 0; i < lesLootPossible.Length; i++)
        {
            ChanceMini+= arrayChanceTrier[i, 1];
            if (ChanceMini>random)
            {
                liseurExel.LesDatas.FindObjectInfo(lesLootPossible[(int)arrayChanceTrier[i, 0]].ID,out ToReturn);
                rotationAuSpawn = lesLootPossible[(int)arrayChanceTrier[i, 0]].rotationSpawn;
                break;
            }
        }


        liseurExel.LesDatas.FindObjectInfo(IDObjectChoose, out ToReturn);
        return ToReturn;
    }
    
}
