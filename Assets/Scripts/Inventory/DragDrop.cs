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
    public ItemContainer initialWindow;
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
        initialWindow = transform.GetComponentInParent<ItemContainer>();
    }

    public void OnEndDrag(PointerEventData eventData)// lorsque je finis de drag un item
    {
        if (eventData.pointerCurrentRaycast.gameObject == null)
        {
            ItemWorld.DropItem(PlayerSingleton.playerInstance.transform.position, eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item);
        }
        else if(eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemContainer>() || eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<ItemContainer>())
        {
            if (eventData.pointerCurrentRaycast.gameObject != initialWindow && eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>())
            {
                print("J'ai déposé l'item sur une autre fenêtre");
                Inventory newInv = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<UIInventory>()._inventory;
                initialWindow.GetComponent<UIInventory>()._inventory.TransferItem(newInv, eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item);
                print(eventData.pointerCurrentRaycast.gameObject.transform.parent);
                eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<UIInventory>().RefreshInventoryItems();
            }
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<ItemSlot>())
            {
                print("Il y a un slot");
            }
            else
            {
                print("Il n'y a rien");
            }

            
        }
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        
        transform.GetComponentInParent<UIInventory>().RefreshInventoryItems();
    }

    public void OnDrag(PointerEventData eventData) //lors du drag
    {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

}
