using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BotDetection))]

public class FOVEditor : Editor
{
    void OnSceneGUI()
    {
        BotDetection BD = (BotDetection)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(BD.transform.position, Vector3.up, Vector3.forward, 360, BD.viewRadius);
        Vector3 ViewAngleA = BD.DirFromAngle(-BD.viewAngle / 2, false);
        Vector3 ViewAngleB = BD.DirFromAngle(BD.viewAngle / 2, false);
        Handles.DrawLine(BD.transform.position, BD.transform.position + ViewAngleA * BD.viewRadius);
        Handles.DrawLine(BD.transform.position, BD.transform.position + ViewAngleB * BD.viewRadius);
    }
}
