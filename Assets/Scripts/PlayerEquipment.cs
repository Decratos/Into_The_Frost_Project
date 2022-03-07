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
        


    public void OnWeaponEquipped(ItemClass weapon, int slotNumber)// lorsqu'une arme est équippé
    {
        WeaponSystem ws = GetComponentInChildren<WeaponSystem>();
        WeaponsClass newWeapon = null;
        
        foreach (var wp in ws.Weapons)
        {
            if(wp.name == weapon.globalInfo.Name)
            {
                newWeapon = wp;
            }
        }

        switch (slotNumber)
        {
            case 1:
                if(Weapon1Item.globalInfo.Name != null)
                {
                    GestionDesScipt.ScriptGestion.Inventory.CheckCapability(Weapon1Item);
                    print(Weapon1Item.globalInfo.Name);
                }
                GestionDesScipt.ScriptGestion.Inventory.RemoveItem(weapon);
                Weapon1.GetComponentInChildren<Image>().sprite = weapon.GetSprite();
                Weapon1Item = weapon;
                CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Add(Weapon1Item);
                print("Le nom de mon arme : " + newWeapon);
                ws.ActiveWeapon(newWeapon, 1);
                break;
            case 2:
                if(Weapon2Item.globalInfo.Name != "")
                {
                    GestionDesScipt.ScriptGestion.Inventory.CheckCapability(Weapon2Item);
                }
                ws.ActiveWeapon(newWeapon, 2);
                GestionDesScipt.ScriptGestion.Inventory.RemoveItem(weapon);
                Weapon2.GetComponentInChildren<Image>().sprite = weapon.GetSprite();
                Weapon2Item = weapon;
                CanvasReference._canvasReference.GetComponent<UIInventory>().equippedItems.Add(Weapon2Item);
                break;
        }
        
    }

    public void UnEquipWeapon(ItemClass weapon, int slot) // lorsqu'une arme est désiquipé
    {
        WeaponSystem ws = GetComponentInChildren<WeaponSystem>();

        switch (slot)
        {
            case 1:
                ws.DesactiveWeapon(1);
                Weapon1Item = new ItemClass{globalInfo = weapon.globalInfo};
                Weapon1.GetComponentInChildren<Image>().sprite = defaultWeapon;
                print(Weapon1Item);
                GetComponent<InventoryManager>().mainInventory.equippedItems.Remove(Weapon1Item);
                break;
            case 2:
                ws.DesactiveWeapon(2);
                Weapon2Item = new ItemClass{globalInfo = weapon.globalInfo};
                Weapon2.GetComponentInChildren<Image>().sprite = defaultWeapon;
                GetComponent<InventoryManager>().mainInventory.equippedItems.Remove(Weapon2Item);
                break;
        }
    }

    public void EquipClothes(ItemClass cloth, bool willBeEquiped, InfoExelvetements infos)
    {
        if(willBeEquiped)
        {
            switch (infos.MaCategorie)
            {
                case InfoExelvetements.SousCategorie.Manteau:
                    ChestClothSlot.GetComponentInChildren<Image>().sprite = cloth.GetSprite();
                    ChestCloth = cloth;
                    CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Add(ChestCloth);


                    break;
                case InfoExelvetements.SousCategorie.pull:
                    ChestArmorSlot.GetComponentInChildren<Image>().sprite = cloth.GetSprite();
                    ChestArmor = cloth;
                    CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Add(ChestArmor);

                    break;
                case InfoExelvetements.SousCategorie.Pantalon:
                    PantsSlot.GetComponentInChildren<Image>().sprite = cloth.GetSprite();
                    Pants = cloth;
                    CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Add(Pants);

                    break;
                case InfoExelvetements.SousCategorie.Chaussure:
                    ShoesSlot.GetComponentInChildren<Image>().sprite = cloth.GetSprite();
                    Shoes = cloth;
                    CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Add(Shoes);

                    break;
            }
            GestionDesScipt.ScriptGestion.Inventory.RemoveItem(cloth);
            GestionDesScipt.ScriptGestion.SurvieScript.ResistanceFroidsTotal += infos.ChaleurResistance;
            GestionDesScipt.ScriptGestion.SurvieScript.ResistanceDegatsTotal += infos.DegatResistance;
            
        }
        else
        {
            switch (infos.MaCategorie)
            {
                case InfoExelvetements.SousCategorie.Manteau:
                    ChestClothSlot.GetComponentInChildren<Image>().sprite = cloth.GetSprite();
                    CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Remove(ChestCloth);
                    break;
                case InfoExelvetements.SousCategorie.pull:
                    ChestArmorSlot.GetComponentInChildren<Image>().sprite = cloth.GetSprite();
                    CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Remove(ChestArmor);
                    break;
                case InfoExelvetements.SousCategorie.Pantalon:
                    PantsSlot.GetComponentInChildren<Image>().sprite = cloth.GetSprite();
                    CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Remove(Pants);
                    break;
                case InfoExelvetements.SousCategorie.Chaussure:
                    ShoesSlot.GetComponentInChildren<Image>().sprite = cloth.GetSprite();
                    CanvasReference._canvasReference.GetCanva().GetComponentInChildren<UIInventory>().equippedItems.Remove(Shoes);
                    break;
            }
            GestionDesScipt.ScriptGestion.Inventory.CheckCapability(cloth);
            GestionDesScipt.ScriptGestion.SurvieScript.ResistanceFroidsTotal -= infos.ChaleurResistance;
            GestionDesScipt.ScriptGestion.SurvieScript.ResistanceDegatsTotal -= infos.DegatResistance;
        }
        
    }
    
}