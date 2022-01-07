using UnityEngine;

public class SourceDeChaleur : MonoBehaviour
{
    
    public float Decalage;
    public float TempsCalculSysteme;
    
    [SerializeField] Vector2 Values;
    bool playerInZone = false;
    float tailleCollider;
    Transform player;
    GestionDesScipt LaGestion;
    bool once;

    private void Start()
    {
        tailleCollider = GetComponent<SphereCollider>().radius;
    }

    private void Update()
    {
        if (playerInZone)
        {
            
                LaGestion.SurvieScript.ChangementDuneDataDeSurvie(calcul() * Time.deltaTime,StateForSurvival.PointDeSurvie.Heat);
                
        }
       
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("Je rentre");
            playerInZone = true;
            player = other.transform;
            if (LaGestion==null)
            {
                LaGestion = other.GetComponent<GestionDesScipt>();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            print("Je sors");
            playerInZone = false;
        }
    }

    float calcul() 
    {
        
        
        float pourcentage = Vector3.Distance(player.position, transform.position) / (tailleCollider - Decalage);
        float Difference = Values.y - Values.x;
        
        return Values.x+Difference*pourcentage;
    }
}
