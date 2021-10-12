using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIstats : MonoBehaviour
{
    [SerializeField] private float health;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            GetComponent<AiDropItem>().DropItem();
            Destroy(gameObject);
        }
    }

    public void ReduceHealth(int dmg)
    {
        health -= dmg;
    }
}
