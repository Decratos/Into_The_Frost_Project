using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditAttack : MonoBehaviour
{
    public void Attack(Vector3 targetPosition, float attackDistance, float damage)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, (targetPosition - transform.position).normalized, out hit, attackDistance))
        {
            if(hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<SurvivalSysteme>().LesDataPourSurvie[0].ActualValue -= damage;
            }
            else if(hit.transform.CompareTag("NPC"))
            {
                hit.transform.GetComponent<AIstats>().ReduceHealth(damage);
            }
            else
            {
                print("Le tag de ma cible : " + hit.transform.tag);
                print("Le nom de ma cible : " + hit.transform.name);
            }
        }
    }
}
