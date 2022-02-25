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
    public bool applyDecroissement; // value de test à supprimer un de ces 4
    [HideInInspector] public int Index;
    
    public Vector2 Range;
    public float ActualValue;
    
   
    public float DecroissementVieUnderMinima;

    [Tooltip("combien je perd en une frame")]
    public float PerteMaxByFrame;
   
    [Tooltip("Combien je perds selon le pourcentage actual / range Y")]
    public AnimationCurve PerteByPourcentage;
}
public class SurvivalSysteme : MesFonctions
{
    #region var
    //Public
    public List<StateForSurvival> LesDataPourSurvie = new List<StateForSurvival>();
    public AnimationCurve PerteChaleur;
    [HideInInspector] public enum TypeOfDammage
    {
        Coup,
        Balle,
        Chute

    }
    public TypeOfDammage LesDammages;
    #region value vetements
    public float ResistanceFroidsTotal;
    public float ResistanceDegatsTotal;
    
    #endregion

    //private

    [SerializeField] float TemperatureBeginLoseLife = 28;
    [SerializeField] float MultiplayerBehind37 = 0.2f;
    int IndexDataVie = 0;
    int IndexDataFrost = 0;
    int IndexDataHunger = 0;
    int IndexDataSoif = 0;
    InfoExelvetements[] LesVetementsQueJePorte;
    private Temperature TemperatureExt;
    [Header("FX value")]
    [SerializeField] float TemperatureValueForEffect;
    [SerializeField] float LifeValueForEffect;
    #endregion
    private void Start()
    {
        TemperatureExt = temperatureScript();
        ToutSetForGood();// lance le set 
    }
    void ToutSetForGood() // set les datas
    {
        for (int i = 0; i < LesDataPourSurvie.Count; i++)
        {
            LesDataPourSurvie[i].Index = i;//met l'index
            if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Vie)
            {
                IndexDataVie = i; // enregistre l'index de la vie
            }
            else if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Heat)
            {
                IndexDataFrost = i;
            }
            else if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Hunger)
            {
                IndexDataHunger = i;
            }
            else if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Soif)
            {
                IndexDataSoif = i;
            }
        }
        
    }
    void Update()
    {
        baisseLesDatas(); // envois la baisse des stats de survie
        SetFeedback();// Selon les valeurs calculer envois les feedbacks
        UpdateFeedBack();// Update le feedback 
        UpdateUI();
    }
    void baisseLesDatas()// fais baisser les datas
    {
        for (int i = 0; i < LesDataPourSurvie.Count; i++)// boucle pour chaque datas
        {

            LosingLifeByData(LesDataPourSurvie[i]);//envois la perte de vie
            if (i != IndexDataVie)// si je ne suis pas dans la vie
            {
                if ((LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Soif || LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Hunger) && LesDataPourSurvie[i].ActualValue > 0) //si les datas sont sup�rieur a 0
                {
                    LesDataPourSurvie[i].ActualValue -= CalculPerteFaimEtSoi(LesDataPourSurvie[i]) * Time.deltaTime; // baisse la faim et la soif
                }
                else // si i= heat
                {
                    if (LesDataPourSurvie[i].ActualValue > 37)//si j'ai une temp�rature sup�rieur � la normal
                    {
                        LesDataPourSurvie[i].ActualValue -= calculPerteChaleur(LesDataPourSurvie[i]) * Time.deltaTime * MultiplayerBehind37; //calcul 
                    }
                    else
                    {
                        LesDataPourSurvie[i].ActualValue -= calculPerteChaleur(LesDataPourSurvie[i]) * Time.deltaTime; //calcul
                    }

                }
            }
            LesDataPourSurvie[i].ActualValue = checkLaRange(i);// v�rifie qu'il n'y a pas de d�passement de valeur
        }// selon les datas de survie
    }
    #region calcul perte des datas
    float CalculPerteFaimEtSoi(StateForSurvival MonState)
    {

        return MonState.PerteByPourcentage.Evaluate(MonState.ActualValue / MonState.Range.y) * MonState.PerteMaxByFrame;
        //return 0; 
    }
    float calculPerteChaleur(StateForSurvival MonState)
    {

        return ((MonState.ActualValue - TemperatureExt.temperature) - ResistanceFroidsTotal) *
             MonState.PerteByPourcentage.Evaluate((MonState.ActualValue - MonState.Range.x  ) / (MonState.Range.y - MonState.Range.x)) * MonState.PerteMaxByFrame;//!\ modifié 


    }
    #endregion
    #region calcul de perte de vie
    void LosingLifeByData(StateForSurvival MonState)
    {
        if (((MonState.SurvivalData == StateForSurvival.PointDeSurvie.Soif || MonState.SurvivalData == StateForSurvival.PointDeSurvie.Hunger) && MonState.ActualValue <= MonState.Range.x)
                   || (MonState.SurvivalData == StateForSurvival.PointDeSurvie.Heat && MonState.ActualValue <= TemperatureBeginLoseLife))// si les states qui font perdre de la vue ont leur valeur
        {
            if (MonState.SurvivalData != StateForSurvival.PointDeSurvie.Heat)
            {
                LesDataPourSurvie[IndexDataVie].ActualValue -= MonState.DecroissementVieUnderMinima * Time.deltaTime; // fais baisser la vie selon soif/faim  
            }
            else
            {
                LesDataPourSurvie[IndexDataVie].ActualValue -= calculPerteLifeHeat(MonState) * Time.deltaTime; // fais baisser selon la chaleur
            }
            LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);// v�rifie que la valeur est bien dans la range
        }

    }
    float calculPerteLifeHeat(StateForSurvival MonState)
    {
        return MonState.DecroissementVieUnderMinima *
            PerteChaleur.Evaluate(( MonState.ActualValue - MonState.Range.x) /       //!\ modifié 
                (TemperatureBeginLoseLife - MonState.Range.x));
    }
    #endregion

    #region FeedBack
    void SetFeedback()
    {
        if (GetState(IndexDataVie).ActualValue< LifeValueForEffect && !PostprocessChange.PostprocessScript.IsCibleAlreadySet(PostprocessChange.Effect.Life))
        {
            PostprocessChange.PostprocessScript.PutDammageVolume();
        }
        else if (GetState(IndexDataVie).ActualValue > LifeValueForEffect && PostprocessChange.PostprocessScript.IsCibleAlreadySet(PostprocessChange.Effect.Life))
        {
            PostprocessChange.PostprocessScript.RemoveDammageVolume();
        }
        else if (GetState(StateForSurvival.PointDeSurvie.Heat).ActualValue < TemperatureValueForEffect && !PostprocessChange.PostprocessScript.IsCibleAlreadySet(PostprocessChange.Effect.Life) && !PostprocessChange.PostprocessScript.IsCibleAlreadySet(PostprocessChange.Effect.Frost))
        {
            PostprocessChange.PostprocessScript.PutIceVolume();
        }
        else if (GetState(StateForSurvival.PointDeSurvie.Heat).ActualValue > TemperatureValueForEffect && PostprocessChange.PostprocessScript.IsCibleAlreadySet(PostprocessChange.Effect.Frost))
        {
            PostprocessChange.PostprocessScript.RemoveIceVolume();
        }
    }
    
    void UpdateFeedBack() 
    {
        if (PostprocessChange.PostprocessScript.IsCibleAlreadySet(PostprocessChange.Effect.Life))
        {
            PostprocessChange.PostprocessScript.SetWeight(PostprocessChange.Effect.Life, 1/PourcentageBetweenValue(LesDataPourSurvie[IndexDataVie].Range.x, LifeValueForEffect, LesDataPourSurvie[IndexDataVie].ActualValue ));
        }
        else if (PostprocessChange.PostprocessScript.IsCibleAlreadySet(PostprocessChange.Effect.Frost))
        {
            PostprocessChange.PostprocessScript.SetWeight(PostprocessChange.Effect.Frost, 1/PourcentageBetweenValue(LesDataPourSurvie[IndexDataFrost].Range.x, LifeValueForEffect, LesDataPourSurvie[IndexDataFrost].ActualValue));
        } 
    }

    #endregion

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
    private void UpdateUI()
    {
        Canvas myCanva = CanvasReference._canvasReference.GetCanva();
        myCanva.transform.Find("SurvivalUI").transform.Find("HungerBar").GetComponent<Slider>().value =
            PourcentageState(IndexDataHunger);
        
        myCanva.transform.Find("SurvivalUI").transform.Find("ThirstBar").GetComponent<Slider>().value =
            PourcentageState(IndexDataSoif);

        myCanva.transform.Find("SurvivalUI").transform.Find("HealthBar").GetComponent<Slider>().value =
            PourcentageState(IndexDataVie);

        myCanva.transform.Find("SurvivalUI").transform.Find("TempBar").GetComponent<Slider>().value =
            PourcentageState(IndexDataFrost);
    }


    #region method accessible à tous
    public float PourcentageState(StateForSurvival.PointDeSurvie State) 
    {
        int index = StateForSurvivalIndex(State);
        return (LesDataPourSurvie[index].ActualValue - LesDataPourSurvie[index].Range.x) / (LesDataPourSurvie[index].Range.y - LesDataPourSurvie[index].Range.x);//LesDataPourSurvie[index].ActualValue
    }
    public float PourcentageState(int index)
    {
        
        return (LesDataPourSurvie[index].ActualValue - LesDataPourSurvie[index].Range.x) / (LesDataPourSurvie[index].Range.y - LesDataPourSurvie[index].Range.x);//LesDataPourSurvie[index].ActualValue
    }
    public int StateForSurvivalIndex(StateForSurvival.PointDeSurvie State) 
    {
        int toReturn = 0;
        foreach (StateForSurvival item in LesDataPourSurvie)
        {
            if (item.SurvivalData == State)
            {
                toReturn = item.Index;
                break;
            }
        }
        return toReturn;
    }
    public void GetFood(Vector4 value)
    {
        for (int i = 0; i < 4; i++)
        {
            ChangementDuneDataDeSurvie(value[i], i);
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
                    if (item.ActualValue > 37)
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
   
    public void TakeDamage(float dammageBrut, TypeOfDammage CeQuiMeFaitMal)
    {

        if (TypeOfDammage.Coup == CeQuiMeFaitMal)
        {
            print("A remplir");
        }
        else if (TypeOfDammage.Balle == CeQuiMeFaitMal)
        {
            LesDataPourSurvie[IndexDataVie].ActualValue -= dammageBrut;
        }
        else
        {
            //degat par metre de chute
            print("A remplir");
        }

    }
    public void Death() 
    {
    
    
    }
    public StateForSurvival GetState(int index) 
    {
        
        return LesDataPourSurvie[index];
    
    }
    public StateForSurvival GetState(StateForSurvival.PointDeSurvie State) 
    {
        StateForSurvival toReturn = new StateForSurvival();
        foreach (StateForSurvival item in LesDataPourSurvie)
        {
            if (item.SurvivalData == State)
            {
                toReturn = item;
                break;
            }
        }
        return toReturn;
    }
    #endregion
}





















//state survival
//state surviva
/*public float VitesseDecroissement;

   public float PourcentageDecroissement;
  */
/*
 if (LesDataPourSurvie[i].applyDecroissement)// si j'applique le decroissement
                {
                    //
                    // le d�croissement normal
                    //    
                    
                    LesDataPourSurvie[i].ActualValue -= LesDataPourSurvie[i].VitesseDecroissement * LesDataPourSurvie[i].PourcentageDecroissement* Time.deltaTime; // baisse une des valeurs de survie
                    
                    
                    LesDataPourSurvie[i].ActualValue = checkLaRange(LesDataPourSurvie[i].Index); // v�rifie que la valeur reste bien dans le carcant 

                    //
                    // le d�croissement lorsqu'une valeur est dans la zone pour faire perdre de la vie
                    //  
                    if (LesDataPourSurvie[i].ActualValue == 0 && (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Hunger || LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Soif)) // si la soif ou la faim est � 0
                    {

                        
                            LesDataPourSurvie[IndexDataVie].ActualValue -= LesDataPourSurvie[i].DecroissementVieUnderMinima * Time.deltaTime;
                        
                       
                        LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);// v�rifie que la valeur reste bien dans le carcant 
                    }
                    if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Heat && LesDataPourSurvie[i].ActualValue<TemperatureBeginLoseLife)// si je suis en dessous de la chaleur
                    {
                        LesDataPourSurvie[IndexDataVie].ActualValue -= calculPerteLife(i);// je calcul la perte de vie selon la valeur
                        LesDataPourSurvie[i].ActualValue = checkLaRange(LesDataPourSurvie[i].Index);
                        LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);
                    }
                }*/

/* public void SetVetements(InfoExelvetements Info, int IndexEmplacement)
    {
        if (IndexEmplacement > NombreDemplacementPourVetement || IndexEmplacement < 0) // viens de retirer un -1
        {
            //print("L'emplacement Demander n'existe pas");
        }
        else
        {
            if (LesVetementsQueJePorte[IndexEmplacement].MaCategorie == Info.MaCategorie || !LesVetementsQueJePorte[IndexEmplacement].IsWeared)
            {

                ResistanceDegatsTotal -= LesVetementsQueJePorte[IndexEmplacement].DegatResistance;
                ResistanceFroidsTotal -= LesVetementsQueJePorte[IndexEmplacement].ChaleurResistance;
                ResistanceDegatsTotal += Info.DegatResistance;
                ResistanceFroidsTotal += Info.ChaleurResistance;
                LesVetementsQueJePorte[IndexEmplacement] = Info;
            }
        }

    }*/