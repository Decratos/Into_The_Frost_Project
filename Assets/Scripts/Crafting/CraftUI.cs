using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    public Crafting craftInterface;

    public static CraftUI instance;
    public bool isOpen = false;
    [SerializeField] private GameObject craftTemplate;
    [SerializeField] private Transform craftContainer;
    private List<RectTransform> craftList;
    public Transform PlayerSubInventory;

    public liseurExel ExcelList;
    public RectTransform tooltipWindow;
    InfoGlobalExel globalInfo;
    public Transform BasicUI;

    private void Awake()
    {
        instance = this;
        MesFonctions.FindDataExelForObject(out ExcelList);
    }
    void Start()
    {
        craftInterface = new Crafting();
        gameObject.SetActive(isOpen);

        craftList = new List<RectTransform>();
    }

    public void OpenHideCraftUI(int maxLevel) // ouvre ou non le craft
    {
        isOpen = !isOpen;
        BasicUI.gameObject.SetActive(isOpen);
        PlayerSubInventory.GetComponent<UIInventory>().OpenHideInventory("Open");
        gameObject.SetActive(isOpen);
        RefreshUI(maxLevel);
    }
    public void OpenHideCraftUI(bool forceOpen, int maxLevel)
    {
        if(forceOpen)
        {
            isOpen = true;
            PlayerSubInventory.GetComponent<UIInventory>().OpenHideInventory("Open");
            gameObject.SetActive(true);
            RefreshUI(maxLevel);
        }
        else
        {
            isOpen = false;
            PlayerSubInventory.GetComponent<UIInventory>().OpenHideInventory("Close");
            gameObject.SetActive(false);
        }
    }

    private void RefreshUI(int maxLevel) // refresh L'UI du craft
    {
        if(craftList.Count > 0)
        {
            foreach (var item in craftList)
            {
                Destroy(item.gameObject);
            }
            craftList.Clear();
        }
        
        int craftListLenght = ExcelList.MesListe.LesItems.Length;
        int y = 0;
        float craftSlotSize = 100f;
        for(int i = 0; i<craftListLenght; i++)
        {
            if(ExcelList.MesListe.LesItems[i].CraftingLevel <= maxLevel && ExcelList.MesListe.LesItems[i].CraftingLevel != -1)
            {
                RectTransform craftRectTransform = Instantiate(craftTemplate, craftContainer).GetComponent<RectTransform>();
                craftRectTransform.gameObject.SetActive(true);
                Text text = craftRectTransform.GetComponentInChildren<Text>();
                text.text = ExcelList.MesListe.LesItems[i].Name;
        
                craftRectTransform.anchoredPosition = new Vector2(0, y * craftSlotSize);
                y++;
                craftList.Add(craftRectTransform);
                craftRectTransform.GetComponentInChildren<CraftButtonUI>().DelayedStart();
            }
        }
    }
}
