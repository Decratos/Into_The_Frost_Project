using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditEquipmentLevel : MonoBehaviour
{
    public enum Levels
    {
        Light,
        Moderate,
        Heavy
    }
    public Levels equipmentLevel;
    [Header("Valeurs de base")]
    public float baseLife;
    public float baseAttackDistance;
    public float baseDamage;

    [Header("Equipment Stats (Ne pas changer)")]
    public float life;
    public float attackDistance;
    public float damage;
    // Start is called before the first frame update
    void Awake()
    {
        switch(equipmentLevel)
        {
            case Levels.Light:
                life = baseLife;
                attackDistance = baseAttackDistance;
                damage = baseDamage;
                break;
            case Levels.Moderate:
                life = baseLife * 1.5f;
                attackDistance = baseAttackDistance * 10;
                damage = baseDamage * 5f;
                break;
            case Levels.Heavy:
                life = baseLife * 2.25f;
                attackDistance = baseAttackDistance * 20;
                damage = baseDamage * 10;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
