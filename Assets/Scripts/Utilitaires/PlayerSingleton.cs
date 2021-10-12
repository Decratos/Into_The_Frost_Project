using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    public static PlayerSingleton playerInstance;
    // Start is called before the first frame update
    void Awake()
    {
        playerInstance = this;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
