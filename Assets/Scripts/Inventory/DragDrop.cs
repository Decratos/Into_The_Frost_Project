using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    
    public PointerEventData data;
    public bool isDropping;
    public Transform initialWindow;
    public Vector2 initialPosition;
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        isDropping = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)//lorsque je drag un item
    {
        _canvasGroup.alpha = .6f;
        _canvasGroup.blocksRaycasts = false;
        initialWindow = transform.parent.parent;
        initialPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnEndDrag(PointerEventData eventData)// lorsque je finis de drag un item
    {
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            ItemWorld.DropItem(PlayerSingleton.playerInstance.transform.position, eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item);
        }
        else if (eventData.pointerCurrentRaycast.gameObject == initialWindow.gameObject)
        {
            this.GetComponent<RectTransform>().anchoredPosition = initialPosition;
            print("C'est la même fenêtre");
        }
        else if(eventData.pointerCurrentRaycast.gameObject != initialWindow && eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<UIInventory>())
        {
            print("J'ai déposé l'item sur une autre fenêtre");
            Inventory newInv = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<UIInventory>()._inventory;
            if(initialWindow.GetComponent<UIInventory>())
                initialWindow.GetComponent<UIInventory>()._inventory.TransferItem(newInv, eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item);
            else
            {
                if(newInv.capability > 0)
                {
                    if (eventData.pointerDrag.GetComponentInChildren<ItemInfo>() && eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item != null)
                    {
                        newInv.CheckCapability(eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item);
                        eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item = null;
                    }

                    else if (eventData.pointerDrag.GetComponentInChildren<ItemSlot>() && eventData.pointerDrag.GetComponentInChildren<ItemSlot>().equippedItemStat != null)
                    {
                        newInv.CheckCapability(eventData.pointerDrag.GetComponentInChildren<ItemSlot>().equippedItemStat);
                        eventData.pointerDrag.GetComponentInChildren<ItemSlot>().equippedItemStat = null;
                        eventData.pointerDrag.GetComponentInChildren<UnityEngine.UI.Image>().sprite = null;
                    }
                        
                    this.GetComponent<RectTransform>().anchoredPosition = initialPosition;
                }
                    
            }

            eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<UIInventory>().RefreshInventoryItems();
        }
        else if(eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemContainer>() || eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<ItemContainer>())
        {
            
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>())
            {
                print("Il y a un slot");
            }
            else
            {
                print("Il n'y a rien");
            }

            
        }
        else if(eventData.pointerCurrentRaycast.gameObject.GetComponent<Quickslot>())
        {
            print("Il y a un quickslot");
            eventData.pointerCurrentRaycast.gameObject.GetComponent<Quickslot>().AssignItem(eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item);
        }
        else
        {
            print(eventData.pointerCurrentRaycast.gameObject.name);
        }
        
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        if(transform.GetComponentInParent<UIInventory>())
            transform.GetComponentInParent<UIInventory>().RefreshInventoryItems();
    }

    public void OnDrag(PointerEventData eventData) //lors du drag
    {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

}
