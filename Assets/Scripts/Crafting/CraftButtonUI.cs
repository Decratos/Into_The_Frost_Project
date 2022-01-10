using UnityEngine;
using UnityEngine.UI;

public class CraftButtonUI : MonoBehaviour
{
    public Craft myCraft;
    liseurExel excel;
    InfoGlobalExel globalInfo;
    GestionDesScipt gestion;
    // Start is called before the first frame update
    public void DelayedStart() //start mais avec du retard 
    {
        excel = CraftUI.instance.ExcelList;
        MesFonctions.FindGestionDesScripts(PlayerSingleton.playerInstance.gameObject, out gestion);
        excel.FindObjectInfo(transform.parent.GetComponentInChildren<Text>().text, out globalInfo);
        myCraft = new Craft{infos = globalInfo.exelCraft};
    }

    public void CreateObject() // cr�er un objet
    {
        CraftUI.instance.craftInterface.CraftItem(myCraft, excel, gestion, globalInfo);
    }

}
