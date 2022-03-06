using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemWorldSpawner : MonoBehaviour
{

    public ItemClass item;
    [SerializeField] private ResumeExelForObject.Type type;
    [SerializeField] private float chanceForNoSpawn;
    public List<ItemClass> lesLootPossible;
    public bool inDebug = false;

    // Start is called before the first frame update
    private void Start()
    {
        if(!inDebug)
        {
            if (CheckNoSpawnParameter())
            {
                InitSpawnMecanism();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        
    }

    [Button]
    private void InitSpawnMecanism()
    {
        lesLootPossible.Clear();
        liseurExel excelLiseur;
        MesFonctions.FindDataExelForObject(out excelLiseur);
        GetAllItemPossible();
        item = ChooseItemToSpawn();
        excelLiseur.findObjectIDByName(item.globalInfo.Name, out item.globalInfo.ID);
        var myItem = ItemWorld.SpawnItemWorld(item, transform.position);
        myItem.transform.parent = transform.parent;
        if(!inDebug)
            Destroy(this.gameObject);
    }
    

    private ItemClass ChooseAnItem()
    {
        bool hasFindItem = false;
        int ID = 0;
        string _name = "";
        InfoGlobalExel _info = new InfoGlobalExel();
        while (!hasFindItem)
        {
            ID = Random.Range(0, liseurExel.LesDatas.MesListe.LesItems.Length);
            liseurExel.LesDatas.findObjectNameByID(ID, out _name);
            liseurExel.LesDatas.FindObjectInfo(_name, out _info);
            print(_info.TypeGeneral.ToString() + " " + _info.Name);

            if (_info.TypeGeneral.ToString() == type.ToString())
            {
                item = new ItemClass {globalInfo = _info};
                hasFindItem = true;
            }
            
        }
        return item;
    }
    private bool CheckNoSpawnParameter()
    {
        float A = Random.Range(0f, 1f);
        if (A <= chanceForNoSpawn)
        {
            return false;
            print("Pas de spawn");
        }
        else
        {
            print("Spawn possible");
            return true;
        }
            
    }

    private void GetAllItemPossible()
    {
        PageExel.TypeDePageExel mon_type = PageExel.TypeDePageExel.General;
        switch (type)
        {
            case ResumeExelForObject.Type.Nourriture:
                mon_type = PageExel.TypeDePageExel.Nourriture;
                break;
            case ResumeExelForObject.Type.Materials:
                mon_type = PageExel.TypeDePageExel.Materials;
                break;
            case ResumeExelForObject.Type.ArmeAfeu:
                mon_type = PageExel.TypeDePageExel.ArmeAfeu;
                break;
            case ResumeExelForObject.Type.ArmeMelee:
                mon_type = PageExel.TypeDePageExel.ArmeMelee;
                break;
            case ResumeExelForObject.Type.Sac:
                mon_type = PageExel.TypeDePageExel.Sac;
                break;
            case ResumeExelForObject.Type.Soins:
                mon_type = PageExel.TypeDePageExel.Soins;
                break;
            case ResumeExelForObject.Type.Utilitaire:
                mon_type = PageExel.TypeDePageExel.Utilitaire;
                break;
            case ResumeExelForObject.Type.Vetements:
                mon_type = PageExel.TypeDePageExel.Vetements;
                break;
        }
        InfoGlobalExel ToReturn = new InfoGlobalExel();
        int[] itemsID;
        liseurExel.LesDatas.findObjectsByType(mon_type,out itemsID);
        foreach(int i in itemsID)
        {
            liseurExel.LesDatas.FindObjectInfo(i, out ToReturn);
            lesLootPossible.Add(new ItemClass {globalInfo = ToReturn, amount = 1, ChanceDeBase = ToReturn.rarity });
        }
    }

    ItemClass ChooseItemToSpawn()
    {
        InfoGlobalExel ToReturn = new InfoGlobalExel(); // créer la valeur a retourner
        int IDObjectChoose = 0;
        float AllChances = 0;
        float[,] arrayChanceTrier = new float[lesLootPossible.Count, 3]; // [Le nombre d'array, Le nombre qu'ils contiennent]

        while(IDObjectChoose == 0)
        {
               foreach (ItemClass item in lesLootPossible)
            {

                if (item.ChanceChoisis != 0f)
                {
                    AllChances += item.ChanceChoisis;

                }
                else
                {
                    AllChances += item.ChanceDeBase;

                }

            } // additionne toutes les chances
            float random = Random.Range(0, AllChances); // fait un nombre random
            //print("Voici mon random" + random);

            float cumulOrdonne = 0;

            float LaChanceChoisi = 0;

            for (int i = 0; i < lesLootPossible.Count; i++)
            {
                float LaChanceSuperieur = 0;
                for (int j = 0; j < lesLootPossible.Count; j++)
                {

                    if (lesLootPossible[j].ChanceChoisis != 0)
                    {
                        LaChanceChoisi = lesLootPossible[j].ChanceChoisis;
                    }
                    else
                    {
                        LaChanceChoisi = lesLootPossible[j].ChanceDeBase;
                    }

                    if (LaChanceChoisi > LaChanceSuperieur && (i == 0 || LaChanceChoisi < arrayChanceTrier[i - 1, 1]))
                    {
                        LaChanceSuperieur = LaChanceChoisi;
                        arrayChanceTrier[i, 2] = lesLootPossible[j].globalInfo.ID;
                    }
                }

                arrayChanceTrier[i, 1] = LaChanceSuperieur;

                cumulOrdonne += arrayChanceTrier[i, 1];

                if (random < cumulOrdonne)
                {
                    IDObjectChoose = (int)arrayChanceTrier[i, 2];
                    break;
                }// choisis l'objet a spawn
            } // trie les chances dans l'ordre
        }
        


        liseurExel.LesDatas.FindObjectInfo(IDObjectChoose, out ToReturn);
        //print("Id choisi : " + IDObjectChoose);
        var finalItem = new ItemClass {globalInfo = ToReturn, amount = 1, ChanceDeBase = ToReturn.rarity };
        return finalItem;
    }
}
