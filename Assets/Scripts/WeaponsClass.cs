using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsClass : MonoBehaviour
{

    [Header("Gathering Attributes")]
    public bool canCutWood;
    public bool canCutStone;

    [Header("General Attributes")]
    public int damage;
    public float attackSpeed;
    public bool rangedWeapon;
    public float attackDistance;
    public float attackRate;

    [Header("Ranged Weapon Attributes"), Tooltip("N'a d'impact que si l'arme est à distance")]
    public int ammo;
    public int maxAmmo;
    public int reloadSpeed;
    public virtual void Shoot()
    {
        if(rangedWeapon)
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                if(hit.transform.CompareTag("NPC"))
                {

                }
            }
        }
        else
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attackDistance))
            {
                if(hit.transform.CompareTag("NPC"))
                {
                    
                }
            }
        }
    }

    public virtual void Reload()
    {
        
    }
}
