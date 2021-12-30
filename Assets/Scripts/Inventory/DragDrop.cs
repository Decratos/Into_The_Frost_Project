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

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = .6f;
        _canvasGroup.blocksRaycasts = false;
        initialWindow = GetComponentInParent<UIInventory>().transform;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<GameObject> hoveredObjects = eventData.hovered;
        print(eventData.hovered.Count);
        if (eventData.hovered.Count == 0)
        {
            ItemWorld.DropItem(PlayerSingleton.playerInstance.transform.position, eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item);
        }
        else if(eventData.hovered[0].transform != initialWindow)
        {
            print("J'ai déposé l'item sur une autre fenêtre");
            Inventory newInv = eventData.hovered[0].transform.GetComponentInParent<UIInventory>()._inventory;
            initialWindow.GetComponent<UIInventory>()._inventory.TransferItem(newInv, eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item);
            eventData.hovered[0].transform.GetComponentInParent<UIInventory>().RefreshInventoryItems();
        }
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        
        transform.GetComponentInParent<UIInventory>().RefreshInventoryItems();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

}
