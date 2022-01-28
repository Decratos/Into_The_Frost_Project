using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawner : MonoBehaviour
{

    public ItemClass item;
    [SerializeField] private ResumeExelForObject.Type type;
    [SerializeField] private float chanceForNoSpawn;
    
    // Start is called before the first frame update
    void Start()
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

    private void InitSpawnMecanism()
    {
        liseurExel excelLiseur;
        MesFonctions.FindDataExelForObject(out excelLiseur);
        item = ChooseAnItem();
        excelLiseur.findObjectIDByName(item.itemName, out item.spriteId);
        var myItem = ItemWorld.SpawnItemWorld(item, transform.position);
        myItem.transform.parent = transform.parent;
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
            ID = Random.Range(0, 79);
            liseurExel.LesDatas.findObjectNameByID(ID, out _name);
            liseurExel.LesDatas.FindObjectInfo(_name, out _info);
            print(_info.TypeGeneral.ToString() + " " + _info.Name);

            if (_info.TypeGeneral.ToString() == type.ToString())
            {
                item = new ItemClass { itemName = _info.Name, globalInfo = _info, itemType = type, spriteId = _info.ID };
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
}
