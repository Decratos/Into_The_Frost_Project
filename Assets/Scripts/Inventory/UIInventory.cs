
using CodeMonkey.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    private Inventory _inventory;
    private Transform itemSlotContainer;
    public Transform itemSlotTemplate;
    private bool inventoryIsOpen = true;

    private void Start()
    {
        GestionDesScipt gestion = null;
        //print(PlayerSingleton.playerInstance.gameObject);
        MesFonctions.FindGestionDesScripts(PlayerSingleton.playerInstance.gameObject, out gestion);
        gestion.uiInventory = this;
        itemSlotContainer = transform.Find("itemSlotContainer");
        PlayerSingleton.playerInstance.GetComponent<InitiateInventory>().Initiate();
        OpenHideInventory(true);
        //itemSlotTemplate = transform.Find("itemSlotTemplate");
    }
    public void SetInventory(Inventory inventory)
    {
        this._inventory = inventory;
        
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    public void RefreshInventoryItems()
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
                            ItemClass duplicateItem = new ItemClass {itemType = item.itemType, amount = item.amount, itemName = item.itemName, globalInfo = item.globalInfo};
                            ItemWorld.DropItem(PlayerSingleton.playerInstance.GetPosition(), duplicateItem);
                            _inventory.RemoveItem(item);
                            RefreshInventoryItems();
                        };
                        itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
                        Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
                        image.sprite = item.GetSprite();
                        TextMeshProUGUI text = itemSlotRectTransform.Find("AmountText").GetComponent<TextMeshProUGUI>();
                        TextMeshProUGUI tooltipText = itemSlotRectTransform.Find("Tooltip").GetComponentInChildren<TextMeshProUGUI>();
                        tooltipText.SetText(item.itemName);
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
    
    public void RemoveClick()
    {}

    public void OpenHideInventory(bool onStart)
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
        RefreshInventoryItems();
        MouseCursorHiderShower.instance.ManageCursor(inventoryIsOpen);
    }
}
