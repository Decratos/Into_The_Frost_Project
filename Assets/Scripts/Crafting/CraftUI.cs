using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    public Crafting craftInterface;

    public static CraftUI instance;
    private bool isOpen = false;
    [SerializeField] private GameObject craftTemplate;
    [SerializeField] private Transform craftContainer;

    public liseurExel ExcelList;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        craftInterface = new Crafting();
        gameObject.SetActive(isOpen);
        MesFonctions.FindDataExelForObject(out ExcelList);
    }

    public void OpenHideCraftUI()
    {
        isOpen = !isOpen;
        gameObject.SetActive(isOpen);
        RefreshUI();
    }

    private void RefreshUI()
    {
        int craftListLenght = ExcelList.MesListe.LesCrafts.Length;
        int y = 0;
        float craftSlotSize = 100f;
        for(int i = 0; i<craftListLenght; i++)
        {
            RectTransform craftRectTransform = Instantiate(craftTemplate, craftContainer).GetComponent<RectTransform>();
            craftRectTransform.gameObject.SetActive(true);
            Text text = craftRectTransform.GetComponentInChildren<Text>();
            text.text = ExcelList.MesListe.LesCrafts[i].Name;

            craftRectTransform.anchoredPosition = new Vector2(0, y * craftSlotSize);
            y++;
        }
    }
}
