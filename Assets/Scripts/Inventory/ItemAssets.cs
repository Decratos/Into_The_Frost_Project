using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemAssets : MonoBehaviour
{

    public static ItemAssets ItemAssetsInstance { get; private set; }
    [SerializeField] private int spritesArrayLenght;
    // Start is called before the first frame update
    void Awake()
    {
        ItemAssetsInstance = this;
    }

    public Transform itemWorldPrefab;

    public Sprite[] sprites;


    [Button(ButtonSizes.Medium)]
    private void SetLenght() {
        liseurExel ExcelList;
        MesFonctions.FindDataExelForObject(out ExcelList);
        int globalLenght = ExcelList.MesListe.LesItems.Length;
        sprites = new Sprite[spritesArrayLenght];
    }
}
