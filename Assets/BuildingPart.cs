using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPart : MonoBehaviour
{
    public bool hasWindow;
    public bool hasDoor;
    public bool isExternWall;
    public int ID;

    private GameObject wallGenerated;
    public void GenerateWall(BuildingGenerator bg, int wallNumbers, int seed)
    {
        if(ID == 0)
        {
            ID = Random.Range(0, 100);
        }
        foreach(Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
        GameObject wallToGenerate = null;
        if(isExternWall)
        {
            if (hasWindow)
            {
                wallToGenerate = bg.windowWall;
            }
            else if (hasDoor)
            {
                wallToGenerate = bg.doorWall;
            }
            else
            {
                wallToGenerate = bg.wall;
            }
        }
        else
        {
            int tempID = ID * seed / wallNumbers;
            if((seed + tempID) % 2 > 0)
            {
                if (hasDoor)
                {
                    wallToGenerate = bg.doorWall;
                }
                else
                {
                    wallToGenerate = bg.wall;
                }
            }
        }
        if(wallToGenerate != null)
        {
            wallGenerated = wallToGenerate;
            var wall = Instantiate(wallToGenerate, transform.position, transform.rotation);
            wall.transform.SetParent(this.transform);
        }
        

    }

    public void Clean()
    {
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}
