using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerActions : MonoBehaviour
{
    public TextMeshProUGUI ObjectText;

    private void Start()
    {
        ObjectText = CanvasReference._canvasReference.GetCanva().transform.Find("ObjectText").GetComponent<TextMeshProUGUI>();
    }
    public void Gather(RaycastHit hit, GestionDesScipt ScriptGestion) // récolte
    {
        if(GetComponentInChildren<WeaponSystem>().actualWeaponInHands)
        {
            
            string name = hit.transform.GetComponent<BasicRessourcesSource>().ressourceName;
            int amount = hit.transform.GetComponent<BasicRessourcesSource>().ressourceAmount;
            if(name == "Bois" && GetComponentInChildren<WeaponSystem>().actualWeaponInHands.canCutWood || name == "Rocher" && GetComponentInChildren<WeaponSystem>().actualWeaponInHands.canCutStone)
            {
                switch (name)
                {
                    case "Bois":
                        FMODUnity.RuntimeManager.PlayOneShot("event:/Collecting/GatheringWood", hit.point);
                    break;
                    case "Rocher":
                        FMODUnity.RuntimeManager.PlayOneShot("event:/Collecting/GatheringStone", hit.point);
                    break;
                }
                InfoGlobalExel objectInfo = new InfoGlobalExel();
                liseurExel.LesDatas.FindObjectInfo(name, out objectInfo);
                var newItem = new ItemClass {globalInfo = objectInfo, amount = amount };
                ScriptGestion.Inventory.CheckCapability(newItem, hit.transform.gameObject);
                hit.transform.GetComponent<BasicRessourcesSource>().ReduceDurability();
                Instantiate(ItemAssets.ItemAssetsInstance.GetComponent<ParticlesAssets>().particles[0], hit.point, PlayerSingleton.playerInstance.transform.rotation);
            }
            else
            {
                print("Je n'ai pas l'outil nécessaire");
            }
            
        }
    }

    public void Collect(RaycastHit hit, GestionDesScipt ScriptGestion)//récupére l'objet
    {
       
            InfoGlobalExel objectInfo = new InfoGlobalExel();
            ResumeExelForObject stats = hit.transform.GetComponent<ResumeExelForObject>();
            liseurExel.LesDatas.FindObjectInfo(stats.ID, out objectInfo);
            ItemClass itemWorld = hit.transform.GetComponent<ItemWorld>().GetItem();
            ScriptGestion.Inventory.CheckCapability(new ItemClass{globalInfo = objectInfo}, hit.transform.gameObject);
            Destroy(hit.transform.gameObject);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Collecting/TakeItem", hit.point);
            GetComponent<Animator>().Play("TakeItem");
    }

    public void Attack()// il me semble qu'il y'a un deuxiéme void attack
    {
        if(GetComponentInChildren<WeaponSystem>().actualWeaponInHands)
        {
            var wp = GetComponentInChildren<WeaponSystem>().actualWeaponInHands;
            wp.Shoot(wp);
        }
        /*else
        {
            if(hit.transform.GetComponent<AIstats>())
                hit.transform.GetComponent<AIstats>().ReduceHealth(5);
        }*/
        

    }

    private void Update()
    {
       Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
       if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
       {
            if(hit.transform.GetComponent<ItemWorld>())
            {
                ObjectText.text = hit.transform.GetComponent<ItemWorld>().GetItem().globalInfo.Name;
            }
            else
            {
                ObjectText.text = "";
            }
       }
        else
        {
            ObjectText.text = "";
        }
    }
}
