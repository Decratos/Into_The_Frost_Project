using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemClass
{
   public InfoGlobalExel globalInfo;
   public string itemName;
   public int spriteId;

   public ResumeExelForObject.Type itemType;
   public int amount;
   

   public Sprite GetSprite() // Fonction permettant d'afficher la bonne image
   {
      Debug.Log("Mon id :" +spriteId);
      if(spriteId != 0)
         return ItemAssets.ItemAssetsInstance.sprites[spriteId-1];
      else
      {
         return ItemAssets.ItemAssetsInstance.sprites[spriteId];
      }
   }

   public bool isStackable()
   {
      switch (itemType)
      {
         default: 
         case ResumeExelForObject.Type.Nourriture:
         case ResumeExelForObject.Type.Soins:
         case ResumeExelForObject.Type.Materials:
            return true;
         case ResumeExelForObject.Type.ArmeAfeu:
            return false;
      }
   }
}
