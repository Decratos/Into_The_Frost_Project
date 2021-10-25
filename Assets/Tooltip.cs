using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public PointerEventData data;

    private GameObject Object;

    private void Awake() {
        Object = transform.Find("Tooltip").gameObject;
        Object.SetActive(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Object.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Object.SetActive(false);
    }
}
