using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemClass
{
    public InfoGlobalExel globalInfo;
    [Tooltip("Sert juste à savoir quel item possède le joueur dans l'inventory_debug")]
    public string name;

    public int amount;
    public float ChanceChoisis;
    public float ChanceDeBase;

   public Sprite GetSprite() //Fonction permettant d'afficher la bonne image
   {
      if(globalInfo.ID != 0)
         return ItemAssets.ItemAssetsInstance.sprites[globalInfo.ID- 1];
      else
      {
         return ItemAssets.ItemAssetsInstance.sprites[globalInfo.ID];
      }
   }

   public bool isStackable() 
   {
      switch (globalInfo.TypeGeneral)
      {
         default: 
         case InfoGlobalExel.Type.Nourriture:
         case InfoGlobalExel.Type.Soins:
         case InfoGlobalExel.Type.Materials:
            return true;
         case InfoGlobalExel.Type.ArmeAfeu:
            return false;
      }
   }
}
