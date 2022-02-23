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
        if(parentItemSlot.equippedItemStat.globalInfo.Name != "")
        {
            if(parentItemSlot.equippedItemStat.globalInfo.TypeGeneral == InfoGlobalExel.Type.ArmeAfeu || parentItemSlot.equippedItemStat.globalInfo.TypeGeneral == InfoGlobalExel.Type.ArmeMelee)
            {
                PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().UnEquipWeapon(parentItemSlot.equippedItemStat, 1);
                GestionDesScipt.ScriptGestion.Inventory.CheckCapability(parentItemSlot.equippedItemStat);
                parentItemSlot.equippedItemStat = null;
            }
            else
            {
                liseurExel excel;
                InfoExelvetements infoVetement;
                excel = liseurExel.LesDatas;
                excel.FindObjectInfo(parentItemSlot.equippedItemStat.globalInfo.Name, out infoVetement);
                PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().EquipClothes(parentItemSlot.equippedItemStat, parentItemSlot.slotType, false, infoVetement);
            }
            
        }
    }
}
