using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AiProfil", menuName = "Into The Frost/AiProfil", order = 0)]
public class AiProfil : ScriptableObject
{

    public enum AItype
    {
        HumanNeutral,
        AnimalNeutral,
        AnimalHostile,
        HumanHostile
    }

    public AItype type;
    public float waitTime;

    [HideInInspector]
    public float actualWaitTime;
    public float fleeDistance;
     
    public Transform target;

    public float attackDistance;
    public float attackInterval;
     [HideInInspector]
    public float actualAttackInterval;
}
