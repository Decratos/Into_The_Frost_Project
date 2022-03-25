using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class RespawnDistance 
{
    public enum Distance 
    {
    Same,
    courte,
    moyen,
    longue
    
    }
    public Distance LaDitance;
    public Vector2 Interval;

}

public class ResPawn : MesFonctions
{
    [SerializeField] RespawnDistance[] LesInfosDeDistance = new RespawnDistance[4];
    [SerializeField] string RespawnLocationTag;
    [SerializeField] string RespawnTag;
    
    //[SerializeField] float RayonAugmentation;
    public SurvivalSysteme.TypeOfDammage TypeDeDommageQuiATuer;
    public enum WhereAmIDead 
    {
    
    Base,
    Exterrior,
    EnnemieBase


    }
    public WhereAmIDead FinalPlace;

    GestionDesScipt gestion;
    
    [SerializeField] float ChaleurRespawn;
    Vector3 PositionOfDeath;
    Vector3 Position4Respawn;
   
    Vector2 distanceRespawn;
    
    private void Start()
    {
        FindGestionDesScripts(this.gameObject, out gestion);
        checkSiLesInfosDistanceSontBonne();
    }

    void checkSiLesInfosDistanceSontBonne() 
    {
       
        if (LesInfosDeDistance.Length!= System.Enum.GetNames(typeof(RespawnDistance.Distance)).Length) // si j'ai bien le nombre d'info necessaire
        {
            print("Jai un gros problème");
        }
    
    }

    public void SetCauseOfDeath(SurvivalSysteme.TypeOfDammage CeQuiMaTuer) 
    {
        TypeDeDommageQuiATuer = CeQuiMaTuer;
    }

    public void SetPlaceOfDeath(WhereAmIDead Place) 
    {

        FinalPlace = Place;
    }
    
    public void LanceLeRespawn() 
    {
        
        PositionOfDeath = transform.position;
        print("position done");
        
        distanceRespawn = SetDitance(ChooseDistance(TypeDeDommageQuiATuer, FinalPlace));
        print("Distance done");
        
        Position4Respawn = PositionForRespawn(PositionOfDeath);
        print("Position du respawn Done");
        
        resetStats();

        transform.position = Position4Respawn;
    }
    
    Vector3 PositionForRespawn(Vector3 PositionRef) 
    {
        
        GameObject ObjetDeRespawn = null;
        Vector3 toReturn = PositionRef;
        Collider[] LesObjetsTrouver = Physics.OverlapSphere(PositionRef, distanceRespawn.y);
        List<GameObject> LesObjetsATrier = new List<GameObject>();
        foreach (Collider item in LesObjetsTrouver)
        {
            if (RespawnLocationTag == item.gameObject.tag && Vector3.Distance(PositionRef, item.transform.position)> distanceRespawn.x)
            {
                LesObjetsATrier.Add(item.gameObject); 
            }
        }
        ObjetDeRespawn = LesObjetsATrier[Random.Range(0,LesObjetsATrier.Count)];
        print("objet respawn" + LesObjetsATrier.Count);
        List<GameObject> LesSpawnPoint = new List<GameObject>();
        List<Transform> ToLook = new List<Transform>();
        List<Transform> Looked = new List<Transform>();
        Transform toCheck = ObjetDeRespawn.transform;
        bool checkedChild = false;
        if (ObjetDeRespawn != null)
        {
            
            while (!checkedChild)
            {
                print("J'ai objet");
                if (toCheck.tag == RespawnTag && !LesSpawnPoint.Contains(toCheck.gameObject))
                {
                    print("j'ajoute un objet a spawnpoint");
                    LesSpawnPoint.Add(toCheck.gameObject);
                }
                if (toCheck.childCount > 0)
                {
                    print("L'objet que je check" + toCheck + "à des enfants");
                    foreach (Transform childDirect in toCheck)
                    {
                        if (childDirect.childCount > 0)
                        {
                            print("je vois que l'enfant un enfant et je l'ajoute");
                            ToLook.Add(childDirect);
                        }
                        else
                        {
                            print("l'enfant n'a pas d'enfant");
                            Looked.Add(childDirect);
                            if (childDirect.tag == RespawnTag && !LesSpawnPoint.Contains(childDirect.gameObject))
                            {
                                print("l'enfant à le bon tag");
                                LesSpawnPoint.Add(childDirect.gameObject);
                            }
                        }


                    }
                    if (ToLook.Contains(toCheck))
                    {
                        
                        ToLook.Remove(toCheck);
                        if (!Looked.Contains(toCheck))
                        {
                            
                            Looked.Add(toCheck);
                        }
                    }

                }
                if (ToLook.Count > 0)
                {
                    print("Je continue a chercher");
                    toCheck = ToLook[0];
                }
                else
                {
                    
                    checkedChild = true;
                }

            }
        }
        else 
        {
            print("Je n'ai pas trouver de respawn");
        
        }
        print("spawnpoint" + LesSpawnPoint.Count);
        if (LesSpawnPoint.Count>0)
        {
            
            toReturn = LesSpawnPoint[Random.Range(0, LesSpawnPoint.Count)].transform.position;
        }
        
        return toReturn;
    }
    
    RespawnDistance.Distance ChooseDistance(SurvivalSysteme.TypeOfDammage CeQuiMaTuer, WhereAmIDead Place) 
    {
        RespawnDistance.Distance toReturn = RespawnDistance.Distance.Same;
        if (Place == WhereAmIDead.Base)
        {
            if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Balle)
            {
                toReturn = RespawnDistance.Distance.courte;
                
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Coup)
            {
                toReturn = RespawnDistance.Distance.courte;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Chaleur)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Chute)
            {
                toReturn = RespawnDistance.Distance.longue;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Soif)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.faim)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
            
        }
        else if (Place == WhereAmIDead.EnnemieBase)
        {
            if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Balle)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Coup)
            {
                toReturn = RespawnDistance.Distance.longue;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Chaleur)
            {
                toReturn = RespawnDistance.Distance.longue;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Chute)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Soif)
            {
                toReturn = RespawnDistance.Distance.longue;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.faim)
            {
                toReturn = RespawnDistance.Distance.longue;
            }
        }
        else if (Place == WhereAmIDead.Exterrior)
        {
            if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Balle)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Coup)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Chaleur)
            {
                toReturn = RespawnDistance.Distance.courte;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Chute)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.Soif)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
            else if (CeQuiMaTuer == SurvivalSysteme.TypeOfDammage.faim)
            {
                toReturn = RespawnDistance.Distance.moyen;
            }
        }
        return toReturn;
    }

    Vector2 SetDitance(RespawnDistance.Distance LaDitanceVoulue) 
    {
        Vector2 toReturn = Vector2.zero;
        foreach (RespawnDistance item in LesInfosDeDistance)
        {
            if (LaDitanceVoulue == item.LaDitance)
            {
                toReturn = new Vector2(item.Interval.x,item.Interval.y);
            }
        }
        return toReturn;
    }
    void resetStats() 
    {

        foreach (StateForSurvival state in gestion.SurvieScript.LesDataPourSurvie)
        {
            state.ActualValue = state.Range.y;
            if (state.SurvivalData== StateForSurvival.PointDeSurvie.Heat)
            {
                state.ActualValue = ChaleurRespawn;
            }
        }
        
        
    
    }
    void resetInventory() 
    {

        print("Laissez ça a T");

    }

    [Button(ButtonSizes.Medium)]
    void Simulation() 
    {
        TypeDeDommageQuiATuer = SurvivalSysteme.TypeOfDammage.Coup;
        FinalPlace = WhereAmIDead.Base;
        LanceLeRespawn();
        
    }

    
}
/*while (!AiJeTrouveUnSpawn)
        {
            // fais une position sur le cercle 
            // Si je trouve un objet correspondant  
            toReturn = PositionRef + new Vector3(distanceRespawn, 0, distanceRespawn).normalized * distanceRespawn;
            RaycastHit hit;
            if (Physics.Raycast(toReturn, Vector3.down,out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == RespawnLocationTag)
                {
                    ObjetDeRespawn = hit.transform.gameObject;
                    AiJeTrouveUnSpawn = true;
                }
            }

        }*/
//trouver tous les objets qui ont le bon tag