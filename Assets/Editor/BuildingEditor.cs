using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BuildingPart))]
public class BuildingEditor : Editor
{
    void OnSceneGUI()
    {
        BuildingPart bp = (BuildingPart)target;
    }
}