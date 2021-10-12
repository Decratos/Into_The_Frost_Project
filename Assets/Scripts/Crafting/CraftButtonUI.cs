using UnityEngine;
using UnityEngine.UI;

public class CraftButtonUI : MonoBehaviour
{
    [SerializeField] private Craft myCraft;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void CreateObject()
    {
        liseurExel excel = CraftUI.instance.ExcelList;
        print("L'excel : " +excel);
        InfoGlobalExel globalInfo;
        GestionDesScipt gestion;
        MesFonctions.FindGestionDesScripts(PlayerSingleton.playerInstance.gameObject, out gestion);
        excel.FindObjectInfo(transform.parent.GetComponentInChildren<Text>().text, out globalInfo);
        myCraft = new Craft{infos = globalInfo.exelCraft};
        CraftUI.instance.craftInterface.CraftItem(myCraft, excel, gestion, globalInfo);
    }

}
