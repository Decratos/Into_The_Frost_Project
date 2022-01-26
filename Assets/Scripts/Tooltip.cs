using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public PointerEventData data;

    [SerializeField] private GameObject Object;

    public bool globalTooltipForCraft = false;

    private void Awake() {
        if(transform.Find("Tooltip") != null)
            Object = transform.Find("Tooltip").gameObject;
        Object.SetActive(false);
    }
    
    public void OnPointerEnter(PointerEventData eventData)// permet d'afficher le craft
    {
        Object.SetActive(true);
        if(globalTooltipForCraft)
        {
            TextMeshProUGUI tooltipText = Object.GetComponentInChildren<TextMeshProUGUI>();
            Craft craft = GetComponent<CraftButtonUI>().myCraft;
            string craftRessources = "";
            for (int i = 0 ; i < craft.infos.NomdesRessourcesNecessaire.Length; i++)
            {
                craftRessources += " "+craft.infos.NomdesRessourcesNecessaire[i];
            }
            
            
            tooltipText.SetText(craftRessources);

        }
    }
 
    public void OnPointerExit(PointerEventData eventData)// Lorsque le pointer sors desaffiche l'objet
    {
        Object.SetActive(false);
    }
}
