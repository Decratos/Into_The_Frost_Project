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
        Debug.Break();
    }

    // Update is called once per frame
    void objetToLook() 
    {
        InfoGlobalExel test = new InfoGlobalExel();
        InfoExelCuisine test2 = new InfoExelCuisine();
        Data.FindObjectInfo(id,out test);
        Data.FindObjectInfo(id, out test2);
        if (test.ID!=0)
        {
            print("Il trouve dans global");
        }
        else
        {
            print("pas trouvé");
        }
        if (test2.TempsDeCuisson!=0)
        {
            print(test2.TempsDeCuisson);
            foreach (int id in test2.IDOfResult)
            {
                print(id);
            }
            foreach(string names in test2.NameOfResult) 
            {
                print(names);
            }
        }
        else 
        {
            print("pas trouvé");
        }
    }
}
