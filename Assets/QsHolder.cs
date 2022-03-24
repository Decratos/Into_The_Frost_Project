using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QsHolder : MonoBehaviour
{
    [SerializeField] private List<Quickslot> quickslotsList;

    public void UseQuickslot(int quickslotNumber)
    {
        quickslotsList[quickslotNumber - 1].UseSlot();
    }
}
