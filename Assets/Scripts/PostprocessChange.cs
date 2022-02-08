using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering.PostProcessing;




public class PostprocessChange : MesFonctions
{
    //Public
    [HideInInspector]public static PostprocessChange PostprocessScript;
    public VolumeProfile FroidsEffect;
    public VolumeProfile DouleursEffect;
    public enum Effect 
    {
    Frost,
    Life
    
    }
    
    // private
    private GameObject PostProcessObject;
    private Volume FrostBite;
    private Volume Dammage;



    // Start is called before the first frame update


    // Update is called once per frame
    private void Awake()
    {
        if (PostprocessScript!=this)
        {
            PostprocessScript = this;
        }
    }
    void Start()
    {
        PostProcessObject = FindPostProcessVolume();
    }

    public void PutIceVolume() 
    {

        FrostBite = PostProcessObject.AddComponent<Volume>();
        FrostBite.profile = FroidsEffect;
        
    }
    public void RemoveIceVolume() 
    {

        Destroy(FrostBite);

    }

    public void PutDammageVolume() 
    {
        if (FrostBite!=null)
        {
            RemoveIceVolume();
        }
        
        Dammage = PostProcessObject.AddComponent<Volume>();
        Dammage.profile = DouleursEffect;
    }

    public void RemoveDammageVolume() 
    {
        
        Destroy(Dammage);
    
    }

    public void SetWeight(Effect effetToChange, float newWeight) 
    {

        switch (effetToChange)
        {
            case  Effect.Frost:
                FrostBite.weight = newWeight;
                break;
            
            case Effect.Life:
                Dammage.weight = newWeight;
                break;
        }

    }
    public bool IsCibleAlreadySet(Effect EffetToCheck) 
    {

        if (EffetToCheck == Effect.Frost)
        {
            if (FrostBite!=null)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        else if (EffetToCheck == Effect.Life)
        {
            if (Dammage!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    

    }
}
