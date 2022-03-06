using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiEquipmentSetter : MonoBehaviour, IDropHandler
{
    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
    public bool isDropping = false;

    public enum equipmentType
    {
        Weapon,
        Coat,
        Pull,
        HelmetOrTop,
        Pants,
        Shoes,

    }

    public class OnItemDroppedEventArgs : EventArgs
    {
        public ItemClass item;
    }

    public void OnDrop(PointerEventData eventData) // lache l'item
    {
        
        ItemClass item = eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item;

        print(item.globalInfo.ID);
        switch (item.globalInfo.TypeGeneral)
        {
            case InfoGlobalExel.Type.ArmeAfeu:
            case InfoGlobalExel.Type.ArmeMelee:     
                PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().OnWeaponEquipped(item, 1);
                PlayerSingleton.playerInstance.GetComponent<Inventory_Debug>().inv.RemoveItem(item);
                break;
            case InfoGlobalExel.Type.Sac:
                PlayerSingleton.playerInstance.GetComponent<Inventory_Debug>().inv.RemoveItem(item);
                break;
            case InfoGlobalExel.Type.Vetements:
                liseurExel excel;
                InfoExelvetements infoVetement;
                excel = liseurExel.LesDatas;
                excel.FindObjectInfo(item.globalInfo.Name, out infoVetement);
                PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().EquipClothes(item, true, infoVetement);
                PlayerSingleton.playerInstance.GetComponent<Inventory_Debug>().inv.RemoveItem(item);
                break;

        }
        GetComponentInParent<UIInventory>().RefreshEquippedItem();

    }
}
