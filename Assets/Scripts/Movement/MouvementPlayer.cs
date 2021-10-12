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
       
        MyCharacterController = GetComponent<CharacterController>();
        ActualVitesseWanted = VitesseDeplacement;
    }
    

    public void Mouvement(Vector2 Direction) 
    {
        DirectionPerso = Direction;
        

    } 

    public void Saut() 
    {
        if (MyCharacterController.isGrounded)
        {
            print("je change");
            
            directionSaut = DirectionCalculate;
            Jumping = true;

        }
        else
        {
            print("Je remet la gravité");
            
        }

    }

    public void GoSprint() 
    {

        ActualVitesseWanted = VitesseSprint;

    }
    public void Walk() 
    {

        ActualVitesseWanted = VitesseDeplacement;
        
    }

    private void Update()
    {
        // bouge 
        if (!Jumping)
        {
            DirectionCalculate = new Vector3(DirectionPerso.x, 0, DirectionPerso.y);
        }
        else 
        {
            DirectionCalculate = directionSaut;
        
        }
        MyCharacterController.Move(((Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * DirectionCalculate).normalized * ActualVitesseWanted  + CalulateVitesseY()) * Time.deltaTime);
        if (MyCharacterController.collisionFlags != 0)
        {
            detectUneCollision(((int)MyCharacterController.collisionFlags));
            SpeedY = 0;
        }
        
    }


    void detectUneCollision(int collision) 
    {
        if (Jumping)
        {
            if ((int)CollisionFlags.Below==collision )
            {
                print("Atteri");
                Jumping = false;
            }
        }
    
    }


    Vector3 CalulateVitesseY() 
    {
        Vector3 temp = Vector3.up;
        //set les datas nécessaire
        
        SpeedY -= Gravity.y * Time.deltaTime;
        if (Jumping)
        {

            temp *= VitesseSaut - SpeedY;
            

        }
        else 
        {

            temp = Gravity;
        }
        
        return temp;
        
        }



}



/* print("Saut");

    //print();*/

//calculer la proportionnel


/*private void OnDrawGizmos()
{
    Gizmos.color =debug.StepOffset;
    Gizmos.DrawLine(transform.position - (new Vector3(0,1,0)*((MyCharacterController.height/2)-MyCharacterController.stepOffset)),transform.forward);
    // draw ray hauteur
    // draw angle
    // skinwidth



}*/


/* if (ActualY>YEndJump)
   {
       ActualY = YEndJump;
   }
   if (Jumping)
   {

       float proportionnel = 1- (YEndJump - ActualY) /(YEndJump - YStartJump) ;
       Toreturn.y *= proportionnel;

   }
   else 
   {
       float proportionnel = 0;
       if ((YEndJump - ActualY)<0.1)
       {
           proportionnel = 0.1f;
       }
       else 
       {
         proportionnel =  (YEndJump - ActualY) / (YEndJump - YStartJump);

       }

       Toreturn.y *= proportionnel;
   }*/


/*if (Jumping)
    {
        print("je jump");
        Ray MonRay = new Ray((transform.position - new Vector3(0, 1, 0) * (MyCharacterController.height / 2)), Vector3.down);
        if (!Physics.Raycast(MonRay, HauteurSaut))
        {

            Jumping = false;
            vitesseY = Gravity;
        }
    }*/
//transform.Translate(new Vector3(test.x, 0, test.y)*Time.deltaTime*ActualVitesseWanted);

