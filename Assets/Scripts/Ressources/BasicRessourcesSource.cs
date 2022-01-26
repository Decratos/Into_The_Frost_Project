using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRessourcesSource : MonoBehaviour
{
    public string ressourceName;
    public int ressourceAmount;
    [SerializeField] private int durability;

    public void ReduceDurability()// r�duits la durabilit�
    {
        durability -= ressourceAmount;
        if(durability <= 0)
        {
            Destroy(gameObject);
        }
    }
}
