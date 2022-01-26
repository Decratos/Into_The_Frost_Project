using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropEquippedWeapon : MonoBehaviour, IPointerDownHandler
{
    public PointerEventData data;

    private ItemSlot parentItemSlot;

    private void Awake() {
       parentItemSlot = transform.GetComponentInParent<ItemSlot>();
    }
    public void OnPointerDown(PointerEventData eventData)// lorsque je clique
    {
        if(parentItemSlot.equippedItemStat.itemName != "")
        {
            PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().UnEquipWeapon(parentItemSlot.equippedItemStat, 1);
            GestionDesScipt.ScriptGestion.Inventory.AddItem(parentItemSlot.equippedItemStat);
            parentItemSlot.equippedItemStat = null;
        }
    }
}
