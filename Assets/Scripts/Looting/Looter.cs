using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looter : MesFonctions
{
    [SerializeField] string Path;
    public void InstantiateObject(InfoGlobalExel Info,Quaternion rotationSpawn) 
    {
        //GameObject tospawn = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //Path += Info.ID.ToString();
        Path += 1.ToString();
        //print(Path);
        GameObject tospawn = Resources.Load<GameObject>(Path);
        GameObject toInstantiate=Instantiate(tospawn,transform.position,rotationSpawn);
        
        toInstantiate.name = Info.Name;
        //toInstantiate.AddComponent<ResumeExelForObject>();

    } 

}
//ItemWorld itemWorld = itemToInst.GetComponent<ItemWorld>();
//itemWorld.SetItem(item);