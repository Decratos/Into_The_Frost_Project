using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadExcel : MonoBehaviour
{
    public Item blankitem;
    public List<Item> itemDatabase = new List<Item>();

    public void LoadItemData() 
    {
        itemDatabase.Clear();

        List<Dictionary<string, object>> data = CSVReader.Read("TestUnity");
        for (int i = 0; i < data.Count; i++)
        {
            int id = int.Parse(data[i]["ID"].ToString(), System.Globalization.NumberStyles.Integer);
            string Nom = data[i]["Nom"].ToString();
            int nombre = int.Parse(data[i]["nombre"].ToString(), System.Globalization.NumberStyles.Integer);

            AddItem(id, Nom, nombre);
        }
    
    }

    void AddItem(int ID, string nom, int nombre) 
    {
        Item tempItem = new Item(blankitem);

        tempItem.ID = ID;
        tempItem.Nom = nom;
        tempItem.nombre = nombre;

        itemDatabase.Add(tempItem);
    
    }

}
