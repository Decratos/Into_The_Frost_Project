using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testData : MesFonctions
{
    [SerializeField] int id;
    [SerializeField] string name;
    liseurExel Data;
    void Start()
    {
        Data = GetComponent<liseurExel>();
        objetToLook();
    }

    // Update is called once per frame
    void objetToLook() 
    {
        InfoGlobalExel test = new InfoGlobalExel();
        InfoExelGun test2 = new InfoExelGun();
        Data.FindObjectInfo(name,out test);
        Data.FindObjectInfo(name, out test2);
        if (test2.Chargeur!=0)
        {
            print("mon chargeur" +test2.Chargeur);
        }
        if (test.ID !=0)
        {
            print("je l'ai trouvé" + test.Name);
        }
        else 
        {
            print("pas trouvé");
        }
    }
}
