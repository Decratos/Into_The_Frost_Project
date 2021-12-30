using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptable/Lighting Preset", order = 1)] 
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;

    public AnimationCurve directionalTemperature;

    public Gradient FogColor;
    public AnimationCurve temperatureByTime;
    
}
