using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionPlace : MonoBehaviour
{
    public ResPawn.WhereAmIDead OuJeSuis;
    [SerializeField] string tagPlayer;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == tagPlayer && other.GetComponent<ResPawn>().FinalPlace != OuJeSuis)
        {

            other.GetComponent<ResPawn>().SetPlaceOfDeath(OuJeSuis);


        }
    }
}
