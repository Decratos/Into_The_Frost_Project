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

public class MouvementPlayer : MonoBehaviour
{
    //public
    [Header("Saut")]
    public float VitesseSaut;
    public float multiplicateurEffetSaut;
    public float HauteurSaut;
    [Header("Déplacement")]
    public float VitesseDeplacement;
    public float VitesseSprint;
    [Header("autre")]
    public DebugVisualCharacter debug;
    public Vector3 Gravity = new Vector3(0,-9.81f,0) ;
    public bool Jumping = false;
    //local

    Vector2 DirectionPerso;
    float ActualVitesseWanted;
    CharacterController MyCharacterController;


    #region Pour Deplacement
    
    
    Vector3 DirectionCalculate;
    Vector3 directionSaut;
    float SpeedY;

    
    #endregion

    void Start() 
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
            print("je change");
            
            directionSaut = DirectionCalculate; // lance la direction du saut
            Jumping = true;// permet de savoir s'il saute

        }
        else
        {
            print("Je remet la gravité");
            
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
                print("Atteri");
                Jumping = false;// n'est plus en saut
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



}





