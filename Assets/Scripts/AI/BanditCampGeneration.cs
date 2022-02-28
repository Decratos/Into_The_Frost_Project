using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditCampGeneration : MonoBehaviour
{
    Collider myCollider;
    public enum CampSize
    {
        Small,
        Medium,
        Large
    }

    [SerializeField] private CampSize size;
    [SerializeField] private GameObject banditPrefab;
    [HideInInspector] private GameObject chestPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        myCollider = GetComponent<Collider>();
        int numberOfBandits;
        var min = myCollider.bounds.min;
        var max = myCollider.bounds.max;
        switch (size)
        {
            case CampSize.Small:
                numberOfBandits = 2;
            break;
            case CampSize.Medium:
                numberOfBandits = 4;
            break;
            case CampSize.Large:
                numberOfBandits = 6;
            break;
            default:
                numberOfBandits = 0;
            break;
        }

        for (int i = 0; i < numberOfBandits; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(min.x, max.x), transform.position.y, Random.Range(min.z, max.z));
            var bandit = Instantiate(banditPrefab, spawnPosition, Quaternion.identity);
            bandit.GetComponent<AIBehaviour>().CampPosition = this.transform.position;
            bandit.GetComponent<AIBehaviour>().isFromACamp = true;
        }
    }
}
