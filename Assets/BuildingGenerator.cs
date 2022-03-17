using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BuildingGenerator : MonoBehaviour
{
    public GameObject wall;
    public GameObject windowWall;
    public GameObject doorWall;
    [SerializeField] private int customSeed;
    private int seed;
    public GameObject[] floors;

    [Button("Generate Walls")]
    private void GenerateWalls()
    {
        if(customSeed != 0)
        {
            seed = customSeed;
        }
        else
        {
            seed = Random.Range(0, 400);
        }
        int wallnum = 0;
        foreach(GameObject floor in floors)
        {
            foreach (Transform child in floor.transform)
            {
                if (child.GetComponent<BuildingPart>())
                {
                    wallnum++;
                    child.GetComponent<BuildingPart>().GenerateWall(this, wallnum, seed);
                }
            }
                
        }
    }

    [Button("Clean Walls")]
    private void CleanWalls()
    {
        foreach (GameObject floor in floors)
        {
            foreach(Transform child in floor.transform)
            {
                if (child.GetComponent<BuildingPart>())
                    child.GetComponent<BuildingPart>().Clean();
            }
            
        }
    }

    [Button("Save Seed")]
    private void SaveSeed()
    {
        customSeed = seed;
    }

    [Button("Clear Seed")]
    private void ClearSeed()
    {
        customSeed = 0;
    }
}
