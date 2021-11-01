using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looter : MesFonctions
{
    [SerializeField] string Path;
    public void InstantiateObject(InfoGlobalExel Info,Quaternion rotationSpawn) 
    {
        GameObject tospawn = Resources.Load<GameObject>(Path);
        GameObject toInstantiate=Instantiate(tospawn,transform.position,rotationSpawn);
        toInstantiate.AddComponent<ResumeExelForObject>();

    } 

}
