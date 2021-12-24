using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public PointerEventData data;
    public bool isDropping = false;

    
    public ItemClass equippedItemStat;

    public enum equipmentType
    {
        Weapon,
        Armor,
        ChestClothing,
        HelmetOrTop,
        Pants,
        Shoes,

    } 
    public equipmentType slotType;

    public int slotNumber;

    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;

    public class OnItemDroppedEventArgs : EventArgs {
        public ItemClass item;
    }
    public void OnDrop(PointerEventData eventData)
    {
        ItemClass item = eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item;
        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs {item = item});
        if(eventData.pointerDrag != null)
        {
            if(item.itemType == ResumeExelForObject.Type.ArmeAfeu || item.itemType == ResumeExelForObject.Type.ArmeMelee)
            {
                print("J'équippe");
                eventData.pointerDrag.GetComponent<RectTransform>().position = 
                GetComponent<RectTransform>().position;
                equippedItemStat = item;
                equippedItemStat.amount = 1;
                PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().OnWeaponEquipped(item, slotNumber);
            }
        }
    }

    
}
