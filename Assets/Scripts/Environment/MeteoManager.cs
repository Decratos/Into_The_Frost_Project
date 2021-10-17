
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

[ExecuteAlways]
public class MeteoManager : MonoBehaviour
{
    public float SpeedDay;
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset myPreset;
    [SerializeField, Range(0,24)] private float TimeOfDay;
    [SerializeField] private VolumeProfile EnvironmentProfile;
    [SerializeField] private GameObject[] WeatherVFXs;
    public GameObject actualWeatherVFXused;
    private Fog _fog;
    private float temperatureWeatherAddition;
    private float temperatureWeatherScaleAddition;
    
    
    public enum weather
    {
        Snow, Rain, Foggy, Clear
    }

    public enum Intensity
    {
        Low, Medium, Big
    }
    

    [SerializeField] private weather _weather;

    [SerializeField] private Intensity _intensity;
    // Start is called before the first frame update

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                }
            }
        }
    }
    

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = myPreset.AmbientColor.Evaluate(timePercent);
        //_fog.color.value = myPreset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.colorTemperature = myPreset.directionalTemperature.Evaluate(timePercent);
            DirectionalLight.transform.localRotation =
                Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    private void UpdateTemperature(float timePercent)
    {
        GetComponent<Temperature>().temperature = myPreset.temperatureByTime.Evaluate(timePercent) + (temperatureWeatherAddition + temperatureWeatherScaleAddition);
    }
    // Update is called once per frame
    void Update()
    {
        if (myPreset == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime*(SpeedDay/100);
            TimeOfDay %= 24;//Clamp between 0-24
            UpdateLighting(TimeOfDay / 24);
            UpdateWeather();
            UpdateTemperature(TimeOfDay / 24);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24);
            UpdateWeather();
            UpdateTemperature(TimeOfDay / 24);
        }
    }

    private void UpdateWeather()
    {
        EnvironmentProfile.TryGet<Fog>(out var fog);
        switch (_weather)
        {
            case weather.Foggy:
                temperatureWeatherAddition = -5;
                fog.enabled.value = true;
                actualWeatherVFXused = WeatherVFXs[2];
                UpdateVFX(2);
                SetWeatherSettings(weather.Foggy, fog);
                break;
            case weather.Clear:
                temperatureWeatherAddition = 0;
                fog.enabled.value = false;
                UpdateVFX(-1);
                RenderSettings.fogDensity = 0;
                break;
            case weather.Rain:
                temperatureWeatherAddition = -10f;
                fog.enabled.value = true;
                actualWeatherVFXused = WeatherVFXs[1];
                UpdateVFX(1);
                SetWeatherSettings(weather.Rain, fog);
                break;
            case weather.Snow:
                temperatureWeatherAddition = -15;
                fog.enabled.value = true;
                actualWeatherVFXused = WeatherVFXs[0];
                UpdateVFX(0);
                SetWeatherSettings(weather.Snow, fog);
                break;
        }
    }

    private void SetWeatherSettings(weather _weather, Fog myFog)
    {
        switch (_intensity)
        {
            case Intensity.Big:
                temperatureWeatherScaleAddition = -8;
                myFog.meanFreePath.value = 2f;
                if(actualWeatherVFXused.GetComponent<VisualEffect>())
                    actualWeatherVFXused.GetComponent<VisualEffect>().SetFloat("Intensity", 5);
                else
                {
                    WeatherVFXs[0].GetComponent<ParticleSystem>().emissionRate = 100;
                }
                break;
            case Intensity.Medium:
                temperatureWeatherScaleAddition = -5;
                myFog.meanFreePath.value = 6f;
                if(actualWeatherVFXused.GetComponent<VisualEffect>())
                    actualWeatherVFXused.GetComponent<VisualEffect>().SetFloat("Intensity", 3);
                else
                {
                    WeatherVFXs[0].GetComponent<ParticleSystem>().emissionRate = 100;
                }
                break;
            case Intensity.Low:
                temperatureWeatherScaleAddition = -3;
                myFog.meanFreePath.value = 10f;
                if(actualWeatherVFXused.GetComponent<VisualEffect>())
                    actualWeatherVFXused.GetComponent<VisualEffect>().SetFloat("Intensity", 1);
                else
                {
                    WeatherVFXs[0].GetComponent<ParticleSystem>().emissionRate = 25;
                }
                break;
        }
    }

    private void UpdateVFX(int id)
    {
        foreach (var vfx in WeatherVFXs)
        {
            vfx.SetActive(false);
        }
        if(id != -1)
            WeatherVFXs[id].SetActive(true);
        else
        {
            actualWeatherVFXused = null;
        }
    }
}
