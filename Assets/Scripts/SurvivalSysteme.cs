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
    public bool applyDecroissement; // value de test à supprimerun de ces 4
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
    [HideInInspector]public enum TypeOfDammage 
    {
    Coup,
    Balle,
    Chute

    }
    public TypeOfDammage LesDammages;
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
    private Temperature TemperatureExt;
    #endregion
    private void Start()
    {
        TemperatureExt = temperatureScript();
        ToutSetForGood();// lance le set 
    }
    void Update()
    {
            baisseLesDatas(); // envois la baisse des stats de survie
    }
    void baisseLesDatas()// fais baisser les datas
    {
        for (int i = 0; i < LesDataPourSurvie.Count; i++)
        {
            
            LosingLifeByData(LesDataPourSurvie[i]);//envois la perte de vie
            if (i!=IndexDataVie)
            {
                if ((LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Soif || LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Hunger) && LesDataPourSurvie[i].ActualValue > 0)
                {
                    LesDataPourSurvie[i].ActualValue = CalculPerteFaimEtSoi(LesDataPourSurvie[i]);
                }
                else
                {
                    LesDataPourSurvie[i].ActualValue = calculPerteChaleur(LesDataPourSurvie[i]);
                }
            }  
            LesDataPourSurvie[i].ActualValue = checkLaRange(i);
        }// selon les datas de survie
    }
        #region calcul perte des datas
    float CalculPerteFaimEtSoi(StateForSurvival MonState) 
    {
        
        MonState.ActualValue -= MonState.PerteByPourcentage.Evaluate(MonState.ActualValue/ MonState.Range.y) * MonState.PerteMaxByFrame;
        return 0;
    }

    float calculPerteChaleur (StateForSurvival MonState)
    {

        //= (MonState.PerteByPourcentage.Evaluate(%température min / max)*(PerteByFrame- ResistanceFroidsTotal);
        //MonState.ActualValue = MonState.PerteByPourcentage.Evaluate();// modifier le script temperature
        // me faut 

        return 0;
    }
        #endregion
        #region calcul de perte de vie
    void LosingLifeByData(StateForSurvival MonState) 
    {
        if (((MonState.SurvivalData == StateForSurvival.PointDeSurvie.Soif || MonState.SurvivalData == StateForSurvival.PointDeSurvie.Hunger) && MonState.ActualValue <= TemperatureBeginLoseLife)
                   || (MonState.SurvivalData == StateForSurvival.PointDeSurvie.Heat && MonState.ActualValue <= TemperatureBeginLoseLife))// si les states qui font perdre de la vue ont leur valeur
        {
            if (MonState.SurvivalData != StateForSurvival.PointDeSurvie.Heat)
            {
                MonState.ActualValue -= MonState.DecroissementVieUnderMinima * Time.deltaTime; // fais baisser la vie selon soif/faim  
            }
            else
            {
                MonState.ActualValue -= calculPerteLifeHeat(MonState); // fais baisser selon la chaleur
            }
            LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);// vérifie que la valeur est bien dans la range
        }

    }
    float calculPerteLifeHeat(StateForSurvival MonState) 
   {       
        return MonState.DecroissementVieUnderMinima * 
            PerteChaleur.Evaluate( (TemperatureBeginLoseLife - MonState.ActualValue) / 
                (TemperatureBeginLoseLife - MonState.Range.x)); 
   }
        #endregion

    void ToutSetForGood() // set les datas
    {
        for (int i = 0; i < LesDataPourSurvie.Count; i++)
        {
            
            LesDataPourSurvie[i].Index = i;//met l'index
            if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Vie)
            {
                IndexDataVie = i; // enregistre l'index de la vie
            }
            
        }
        LesVetementsQueJePorte = new InfoExelvetements[NombreDemplacementPourVetement]; // A voir avec théo
        print("Commentaire à résoudre");
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
        if (IndexEmplacement > NombreDemplacementPourVetement || IndexEmplacement < 0) // viens de retirer un -1
        {
            //print("L'emplacement Demander n'existe pas");
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
   
    
    public void TakeDamage(float dammageBrut,TypeOfDammage CeQuiMeFaitMal) 
    {

        if (TypeOfDammage.Coup == CeQuiMeFaitMal)
        {

        }
        else if( TypeOfDammage.Balle == CeQuiMeFaitMal )
        {
            LesDataPourSurvie[IndexDataVie].ActualValue -= dammageBrut; 
        }
        else 
        {
            //degat par metre de chute
        }

    }
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
                    // le décroissement normal
                    //    
                    
                    LesDataPourSurvie[i].ActualValue -= LesDataPourSurvie[i].VitesseDecroissement * LesDataPourSurvie[i].PourcentageDecroissement* Time.deltaTime; // baisse une des valeurs de survie
                    
                    
                    LesDataPourSurvie[i].ActualValue = checkLaRange(LesDataPourSurvie[i].Index); // vérifie que la valeur reste bien dans le carcant 

                    //
                    // le décroissement lorsqu'une valeur est dans la zone pour faire perdre de la vie
                    //  
                    if (LesDataPourSurvie[i].ActualValue == 0 && (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Hunger || LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Soif)) // si la soif ou la faim est à 0
                    {

                        
                            LesDataPourSurvie[IndexDataVie].ActualValue -= LesDataPourSurvie[i].DecroissementVieUnderMinima * Time.deltaTime;
                        
                       
                        LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);// vérifie que la valeur reste bien dans le carcant 
                    }
                    if (LesDataPourSurvie[i].SurvivalData == StateForSurvival.PointDeSurvie.Heat && LesDataPourSurvie[i].ActualValue<TemperatureBeginLoseLife)// si je suis en dessous de la chaleur
                    {
                        LesDataPourSurvie[IndexDataVie].ActualValue -= calculPerteLife(i);// je calcul la perte de vie selon la valeur
                        LesDataPourSurvie[i].ActualValue = checkLaRange(LesDataPourSurvie[i].Index);
                        LesDataPourSurvie[IndexDataVie].ActualValue = checkLaRange(IndexDataVie);
                    }
                }*/