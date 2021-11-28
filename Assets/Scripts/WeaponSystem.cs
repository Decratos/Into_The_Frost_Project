using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private bool FirstWeapon = false;
    public List<WeaponsClass> Weapons;

    public WeaponsClass actualWeaponInHands;

    public WeaponsClass EquippedWeapon1;
    public WeaponsClass EquippedWeapon2;

    public void Attack()
    {
        if (!FirstWeapon)
            EquippedWeapon1.Shoot();
        
        else
        {
            EquippedWeapon2.Shoot();
        }
    }

    public void ChangeWeaponMode()
    {
        FirstWeapon = !FirstWeapon;

        if (!FirstWeapon)
        {
            EquippedWeapon1.gameObject.SetActive(true);
            EquippedWeapon2.gameObject.SetActive(false);
            actualWeaponInHands = EquippedWeapon1;
        }
        else
        {
            EquippedWeapon1.gameObject.SetActive(false);
            EquippedWeapon2.gameObject.SetActive(true);
            actualWeaponInHands = EquippedWeapon2;
        }
    }

    public void ActiveWeapon(WeaponsClass newWeapon, int slot)
    {
        if (slot == 1)
        {
            if(EquippedWeapon1)
                EquippedWeapon1.gameObject.SetActive(false);
            EquippedWeapon1 = newWeapon;
            EquippedWeapon1.gameObject.SetActive(true);
        }
        else
        {
            if(EquippedWeapon2)
                EquippedWeapon2.gameObject.SetActive(false);
            EquippedWeapon2 = newWeapon;
            EquippedWeapon2.gameObject.SetActive(true);
        }
    }

    public void DesactiveWeapon(int slot) {
        if (slot == 1)
        {
            if(EquippedWeapon1)
            {
                EquippedWeapon1.gameObject.SetActive(false);
                EquippedWeapon1 = null;
            }
                
        }
        else
        {
            if(EquippedWeapon2)
            {
                EquippedWeapon2.gameObject.SetActive(false);
                EquippedWeapon2 = null;
            }
        }
    }
}
