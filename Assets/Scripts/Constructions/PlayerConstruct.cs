using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConstruct : MonoBehaviour
{

    public bool constructionMode;
    public GameObject selectedStructure;
    private StructureList _structureList;
    public int listID = 0;

    public GameObject buildingPrevisualizationObject;
    public float buildMaxDistance;


    private void Start()
    {
        GetComponent<GestionDesScipt>().PlayerConstruct = this;
        _structureList = StructureList.structureInstance;
        if(_structureList == null)
        {
            print("Pas de liste de structure");
        }
    }

    public void ChangeBuildMode(bool active)
    {
        if (active)
        {
            buildingPrevisualizationObject.SetActive(true);
        }
        else
        {
            buildingPrevisualizationObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (constructionMode)
        {
            if(Physics.Raycast(transform.position,Camera.main.transform.forward, out RaycastHit hit , Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Construction") && Vector3.Distance(hit.transform.position, transform.position) <= buildMaxDistance)
                {
                    print(hit.transform);
                    Transform nearestAnchorPoint = null;
                    Transform nearestPrevizuAnchorPoint = null;
                    float distance = buildMaxDistance;
                    for (int i = 0; i<hit.transform.GetChild(0).childCount; i++)
                    {
                        if (Vector3.Distance(hit.point, hit.transform.GetChild(0).GetChild(i).transform.position) < distance)
                        {
                            nearestAnchorPoint = hit.transform.GetChild(0).GetChild(i);
                            distance = Vector3.Distance(hit.point, hit.transform.GetChild(0).GetChild(i).transform.position);
                        }
                    }
                    distance = 10000;
                    for (int y = 0; y < selectedStructure.transform.GetChild(0).childCount; y++)
                    {
                        if (Vector3.Distance(nearestAnchorPoint.position, selectedStructure.transform.GetChild(0).GetChild(y).transform.position) < distance)
                        {
                            nearestPrevizuAnchorPoint = selectedStructure.transform.GetChild(0).GetChild(y);
                            distance = Vector3.Distance(nearestAnchorPoint.position, selectedStructure.transform.GetChild(0).GetChild(y).transform.position);
                        }
                    }
                    Vector3 direction = (hit.transform.position - nearestAnchorPoint.transform.position).normalized;
                    float distanceAB = Vector3.Distance(selectedStructure.transform.position, nearestPrevizuAnchorPoint.position);
                    buildingPrevisualizationObject.transform.GetChild(0).rotation = Quaternion.LookRotation(nearestAnchorPoint.transform.forward);
                    buildingPrevisualizationObject.transform.GetChild(0).position = nearestAnchorPoint.transform.position - direction * (distanceAB);
                }
                else
                {
                    if (hit.point != Vector3.zero && Vector3.Distance(hit.point, transform.position) < buildMaxDistance)
                        buildingPrevisualizationObject.transform.position = hit.point;
                    else
                    {
                        buildingPrevisualizationObject.transform.position =
                            (transform.position + transform.forward * buildMaxDistance) - new Vector3(0, transform.localScale.y,0);
                    }
                }
            }
            
        }
    }

    public void Construct()
    {
        int price = _structureList.structuresPrice[listID];
        if (GetComponent<GestionDesScipt>().Inventory
            .CheckItemOnList(new ItemClass {itemName = "Planche"}) >= price)
        {
            GameObject structure = Instantiate(selectedStructure, buildingPrevisualizationObject.transform.GetChild(0).position, buildingPrevisualizationObject.transform.GetChild(0).rotation);
            GetComponent<GestionDesScipt>().Inventory.RemoveItem(new ItemClass {itemName = "Planche", amount = price});
            
            if(structure.GetComponentsInChildren<Collider>().Length > 0){
                foreach (var col in  structure.GetComponentsInChildren<Collider>())
                {
                   col.enabled = true;
                }
            }
            else if(structure.GetComponent<Collider>())
                structure.GetComponent<Collider>().enabled = true;
        }
        else
        {
            print(GetComponent<GestionDesScipt>().Inventory
            .CheckItemOnList(new ItemClass {itemName = "Planche"}));
        }
            
    }

    public void increaseIdStructure()
    {
        listID++;
        if (listID > _structureList.structures.Count-1)
        {
            listID = 0;
        }

        selectedStructure = _structureList.structures[listID];
        buildingPrevisualizationObject.GetComponent<ConstructionPrevisualization>().UpdateObjectPrevisualization(selectedStructure);
    }

    public void decreaseIdStructure()
    {
        listID--;
        if(listID < 0)
        {
            listID = _structureList.structures.Count;
        }
        selectedStructure = _structureList.structures[listID];
    }
}
