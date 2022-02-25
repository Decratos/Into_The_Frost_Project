using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torchlight : MonoBehaviour
{
    private Light torchLight;
    private GameObject mainCamera;
    private Vector3 vectOffset;
    [SerializeField] private float speed = 3.0f;
    bool isOn = true;

    private void Awake()
    {
        torchLight = GameObject.Find("Torchlight").GetComponent<Light>();
        mainCamera = Camera.main.gameObject;
        vectOffset = transform.position - mainCamera.transform.position;
        LightUnlightTorch(false);
    }
    public void LightUnlightTorch()
    {
        isOn = !isOn;
        torchLight.gameObject.SetActive(isOn);
    }
    public void LightUnlightTorch(bool isOn)
    {
        torchLight.gameObject.SetActive(isOn);
    }

    private void Update()
    {
        torchLight.transform.position = mainCamera.transform.position + vectOffset;
        torchLight.transform.rotation = Quaternion.Slerp(torchLight.transform.rotation, mainCamera.transform.rotation, speed * Time.deltaTime); ;
    }
}
