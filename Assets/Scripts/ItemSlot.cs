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
        Coat,
        Pull,
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
    public void OnDrop(PointerEventData eventData) // lache l'item
    {
        ItemClass item = eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item;
        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs {item = item});
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = 
            GetComponent<RectTransform>().position;
            equippedItemStat = item;
            if(item.itemType != ResumeExelForObject.Type.Vetements)
            {
                print("Ce n'est pas un vêtement");
                if (item.itemType == ResumeExelForObject.Type.ArmeAfeu || item.itemType == ResumeExelForObject.Type.ArmeMelee)
                {
                    print("C'est une arme");
                    PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().OnWeaponEquipped(item, slotNumber);
                }
                print(item.itemType);
            }
            else
            {
                print("C'est un vêtement");
                liseurExel excel;
                InfoExelvetements infoVetement;
                excel = liseurExel.LesDatas;
                excel.FindObjectInfo(item.itemName, out infoVetement);
                if (infoVetement.MaCategorie == InfoExelvetements.SousCategorie.Tshirt && slotType == equipmentType.Coat)
                {
                    PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().EquipClothes(item, slotType, true, infoVetement);
                }
                else if(infoVetement.MaCategorie == InfoExelvetements.SousCategorie.Pantalon && slotType == equipmentType.Pants)
                {
                    PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().EquipClothes(item, slotType, true, infoVetement);
                }
                else if(infoVetement.MaCategorie == InfoExelvetements.SousCategorie.Chaussure && slotType == equipmentType.Shoes)
                {
                    PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().EquipClothes(item, slotType, true, infoVetement);
                }
                else if(infoVetement.MaCategorie == InfoExelvetements.SousCategorie.pull && slotType == equipmentType.Pull)
                {
                    PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().EquipClothes(item, slotType, true, infoVetement);
                }
            }
            equippedItemStat.amount = 1;


        }
    }

    
}
