
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
    public List<UIInventory> inheritedInventory;
    private Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    public bool inventoryIsOpen = false;
    public bool isPlayerInventory = false;

    public bool isPrimaryWindow = false;

    private void Start()
    {
        GestionDesScipt gestion = null;
        itemSlotContainer = transform.Find("itemSlotContainer");

        if(isPlayerInventory)
        {
            if(isPrimaryWindow)
            {
                MesFonctions.FindGestionDesScripts(PlayerSingleton.playerInstance.gameObject, out gestion);
                gestion.uiInventory = this;
                PlayerSingleton.playerInstance.GetComponent<InventoryManager>().Initiate();
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
    }

    public void RefreshInventoryItems() //refresh l'inventaire
    {
        if (inventoryIsOpen)
        {
            foreach (Transform child in itemSlotContainer)
                    {
                        if (child == itemSlotTemplate) continue;
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
                        itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
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
    
    public void RemoveClick()//?
    {}

    public void OpenHideInventory(bool onStart)// ouvre ou ferme l'inventaire
    {
        inventoryIsOpen = !inventoryIsOpen;
        if(!onStart)
        {
            switch (inventoryIsOpen)
            {
                case true:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Inventory/InventoryOpen", transform.position);
                break;
                case false:
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Inventory/InventoryClose", transform.position);
                break;
            }
        }
        gameObject.SetActive(inventoryIsOpen);
        PlayerSingleton.playerInstance.GetComponentInChildren<CameraMouvement>().canLook = !inventoryIsOpen;
        PlayerSingleton.playerInstance.GetComponent<CharacterController>().enabled = !inventoryIsOpen;
        RefreshInventoryItems();
        MouseCursorHiderShower.instance.ManageCursor(inventoryIsOpen);
    }
}
