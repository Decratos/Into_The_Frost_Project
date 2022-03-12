
using CodeMonkey.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class UIInventory : MonoBehaviour
{
    public Inventory _inventory;
    public List<ItemClass> equippedItems;
    private Transform equippedItemContainer;
    public Transform equippedItemTemplate;
    public List<UIInventory> inheritedInventory;
    private Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public bool inventoryIsOpen = false;
    public bool isPlayerInventory = false;
    public Transform BasicUI;

    public bool isPrimaryWindow = false;

    private void Start()
    {
        GestionDesScipt gestion = null;
        itemSlotContainer = transform.Find("itemSlotContainer");
        equippedItemContainer = transform.Find("EquipmentList");

        if(isPlayerInventory)
        {
            if(isPrimaryWindow)
            {
                MesFonctions.FindGestionDesScripts(PlayerSingleton.playerInstance.gameObject, out gestion);
                gestion.uiInventory = this;
                PlayerSingleton.playerInstance.GetComponent<InventoryManager>().Initiate();
                PlayerSingleton.playerInstance.GetComponent<InventoryManager>().mainInventory = this;
            }
        }
        if(inheritedInventory.Count > 0)
        {
            foreach (var inv in inheritedInventory)
            {
                inv._inventory = this._inventory;
            }
        }
        gameObject.SetActive(false);
        //itemSlotTemplate = transform.Find("itemSlotTemplate");
    }

    [Button("SetPlayerInventaire")]
    private void SetPlayerInventory()// set l'inventaire du joueur
    {
        foreach (var inv in inheritedInventory)
        {
            print("Inventaire défini");
            inv._inventory = PlayerSingleton.playerInstance.GetComponent<Inventory_Debug>().inv;
        }
    }
    public void SetInventory(Inventory inventory)// set l'inventaire
    {
        this._inventory = inventory;
        
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        //RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
        RefreshEquippedItem();
    }

    public void RefreshInventoryItems() //refresh l'inventaire
    {
        if (inventoryIsOpen)
        {
            foreach (Transform child in itemSlotContainer)
                    {
                        if (child == itemSlotTemplate && child.transform.name != "ScrolArea") continue;
                        Destroy(child.gameObject);
                    }
                    int x = 0;
                    int y = 0;
                    float itemSlotCellSize = 120f;
                    foreach (ItemClass item in _inventory.GetItemList())
                    {
                        RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
                        itemSlotRectTransform.gameObject.SetActive(true);
            
                        itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
                        {
                            _inventory.UseItem(item);
                        };
                        itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
                        {
                            ItemClass duplicateItem = new ItemClass {amount = item.amount,globalInfo = item.globalInfo};
                            ItemWorld.DropItem(PlayerSingleton.playerInstance.GetPosition(), duplicateItem);
                            _inventory.RemoveItem(item);
                            RefreshInventoryItems();
                        };
                        //itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
                        itemSlotRectTransform.anchoredPosition = new Vector2(itemSlotTemplate.GetComponent<RectTransform>().anchoredPosition.x, itemSlotTemplate.GetComponent<RectTransform>().anchoredPosition.y);
                        itemSlotRectTransform.localScale = itemSlotTemplate.localScale;
                        Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
                        image.sprite = item.GetSprite();
                        TextMeshProUGUI text = itemSlotRectTransform.Find("AmountText").GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI tooltipText = itemSlotRectTransform.Find("Tooltip").GetComponentInChildren<TextMeshProUGUI>();
                        tooltipText.SetText(item.globalInfo.Name);
                        itemSlotRectTransform.GetComponentInChildren<ItemInfo>().item = item;
                        if (item.amount == 1)
                        {
                            text.SetText("");
                        }
                        else
                        {
                            text.SetText(item.amount.ToString());
                        }
                        x++;
                        if (x == 3)
                        {
                            x = 0;
                            y--;
                        }
                    }
        }
        
    }

    public void RefreshEquippedItem()
    {
        if(inventoryIsOpen && equippedItems.Count > 0)
        {
            foreach (Transform child in equippedItemContainer)
            {
                if (child == equippedItemTemplate) continue;
                Destroy(child.gameObject);
            }
            foreach (var item in equippedItems)
            {
                RectTransform itemSlotRectTransform = Instantiate(equippedItemTemplate, equippedItemContainer).GetComponent<RectTransform>();
                itemSlotRectTransform.gameObject.SetActive(true);
                itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
                {
                    if(item.globalInfo.TypeGeneral == InfoGlobalExel.Type.ArmeAfeu || item.globalInfo.TypeGeneral == InfoGlobalExel.Type.ArmeMelee)
                    {
                        PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().UnEquipWeapon(item, 1);  
                    }
                    else if(item.globalInfo.TypeGeneral == InfoGlobalExel.Type.Vetements)
                    {
                        InfoExelvetements infoVetements;
                        liseurExel.LesDatas.FindObjectInfo(item.globalInfo.Name, out infoVetements);
                        PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().EquipClothes(item, false, infoVetements);
                    }
                    else if(item.globalInfo.TypeGeneral == InfoGlobalExel.Type.Sac)
                    {
                        PlayerSingleton.playerInstance.GetComponent<PlayerEquipment>().EquipBackpack(null, true);
                    }
                    Destroy(itemSlotRectTransform.gameObject);
                    equippedItems.Remove(item);
                    _inventory.CheckCapability(item);
                    
                };
                itemSlotRectTransform.anchoredPosition = new Vector2(equippedItemTemplate.GetComponent<RectTransform>().anchoredPosition.x, equippedItemTemplate.GetComponent<RectTransform>().anchoredPosition.y);
                Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
                image.sprite = item.GetSprite();
                itemSlotRectTransform.GetComponentInChildren<ItemInfo>().item = item;
                itemSlotRectTransform.GetComponentInChildren<TextMeshProUGUI>().text = item.globalInfo.Name;
            }
        }
    }

    public void OpenHideInventory(bool onStart)// ouvre ou ferme l'inventaire
    {
        inventoryIsOpen = !inventoryIsOpen;
        if (!onStart)
        {
            switch (inventoryIsOpen)
            {
                case true:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Inventory/InventoryOpen", PlayerSingleton.playerInstance.transform.position);
                    BasicUI.GetComponent<BasicUIGestion>().SetLastWindow(this.transform);
                break;
                case false:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Inventory/InventoryClose", PlayerSingleton.playerInstance.transform.position);
                    break;
            }
        }
        gameObject.SetActive(inventoryIsOpen);
        PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = !inventoryIsOpen;
        PlayerSingleton.playerInstance.GetComponent<CharacterController>().enabled = !inventoryIsOpen;
        RefreshInventoryItems();
        RefreshEquippedItem();
        MouseCursorHiderShower.instance.ManageCursor(inventoryIsOpen);
    }
    public void OpenHideInventory(string forceOpen, bool reactivePlayer)
    {
        switch (forceOpen)
        {
            case "Open":
                gameObject.SetActive(true);
                inventoryIsOpen = true;
                RefreshInventoryItems();
                RefreshEquippedItem();
                PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = false;
                PlayerSingleton.playerInstance.GetComponent<CharacterController>().enabled = false;
                MouseCursorHiderShower.instance.ManageCursor(true);
                break;
            case "Close":
                gameObject.SetActive(false);
                inventoryIsOpen = false;
                break;
        }
        if(reactivePlayer)
        {
            PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = true;
            PlayerSingleton.playerInstance.GetComponent<CharacterController>().enabled = true;
            MouseCursorHiderShower.instance.ManageCursor(false);
        }
        
    }
    public void OpenHideInventory(string forceOpen)
    {
        switch (forceOpen)
        {
            case "Open":
                gameObject.SetActive(true);
                inventoryIsOpen = true;
                RefreshInventoryItems();
                RefreshEquippedItem();
                break;
            case "Close":
                gameObject.SetActive(false);
                inventoryIsOpen = false;
                break;
        }

    }
}
