using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private bool weaponModeIsRanged = false;
    public List<WeaponsClass> Weapons;
    public WeaponsClass actualMeleeWeapon;
    public WeaponsClass actualRangedWeapon;

    public void Attack()
    {
        if (!weaponModeIsRanged)
            actualMeleeWeapon.Shoot();
        
        else
        {
            actualRangedWeapon.Shoot();
        }
    }

    public void ChangeWeaponMode()
    {
        weaponModeIsRanged = !weaponModeIsRanged;

        if (!weaponModeIsRanged)
        {
            print("Combat de melée");
            actualMeleeWeapon.gameObject.SetActive(true);
            actualRangedWeapon.gameObject.SetActive(false);
        }
        else
        {
            print("Combat à distance");
            actualMeleeWeapon.gameObject.SetActive(false);
            actualRangedWeapon.gameObject.SetActive(true);
        }
    }

    public void ActiveWeapon(WeaponsClass newWeapon, bool ranged)
    {
        if (ranged)
        {
            if(actualRangedWeapon)
                actualRangedWeapon.gameObject.SetActive(false);
            actualRangedWeapon = newWeapon;
            actualRangedWeapon.gameObject.SetActive(true);
        }
        else
        {
            if(actualMeleeWeapon)
                actualMeleeWeapon.gameObject.SetActive(false);
            actualMeleeWeapon = newWeapon;
            actualMeleeWeapon.gameObject.SetActive(true);
        }
    }

    public void DesactiveWeapon(bool ranged) {
        if (ranged)
        {
            if(actualRangedWeapon)
            {
                actualRangedWeapon.gameObject.SetActive(false);
                actualRangedWeapon = null;
            }
                
        }
        else
        {
            if(actualMeleeWeapon)
            {
                actualMeleeWeapon.gameObject.SetActive(false);
                actualMeleeWeapon = null;
            }
        }
    }
}
