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

    #region value vetements
    public float ResistanceFroidsTotal;
    public float ResistanceDegatsTotal;
    public int NombreDemplacementPourVetement;
    #endregion

    //private
    [SerializeField] float TempsCalculSystem = 1;
    [SerializeField] float TemperatureBeginLoseLife = 28;
    [SerializeField] float MultiplayerBehind37 = 0.2f;
    int IndexDataVie = 0;
    InfoExelvetements[] LesVetementsQueJePorte;

    private void Start()
    {
        ToutSetForGood();// lance le set 
        if (!TestUpdate)//si je ne suis pas en mode update /!\ à voir le plus couteux /!\
        {
            BaisseSurvivalData();//void temp pour faire via Invoke
        }
        
    }

    void Update()
    {
        if (TestUpdate)// si je suis en test update /!\ à voir le plus couteux /!\
        {
            baisseLesDatas(); // envois la baisse des stats de survie
        }

        //UpdateUI();
    }

    void BaisseSurvivalData() //
    {
        baisseLesDatas(); // baisse les stats de survie

        Invoke("BaisseSurvivalData", TempsCalculSystem); // relance ce void dans x second
    } // void alternatif du update /!\ à voir le plus couteux /!\

    void baisseLesDatas() 
    {
       
        for (int i = 0; i < LesDataPourSurvie.Count; i++)
        {
            if (LesDataPourSurvie[i].applyDecroissement)// si j'applique le decroissement
            {
                
                if (TestUpdate) // si je suis en mode update /!\ à voir le plus couteux /!\
                {
                    LesDataPourSurvie[i].ActualValue -= LesDataPourSurvie[i].VitesseDecroissement * LesDataPourSurvie[i].PourcentageDecroissement* Time.deltaTime; // baisse une des valeurs de survie
                }
                else 
                {
                    LesDataPourSurvie[i].ActualValue -= LesDataPourSurvie[i].VitesseDecroissement * LesDataPourSurvie[i].PourcentageDecroissement;// baisse une des valeurs de survie
                }
                LesDataPourSurvie[i].ActualValue = checkLaRange(LesDataPourSurvie[i].Index); // vérifie que la valeur reste bien dans le carcant 
                if (LesDataPourSurvie[i].ActualValue == 0 && (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Hunger || LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Soif)) // si la soif ou la faim est à 0
                {
                   
                    if (TestUpdate) // si je suis en mode update /!\ à voir le plus couteux /!\ // à voir si je peux remplacer par calcul perte life
                    {
                        LesDataPourSurvie[IndexDataVie].ActualValue -= LesDataPourSurvie[i].DecroissementVieUnderMinima * Time.deltaTime;
                    }
                    else 
                    {
                        LesDataPourSurvie[IndexDataVie].ActualValue -= LesDataPourSurvie[i].DecroissementVieUnderMinima;
                    }
                    LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);// vérifie que la valeur reste bien dans le carcant 
                }
                if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Heat && LesDataPourSurvie[i].ActualValue<TemperatureBeginLoseLife)// si je suis en dessous de la chaleur
                {
                    LesDataPourSurvie[IndexDataVie].ActualValue -= calculPerteLife(i);// je calcul la perte de vie selon la valeur
                    LesDataPourSurvie[i].ActualValue = checkLaRange(LesDataPourSurvie[i].Index);
                    LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);
                }
            }
        }// selon les datas de survie

    } // 

    

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
        LesVetementsQueJePorte = new InfoExelvetements[NombreDemplacementPourVetement];
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

    public void SetVetements(InfoExelvetements Info,int IndexEmplacement)
    {
        if (IndexEmplacement > NombreDemplacementPourVetement-1 || IndexEmplacement < 0)
        {
            print("L'emplacement Demander n'existe pas");
        }
        else 
        {
            if (LesVetementsQueJePorte[IndexEmplacement].MaCategorie == Info.MaCategorie || !LesVetementsQueJePorte[IndexEmplacement].IsWeared)
            {

                ResistanceDegatsTotal-= LesVetementsQueJePorte[IndexEmplacement].DegatResistance;
                ResistanceFroidsTotal-= LesVetementsQueJePorte[IndexEmplacement].ChaleurResistance;
                ResistanceDegatsTotal += Info.DegatResistance;
                ResistanceFroidsTotal += Info.ChaleurResistance;
                LesVetementsQueJePorte[IndexEmplacement] = Info;
            }
        }
        
    }

}
