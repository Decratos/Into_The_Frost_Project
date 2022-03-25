using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DebugVisualCharacter 
{
    public Color ColorSkin;
    
    public Color SlopeLimit;
    public float LongueurLineSlope;
    public Color StepOffset;
    public float LongueurLineStep;
    public Color CubeOnGround;
    public float GrosseurCube;

}

public class MouvementPlayer : MesFonctions
{
    //public
    [Header("Saut & fall")]
    [Tooltip("")]
    public float VitesseSaut;
    public float VitesseChute;
    public Vector2 HighDegats;
    public Vector2 HighStop;
    public Vector2 TimeStop;

    [Header("Déplacement")]
    public float VitesseDeplacement;
    public float VitesseSprint;
    public float VitesseAccroupie;
    public float Acceleration;
    public float Deceleration;
   
    [Header("autre")]
    public DebugVisualCharacter debug;
    public Vector3 Gravity = new Vector3(0,-9.81f,0) ;
    public enum StateDeplacement 
    {
    Immobile,
    Marche,
    Cour,
    Saute,
    Fall,
    Atteri,
    Accroupie
    
    }
    public StateDeplacement StateDeDeplacement;

    //local
    
    Vector2 DirectionPerso;
    float ActualVitesseWanted;
    float ActualVitesse;
    float accelerationChute;
    CharacterController MyCharacterController;


    #region Pour Deplacement
    
    
    Vector3 DirectionCalculate;
    Vector3 directionSaut;
    Vector3 EnregistrementHigh;
    float SpeedY;
    bool ResteImmobile;
    
    #endregion

    void Start() 
    {
       
        MyCharacterController = GetComponent<CharacterController>(); // récupére le component character
        ChangementVitesseMax(VitesseDeplacement); // set la vitesse de déplacement
    }
    public void Mouvement(Vector2 Direction) // set la direction 
    {
        DirectionPerso = Direction;
        if (!ResteImmobile)
        {
            
            if (Direction == Vector2.zero)
            {
                StateChange(StateDeplacement.Immobile);
                GetComponent<Animator>().SetBool("IsWalking", false);
            }
            else if (Direction != Vector2.zero && StateDeDeplacement != StateDeplacement.Saute || StateDeDeplacement != StateDeplacement.Fall)
            {
                if (ActualVitesseWanted == VitesseDeplacement)
                {
                    StateChange(StateDeplacement.Marche);
                    GetComponent<Animator>().SetBool("IsWalking", true);
                }
                else if (ActualVitesseWanted == VitesseAccroupie)
                {
                    StateChange(StateDeplacement.Accroupie);
                }
                else if (ActualVitesseWanted == VitesseSprint)
                {
                    StateChange(StateDeplacement.Cour);
                }
            }
        }

       
        

    }

    private void Update()
    {
        
        if (!ResteImmobile)
        {
            
            if (StateDeDeplacement != StateDeplacement.Saute && StateDeDeplacement != StateDeplacement.Fall)// s'il ne saute pas
            {
                DirectionCalculate = new Vector3(DirectionPerso.x, 0, DirectionPerso.y); //change la direction
            }
            else
            {
                DirectionCalculate = directionSaut; // La direction du saut
            }
        }
        else
        {
            DirectionCalculate = Vector3.zero;
        }
        if (ActualVitesse != ActualVitesseWanted && SpeedMustChange())
        {
            calculDeLavitesse();
        }
        MyCharacterController.Move(((Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * DirectionCalculate).normalized * ActualVitesse + CalulateVitesseY()) * Time.deltaTime);// la ligne qui permet le déplacement
        
        //                              selon la rotation                       vas dans la direction voulue       normarliser  par la vitesse voulue   +la vitesse Y (saut ou chute)

            

        if (StateDeDeplacement == StateDeplacement.Saute)
        {
            if (transform.position.y > EnregistrementHigh.y)
            {
                EnregistrementHigh = transform.position;
            }
            else if (transform.position.y < EnregistrementHigh.y ) 
            {
                StateChange(StateDeplacement.Fall);
            }
        }
        if (MyCharacterController.collisionFlags != 0)
        {
            detectUneCollision(((int)MyCharacterController.collisionFlags));// vas me permettre de situer ou est la collision
            
        }
        if (!MyCharacterController.isGrounded && StateDeDeplacement != StateDeplacement.Saute )
        {
           
            StateChange(StateDeplacement.Fall);
        }
    }
    
    public void StateChange(StateDeplacement NewState) // gére les changements d'états
    {

        if (NewState!=StateDeDeplacement && MyCharacterController.isGrounded)// si mon personnage doit changer d'état
        {
            StateDeDeplacement = NewState;
            switch (NewState) 
            {
                case StateDeplacement.Immobile:
                    SuisImmobile();
                    break;

                case StateDeplacement.Marche:
                    Walk();
                    break;

                case StateDeplacement.Cour:
                    GoSprint();
                    break;

                case StateDeplacement.Saute:
                    GetComponent<Animator>().Play("Jump");
                    Saut();
                    break;
                case StateDeplacement.Fall:
                    GetComponent<Animator>().SetBool("IsFalling", true);
                    fall();
                    break;
                case StateDeplacement.Atteri:
                    GetComponent<Animator>().SetBool("IsFalling", false);
                    GetComponent<Animator>().Play("Landing");
                    atteri();
                    break;
                
                case StateDeplacement.Accroupie:
                    saccroupie();
                    break;
            }
        }
        else if(NewState != StateDeDeplacement && !MyCharacterController.isGrounded) 
        {
            switch (NewState)
            {
                
                case StateDeplacement.Fall:
                   
                    StateDeDeplacement = NewState;
                    
                    
                    fall();
                    break;

                case StateDeplacement.Cour:

                    GoSprint();

                    break;
                case StateDeplacement.Marche:
                    Walk();
                    break;
                default:
                    
                    break;
            }
        }
    }
    #region void par changement detat
    void SuisImmobile()
    {
        ActualVitesse = 0;
        
        // A remplir

    }
    public void Walk() // change la vitesse en celle de marche
    {

        ChangementVitesseMax(VitesseDeplacement);

    }
    public void Saut() //lance le saut
    {

        EnregistrementHigh = transform.position;
        directionSaut = DirectionCalculate; // lance la direction du saut
        
    }
    public void GoSprint() // change la vitesse en celle de sprint
    {

        ChangementVitesseMax(VitesseSprint);

    }
    void fall() 
    {
        if (EnregistrementHigh==Vector3.zero)
        {
            EnregistrementHigh = transform.position;
        }
        //enregistre à quelle coordonné le joueur chute
        
    }
    void saccroupie() 
    {
      ChangementVitesseMax(VitesseAccroupie);
    }
    void atteri() 
    {
        //print(MyCharacterController.velocity.magnitude); //
        directionSaut = Vector3.zero;
        float DistanceChute = Vector3.Distance(new Vector3(0,EnregistrementHigh.y,0), new Vector3(0,transform.position.y,0)); // Vector3.Distance(transform.position, EnregistrementHigh); // changement a faire : faire new vector avec le Y
        if (DistanceChute>HighStop.x)
        {
            GrosseChute(DistanceChute);
        }
        else 
        {
            ChuteSimple();
        }

        GestionDesScipt.ScriptGestion.MouvementDeCamera.resetAtLanding();
        
        //calculer au moment de l'impact selon la velocité faire
        // ChuteSimple ou GrosseChute
        EnregistrementHigh = Vector3.zero;
    }
    #endregion
    void ChuteSimple() 
    {
        ChangeStateAtterissage();
        
    }
    void GrosseChute(float DistanceChute) 
    {
        
        if (DistanceChute >= HighDegats.y)
        {
            
            GestionDesScipt.ScriptGestion.SurvieScript.Death(SurvivalSysteme.TypeOfDammage.Chute);
        }
        else 
        {
            if (DistanceChute > HighDegats.x)
            {
                float calcul = 100*((DistanceChute - HighDegats.x) /(HighDegats.y-HighDegats.x)) ;
                GestionDesScipt.ScriptGestion.SurvieScript.TakeDamage(calcul, SurvivalSysteme.TypeOfDammage.Chute);
            }
            
        }
        if (DistanceChute >= HighStop.x)
        {
            float calcul = 0;
            if (DistanceChute>=HighStop.y)
            {
                calcul = HighStop.y;
            }
            else 
            {
                calcul = TimeStop.x + ((TimeStop.y - TimeStop.x) * (DistanceChute - HighStop.x) / (HighStop.y - HighStop.x));
            }
            StateChange(StateDeplacement.Immobile);
            ResteImmobile = true;
            Invoke("ChangeStateAtterissage",calcul);
        }
        
    }
    void ChangeStateAtterissage() 
    {
        
        if (DirectionPerso != Vector2.zero)
        {
            if (VitesseDeplacement == ActualVitesseWanted)
            {
                StateChange(StateDeplacement.Marche);
            }
            else if (VitesseSprint == ActualVitesseWanted)
            {
                StateChange(StateDeplacement.Cour);
                
            }
            else if (VitesseAccroupie == ActualVitesseWanted)
            {
                StateChange(StateDeplacement.Accroupie);
            }
            
        }
        else 
        {
            StateChange(StateDeplacement.Immobile);
        }
        if (ResteImmobile)
        {
            ResteImmobile = false;
        }

    }
    void ChangementVitesseMax(float laVitesseQueJeVeux ) 
    {
        ActualVitesseWanted = laVitesseQueJeVeux; 
    }
    void calculDeLavitesse() // faire en sorte Qu'il Cape la vitesse s'il n'arréte pas de tourner autour;
    {
        if (ActualVitesse<ActualVitesseWanted)
        {
            ActualVitesse += Acceleration * Time.deltaTime;
        }
        else if (ActualVitesse>ActualVitesseWanted)
        {
            ActualVitesse -= Deceleration * Time.deltaTime;
        }
    }
    void detectUneCollision(int collision) // lorsque je detect une collision
    {
        if ((int)CollisionFlags.Below == collision)
        {
            if (StateDeDeplacement == StateDeplacement.Saute || StateDeDeplacement == StateDeplacement.Fall) //si je suis en saut
            {
                if ((int)CollisionFlags.Below == collision)// si la collision se situe en bas de la capsule
                {

                    StateChange(StateDeplacement.Atteri);

                }
            }
            else 
            {
                SpeedY = 0;
                accelerationChute = 0;
            }
           
        }
        
        
    
    }
    Vector3 CalulateVitesseY() // calcul de la vitesse Y
    {
        //set les datas nécessaire
        Vector3 temp = Vector3.up;
        SpeedY += Gravity.y * Time.deltaTime;// me souviens plus du pk delta time
        
        if (StateDeDeplacement == StateDeplacement.Saute) // si je suis entrain de sauter
        {

            temp *= VitesseSaut + SpeedY;// réduit  
            

        }

        else 
        {
            accelerationChute += Gravity.y * Time.deltaTime;
            temp = (Gravity*VitesseChute) + (accelerationChute*Vector3.up) ; // set la gravité back
        }
        
        return temp; //renvoie les le vector 3
        
        }
    bool SpeedMustChange() 
    {
        switch (StateDeDeplacement) 
        {
            case StateDeplacement.Immobile:
            case StateDeplacement.Saute:
            case StateDeplacement.Fall:
            case StateDeplacement.Atteri:
                return false;
            default:
                return true;
        
        }
        
    }
    
}


/*
 * void Start() 
    {
       
        MyCharacterController = GetComponent<CharacterController>(); // récupére le component character
        ActualVitesseWanted = VitesseDeplacement; // set la vitesse de déplacement
    }
    public void Mouvement(Vector2 Direction) // set la direction 
    {
        DirectionPerso = Direction;
        

    } 
    public void Saut() //lance le saut
    {
        if (MyCharacterController.isGrounded)// s'il le perso est au sol
        {
            //print("je change");
            
            directionSaut = DirectionCalculate; // lance la direction du saut
            Jumping = true;// permet de savoir s'il saute
            //GestionDesScipt.ScriptGestion.MouvementDeCamera.VerifSaut(); //Lance le debug
        }
        

    }
    public void GoSprint() // change la vitesse en celle de sprint
    {

        ActualVitesseWanted = VitesseSprint;

    }
    public void Walk() // change la vitesse en celle de marche
    {

        ActualVitesseWanted = VitesseDeplacement;
        
    }
    private void Update()
    {
        // bouge 
        if (!Jumping)// s'il ne saute pas
        {
            DirectionCalculate = new Vector3(DirectionPerso.x, 0, DirectionPerso.y); //change la direction
        }
        else 
        {
            DirectionCalculate = directionSaut;// La direction du saut
        
        }
        MyCharacterController.Move(((Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * DirectionCalculate).normalized * ActualVitesseWanted  + CalulateVitesseY()) * Time.deltaTime);// la ligne qui permet le déplacement
        //                              selon la rotation                       vas dans la direction voulue       normarliser  par la vitesse voulue   +la vitesse Y (saut ou chute)
        if (MyCharacterController.collisionFlags != 0)
        {
            detectUneCollision(((int)MyCharacterController.collisionFlags));// vas me permettre de situer ou est la collision
            SpeedY = 0;
        }
        
    }
    void detectUneCollision(int collision) // lorsque je detect une collision
    {
        if (Jumping) //si je suis en saut
        {
            if ((int)CollisionFlags.Below==collision )// si la collision se situe en bas de la capsule
            {
                //print("Atteri");
                Jumping = false;// n'est plus en saut
                //GestionDesScipt.ScriptGestion.MouvementDeCamera.VerifSaut(); //Lance le debug
            }
        }
    
    }
    Vector3 CalulateVitesseY() // calcul de la vitesse Y
    {
        Vector3 temp = Vector3.up;
        //set les datas nécessaire
        
        SpeedY -= Gravity.y * Time.deltaTime;// me souviens plus du pk delta time
        if (Jumping) // si je suis entrain de sauter
        {

            temp *= VitesseSaut - SpeedY;// réduit  
            

        }
        else 
        {

            temp = Gravity; // set la gravité back
        }
        
        return temp; //renvoie les le vector 3
        
        }
*/


