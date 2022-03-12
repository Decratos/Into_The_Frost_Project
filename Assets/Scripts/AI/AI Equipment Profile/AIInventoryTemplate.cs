using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AiEquipmentProfile", menuName = "Into The Frost/AiEquipmentProfile", order = 1)]
public class AIInventoryTemplate : ScriptableObject
{
    public string weapon;
    public string backpack;
    public string medicalSupply;
}
