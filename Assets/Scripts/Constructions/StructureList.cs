using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureList : MonoBehaviour
{

    public static StructureList structureInstance;

    public List<GameObject> structures;
    
    public List<int> structuresPrice;
    // Start is called before the first frame update
    void Awake()
    {
        if (structureInstance != null)
        {
            Destroy(this);
        }
        else
        {
            structureInstance = this;
        }
    }
}
