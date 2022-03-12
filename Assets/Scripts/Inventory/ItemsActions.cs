using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsActions : MesFonctions
{
    public static ItemsActions itemsActionsInstance;

    private GestionDesScipt scriptsGestion;
    // Start is called before the first frame update
    void Start()
    {
        FindGestionDesScripts(PlayerSingleton.playerInstance.gameObject, out scriptsGestion);
        itemsActionsInstance = this; 
    }


    public void ItemAction(string name, ItemClass item) //itemAction?
    {
        switch (item.globalInfo.TypeGeneral)
        {
            case InfoGlobalExel.Type.Nourriture:
                scriptsGestion.SurvieScript.LesDataPourSurvie[0].ActualValue +=
                    item.globalInfo.exelNourriturre.Nourritture;
                scriptsGestion.SurvieScript.LesDataPourSurvie[1].ActualValue +=
                    item.globalInfo.exelNourriturre.Eau;
                scriptsGestion.SurvieScript.LesDataPourSurvie[2].ActualValue +=
                    item.globalInfo.exelNourriturre.Chaleur;
                scriptsGestion.SurvieScript.LesDataPourSurvie[3].ActualValue +=
                    item.globalInfo.exelNourriturre.Vie;
                break;
            case InfoGlobalExel.Type.Soins:
                scriptsGestion.SurvieScript.LesDataPourSurvie[3].ActualValue +=
                    item.globalInfo.exelSoins.value;
                break;
            default:
                break;
        }
    }
}
