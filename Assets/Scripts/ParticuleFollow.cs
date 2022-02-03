using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticuleFollow : MonoBehaviour
{
    [SerializeField] Vector3 Decalage;
    [SerializeField] Transform Joueur;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Joueur.position + Decalage;
    }
}
