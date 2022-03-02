using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsClass : MonoBehaviour
{
    public int weaponAnimationID;

    [Header("Gathering Attributes")]
    public bool canCutWood;
    public bool canCutStone;

    [Header("General Attributes")]
    public int damage;
    public bool rangedWeapon;
    public float attackDistance;
    public float attackRate;
    public float actualAttackRate = 0;

    [Header("Ranged Weapon Attributes (N'a d'impact que si l'arme est à distance)")]
    public int ammo;
    public int maxAmmo;

    public int reserve;
    public int reloadSpeed;
    public virtual void Shoot(WeaponsClass myWeapon)// Lorsque le joueur tir
    {
        if(rangedWeapon && myWeapon.actualAttackRate <= 0)
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity))
            {
                if(hit.transform.CompareTag("NPC"))
                {
                    hit.transform.GetComponent<AIstats>().ReduceHealth(myWeapon.damage);
                    Instantiate(ItemAssets.ItemAssetsInstance.GetComponent<ParticlesAssets>().particles[1], hit.point, PlayerSingleton.playerInstance.transform.rotation);
                }
            }
            myWeapon.actualAttackRate = myWeapon.attackRate;
        }
        else if(!rangedWeapon && myWeapon.actualAttackRate <= 0)
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, myWeapon.attackDistance))
            {
                if(hit.transform.CompareTag("NPC"))
                {
                    hit.transform.GetComponent<AIstats>().ReduceHealth(myWeapon.damage);
                    Instantiate(ItemAssets.ItemAssetsInstance.GetComponent<ParticlesAssets>().particles[1], hit.point, PlayerSingleton.playerInstance.transform.rotation);
                }
            }
            myWeapon.actualAttackRate = myWeapon.attackRate;
        }
    }

    public virtual void Reload(WeaponsClass myWeapon)// lorsque le joueur recharge
    {
        if(myWeapon.reserve > myWeapon.maxAmmo - myWeapon.ammo)
        {
            myWeapon.ammo = myWeapon.maxAmmo - myWeapon.ammo;
            myWeapon.reserve -= myWeapon.maxAmmo - myWeapon.ammo;
        }
        else
        {
            myWeapon.ammo += reserve;
        }
    }

    public virtual void attackRateManagement()
    {

    }
}
