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
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<GameObject> hoveredObjects = eventData.hovered;
        print(eventData.hovered.Count);
        if (eventData.hovered.Count == 0)
        {
            ItemWorld.DropItem(PlayerSingleton.playerInstance.transform.position, eventData.pointerDrag.GetComponentInChildren<ItemInfo>().item);
        }
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        
        GestionDesScipt.ScriptGestion.uiInventory.RefreshInventoryItems();
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

}
