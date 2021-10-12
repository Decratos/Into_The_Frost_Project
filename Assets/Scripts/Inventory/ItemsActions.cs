using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsActions : MesFonctions
{
    public static ItemsActions itemsActionsInstance;

    private GestionDesScipt scriptsGestion;
    // Start is called before the first frame update
    void Awake()
    {
        FindGestionDesScripts(PlayerSingleton.playerInstance.gameObject, out scriptsGestion);
        itemsActionsInstance = this;
    }


    public void ItemAction(string name, ItemClass item)
    {
        switch (item.itemType)
        {
            case ResumeExelForObject.Type.Nourriture:
                scriptsGestion.SurvieScript.LesDataPourSurvie[0].ActualValue +=
                    item.globalInfo.exelNourriturre.Nourritture;
                scriptsGestion.SurvieScript.LesDataPourSurvie[1].ActualValue +=
                    item.globalInfo.exelNourriturre.Eau;
                scriptsGestion.SurvieScript.LesDataPourSurvie[2].ActualValue +=
                    item.globalInfo.exelNourriturre.Chaleur;
                scriptsGestion.SurvieScript.LesDataPourSurvie[3].ActualValue +=
                    item.globalInfo.exelNourriturre.Vie;
                break;
            case ResumeExelForObject.Type.Soins:
                scriptsGestion.SurvieScript.LesDataPourSurvie[2].ActualValue +=
                    item.globalInfo.exelSoins.value;
                break;
            case ResumeExelForObject.Type.ArmeAfeu:
                break;
            default:
                break;
        }
    }
}
