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
    public void OnPointerDown(PointerEventData eventData)
    {
        if(parentItemSlot.equippedItemStat.itemName != "")
        {
            PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().UnEquipWeapon(parentItemSlot.equippedItemStat);
            InfoGlobalExel infos = new InfoGlobalExel();
            liseurExel.LesDatas.FindObjectInfo(parentItemSlot.equippedItemStat.itemName, out infos);
            GestionDesScipt.ScriptGestion.Inventory.AddItem(parentItemSlot.equippedItemStat, infos, infos.Name);
            parentItemSlot.equippedItemStat = null;
        }
    }
}
