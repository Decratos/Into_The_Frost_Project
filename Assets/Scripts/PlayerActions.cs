using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public void Gather(RaycastHit hit, GestionDesScipt ScriptGestion)
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
                ScriptGestion.Inventory.AddItem(new ItemClass{itemType = ResumeExelForObject.Type.Materials, amount = amount, spriteId = objectInfo.ID}, objectInfo, name);
                hit.transform.GetComponent<BasicRessourcesSource>().ReduceDurability();
                Instantiate(ItemAssets.ItemAssetsInstance.GetComponent<ParticlesAssets>().particles[0], hit.point, PlayerSingleton.playerInstance.transform.rotation);
            }
            else
            {
                print("Je n'ai pas l'outil nécessaire");
            }
            
        }
    }

    public void Collect(RaycastHit hit, GestionDesScipt ScriptGestion)
    {
       
            InfoGlobalExel objectInfo = new InfoGlobalExel();
            ResumeExelForObject stats = hit.transform.GetComponent<ResumeExelForObject>();
            liseurExel.LesDatas.FindObjectInfo(stats.ID, out objectInfo);
            ItemClass itemWorld = hit.transform.GetComponent<ItemWorld>().GetItem();
            ScriptGestion.Inventory.AddItem(new ItemClass{itemType = stats.MainType, amount = itemWorld.amount, spriteId = itemWorld.spriteId}, objectInfo, hit.transform.name);
            Destroy(hit.transform.gameObject);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Collecting/TakeItem", hit.point);
            GetComponent<Animator>().Play("TakeItem");
    }

    public void Attack()
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
}
