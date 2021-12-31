using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField]
    private ItemClass Weapon1Item, Weapon2Item, helmet, ChestCloth, ChestArmor, Pants, Shoes;

    [SerializeField] private Sprite defaultWeapon, defaultHelmet;

    public ItemSlot Weapon1, Weapon2, helmetSlot, ChestClothSlot, ChestArmorSlot, PantsSlot, ShoesSlot;
    
    private void Awake() {
        /*var equipmentHolder = GetComponent<CanvasReference>().GetCanva().transform.Find("Equipment");
        print(equipmentHolder);
        rangedSlot = equipmentHolder.transform.Find("RangedSlot").GetComponent<ItemSlot>();
        CaCSlot = equipmentHolder.transform.Find("MeleeSlot").GetComponent<ItemSlot>();*/
    }
        


    public void OnWeaponEquipped(ItemClass weapon, int slotNumber)
    {
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

        switch (slotNumber)
        {
            case 1:
                if(Weapon1Item.itemName != "")
                {
                    GestionDesScipt.ScriptGestion.Inventory.AddItem(Weapon1Item);
                }
                ws.ActiveWeapon(newWeapon, 1);
                GestionDesScipt.ScriptGestion.Inventory.RemoveItem(weapon);
                Weapon1.GetComponentInChildren<Image>().sprite = weapon.GetSprite();
                Weapon1Item = weapon;
            break;
            case 2:
                if(Weapon2Item.itemName != "")
                {
                    GestionDesScipt.ScriptGestion.Inventory.AddItem(Weapon2Item);
                }
                ws.ActiveWeapon(newWeapon, 2);
                GestionDesScipt.ScriptGestion.Inventory.RemoveItem(weapon);
                Weapon2.GetComponentInChildren<Image>().sprite = weapon.GetSprite();
                Weapon2Item = weapon;
            break;
        }
        
    }

    public void UnEquipWeapon(ItemClass weapon, int slot)
    {
        WeaponSystem ws = GetComponentInChildren<WeaponSystem>();

        switch (slot)
        {
            case 1:
                ws.DesactiveWeapon(1);
                Weapon1Item = new ItemClass{itemName = ""};
                Weapon1.GetComponentInChildren<Image>().sprite = defaultWeapon;
            break;
            case 2:
                ws.DesactiveWeapon(2);
                Weapon2Item = new ItemClass{itemName = ""};
                Weapon2.GetComponentInChildren<Image>().sprite = defaultWeapon;
            break;
        }
    }
    
}
