using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler//lorsque je lache un élément d'ui
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
        Food,
        Materials

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
        GestionDesScipt gestion;
        MesFonctions.FindGestionDesScripts(PlayerSingleton.playerInstance.gameObject, out gestion);
        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs {item = item});
        if(eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = 
            GetComponent<RectTransform>().position;
            equippedItemStat = item;
            if(item.globalInfo.TypeGeneral != InfoGlobalExel.Type.Vetements)
            {
                print("Ce n'est pas un vêtement");
                if (item.globalInfo.TypeGeneral == InfoGlobalExel.Type.ArmeAfeu || item.globalInfo.TypeGeneral == InfoGlobalExel.Type.ArmeMelee)
                {
                    print("C'est une arme");
                    PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().OnWeaponEquipped(item, slotNumber);
                }
                else if(item.globalInfo.TypeGeneral == InfoGlobalExel.Type.Nourriture && slotType == equipmentType.Food)
                {
                    gestion.Inventory.RemoveItem(item);
                }
                else if(item.globalInfo.TypeGeneral == InfoGlobalExel.Type.Materials && slotType == equipmentType.Materials)
                {          
                    gestion.Inventory.RemoveItem(item);
                }
                print(item.globalInfo.TypeGeneral);
                GetComponentInChildren<Image>().sprite = equippedItemStat.GetSprite();
            }
            equippedItemStat.amount = 1;


        }
    }

    
}
