using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class StateForSurvival
{
    public enum PointDeSurvie 
    {
        Hunger,
        Soif,
        Vie,
        Heat
    }
    public PointDeSurvie SurvivalData;
    [HideInInspector] public int Index;
    public Vector2 Range;
    public float ActualValue;
    public float VitesseDecroissement;
    public float PourcentageDecroissement;
    public float DecroissementVieUnderMinima;
    public bool applyDecroissement;
    
}
public class SurvivalSysteme : MonoBehaviour
{
    //Public

    public List<StateForSurvival> LesDataPourSurvie = new List<StateForSurvival>();
    public bool TestUpdate = true;
    public AnimationCurve PerteChaleur;
    //private
    [SerializeField] float TempsCalculSystem = 1;
    [SerializeField] float TemperatureBeginLoseLife = 28;
    [SerializeField] float MultiplayerBehind37 = 0.2f;
    int IndexDataVie = 0;

    private void Start()
    {
        ToutSetForGood();
        if (!TestUpdate)
        {
            BaisseSurvivalData();
        }
        
    }

    void Update()
    {
        if (TestUpdate)
        {
            baisseLesDatas();
        }

        //UpdateUI();
    }

    void BaisseSurvivalData() 
    {
        baisseLesDatas();

        Invoke("BaisseSurvivalData", TempsCalculSystem);
    }

    void baisseLesDatas() 
    {
       
        for (int i = 0; i < LesDataPourSurvie.Count; i++)
        {
            if (LesDataPourSurvie[i].applyDecroissement)
            {
                
                if (TestUpdate)
                {
                    LesDataPourSurvie[i].ActualValue -= LesDataPourSurvie[i].VitesseDecroissement * LesDataPourSurvie[i].PourcentageDecroissement* Time.deltaTime;
                }
                else 
                {
                    LesDataPourSurvie[i].ActualValue -= LesDataPourSurvie[i].VitesseDecroissement * LesDataPourSurvie[i].PourcentageDecroissement;
                }
                LesDataPourSurvie[i].ActualValue = checkLaRange(LesDataPourSurvie[i].Index);
                if (LesDataPourSurvie[i].ActualValue == 0 && (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Hunger || LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Soif))
                {
                   
                    if (TestUpdate)
                    {
                        LesDataPourSurvie[IndexDataVie].ActualValue -= LesDataPourSurvie[i].DecroissementVieUnderMinima * Time.deltaTime;
                    }
                    else 
                    {
                        LesDataPourSurvie[IndexDataVie].ActualValue -= LesDataPourSurvie[i].DecroissementVieUnderMinima;
                    }
                    LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);
                }
                if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Heat && LesDataPourSurvie[i].ActualValue<TemperatureBeginLoseLife)
                {
                    LesDataPourSurvie[IndexDataVie].ActualValue -= calculPerteLife(i);
                    LesDataPourSurvie[i].ActualValue = checkLaRange(LesDataPourSurvie[i].Index);
                    LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);
                }
            }
        }

    }

    

   float calculPerteLife(int index) 
   {
        
        return LesDataPourSurvie[index].DecroissementVieUnderMinima * 
            PerteChaleur.Evaluate( (TemperatureBeginLoseLife - LesDataPourSurvie[index].ActualValue) / 
                (TemperatureBeginLoseLife - LesDataPourSurvie[index].Range.x)); 
   }


    void ToutSetForGood() 
    {
        for (int i = 0; i < LesDataPourSurvie.Count; i++)
        {
            
            LesDataPourSurvie[i].Index = i;
            if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Vie)
            {
                IndexDataVie = i;
            }
            
        }
    
    }
    public void ChangementDuneDataDeSurvie(float value, StateForSurvival.PointDeSurvie ToModifiate) 
    {
        foreach (StateForSurvival item in LesDataPourSurvie)
        {
            if (item.SurvivalData == ToModifiate)
            {
                item.ActualValue += value;
                item.ActualValue = checkLaRange(item.Index);
            }
        }

    }
    public void ChangementDuneDataDeSurvie(float value, int ToModifiate)
    {
        foreach (StateForSurvival item in LesDataPourSurvie)
        {
            if (item.SurvivalData == (StateForSurvival.PointDeSurvie)ToModifiate)
            {
                if (ToModifiate == 3)
                {
                    if (item.ActualValue>37)
                    {
                        item.ActualValue += value * MultiplayerBehind37;
                    }
                }
                else 
                {
                    item.ActualValue += value;
                }
               
                item.ActualValue = checkLaRange(item.Index);
            }
        }

    }
    float checkLaRange(int indexToLook) 
    {
        if (LesDataPourSurvie[indexToLook].ActualValue< LesDataPourSurvie[indexToLook].Range.x)
        {
            return LesDataPourSurvie[indexToLook].Range.x;
        }
        else if (LesDataPourSurvie[indexToLook].ActualValue > LesDataPourSurvie[indexToLook].Range.y)
        {
            return LesDataPourSurvie[indexToLook].Range.y;
        }
        else 
        {
            return LesDataPourSurvie[indexToLook].ActualValue;
        }

    }

    public void GetFood(Vector4 value) 
    {
        for (int i = 0; i < 4; i++)
        {
            
            ChangementDuneDataDeSurvie(value[i], i);


        }
        

    
    }

    private void UpdateUI()
    {
        Canvas myCanva = CanvasReference._canvasReference.GetCanva();
        myCanva.transform.Find("HungerBar").GetComponent<Slider>().value =
            LesDataPourSurvie[0].ActualValue / LesDataPourSurvie[0].Range.y ;
        
        myCanva.transform.Find("ThirstBar").GetComponent<Slider>().value =
            LesDataPourSurvie[1].ActualValue / LesDataPourSurvie[1].Range.y ;
        
        myCanva.transform.Find("HealthBar").GetComponent<Slider>().value =
            LesDataPourSurvie[2].ActualValue / LesDataPourSurvie[2].Range.y;
        
        myCanva.transform.Find("TempBar").GetComponent<Slider>().value =
            LesDataPourSurvie[3].ActualValue / LesDataPourSurvie[3].Range.y;
    }
    
   

}
