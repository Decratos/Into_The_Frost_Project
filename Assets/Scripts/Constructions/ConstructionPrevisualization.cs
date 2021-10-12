using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionPrevisualization : MonoBehaviour
{
    public GameObject objectToPrevisualize;

    public void Start()
    {
        var obj = Instantiate(objectToPrevisualize, this.transform);
        obj.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.2f);
        obj.GetComponent<Collider>().enabled = false;
    }

    public void UpdateObjectPrevisualization(GameObject newObject)
    {
        Destroy(transform.GetChild(0).gameObject);
        objectToPrevisualize = newObject;
        var obj = Instantiate(objectToPrevisualize, this.transform);
        if(obj.GetComponent<Renderer>())
        {
            obj.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.2f);
            obj.GetComponent<Collider>().enabled = false;
        }
        else if(obj.GetComponentInChildren<Renderer>())
        {
            obj.GetComponentInChildren<Renderer>().material.color = new Color(0, 1, 0, 0.2f);
            obj.GetComponentInChildren<Collider>().enabled = false;
        }
        
    }
}
