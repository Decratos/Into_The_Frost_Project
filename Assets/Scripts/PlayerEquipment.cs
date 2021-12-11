using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField]
    private ItemClass rangedWeapon, CaCweapon, helmet;

    [SerializeField] private Sprite defaultRanged, defaultMelee, defaultHelmet;

    public ItemSlot rangedSlot, CaCSlot, helmetSlot;
    
    private void Awake() {
        /*var equipmentHolder = GetComponent<CanvasReference>().GetCanva().transform.Find("Equipment");
        print(equipmentHolder);
        rangedSlot = equipmentHolder.transform.Find("RangedSlot").GetComponent<ItemSlot>();
        CaCSlot = equipmentHolder.transform.Find("MeleeSlot").GetComponent<ItemSlot>();*/
    }
        


    public void OnWeaponEquipped(ItemClass weapon)
    {
        print("J'équipe l'arme");
        InfoGlobalExel objectInfo = new InfoGlobalExel();
        liseurExel.LesDatas.FindObjectInfo(weapon.itemName, out objectInfo);
        WeaponSystem ws = GetComponentInChildren<WeaponSystem>();
        WeaponsClass newWeapon = null;
        foreach (var wp in ws.Weapons)
        {
            if(wp.name == weapon.itemName)
            {
                newWeapon = wp;
            }
        }

        switch (weapon.itemType)
        {
            case ResumeExelForObject.Type.ArmeAfeu:
                if(rangedWeapon.itemName != "")
                {
                    GestionDesScipt.ScriptGestion.Inventory.AddItem(rangedWeapon, objectInfo, objectInfo.Name);
                }
                ws.ActiveWeapon(newWeapon, true);
                GestionDesScipt.ScriptGestion.Inventory.RemoveItem(weapon);
                rangedSlot.GetComponentInChildren<Image>().sprite = weapon.GetSprite();
                rangedWeapon = weapon;
            break;
            case ResumeExelForObject.Type.ArmeMelee:
                if(CaCweapon.itemName != "")
                {
                    GestionDesScipt.ScriptGestion.Inventory.AddItem(CaCweapon, objectInfo, objectInfo.Name);
                }
                ws.ActiveWeapon(newWeapon, false);
                GestionDesScipt.ScriptGestion.Inventory.RemoveItem(weapon);
                CaCSlot.GetComponentInChildren<Image>().sprite = weapon.GetSprite();
                CaCweapon = weapon;
            break;
        }
        
    }

    public void UnEquipWeapon(ItemClass weapon)
    {
        WeaponSystem ws = GetComponentInChildren<WeaponSystem>();

        switch (weapon.itemType)
        {
            case ResumeExelForObject.Type.ArmeAfeu:
                ws.DesactiveWeapon(true);
                rangedWeapon = new ItemClass{itemName = ""};
                rangedSlot.GetComponentInChildren<Image>().sprite = defaultRanged;
            break;
            case ResumeExelForObject.Type.ArmeMelee:
                ws.DesactiveWeapon(false);
                CaCweapon = new ItemClass{itemName = ""};
                CaCSlot.GetComponentInChildren<Image>().sprite = defaultMelee;
            break;
        }
    }
    
}
