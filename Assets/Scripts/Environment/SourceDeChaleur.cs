using UnityEngine;

public class SourceDeChaleur : MonoBehaviour
{
    
    public float Decalage;
    public float TempsCalculSysteme;
    public bool DoByUpdate;
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
            if (DoByUpdate)
            {
                LaGestion.SurvieScript.ChangementDuneDataDeSurvie(calcul() * Time.deltaTime, 3);
                if (once)
                {
                    once = false;
                }
            }
            else if (!once )
            {
                once = true;
                AugmenteChaleur();
            }  
            
        }
       
        
    }

    void AugmenteChaleur() 
    {
        if (playerInZone)
        {
            LaGestion.SurvieScript.ChangementDuneDataDeSurvie(calcul() * Time.deltaTime, 3);
            Invoke("AugmenteChaleur",TempsCalculSysteme);
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
