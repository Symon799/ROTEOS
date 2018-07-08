using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DirtMeteoEffect : MonoBehaviour, IMeteoEffect
{
    [Inject]
    private IMeteoManager _meteoManager;
    public Material ColdMaterial;
    public Material HotMaterial;
    public Material SnowMaterial;

    private Material baseMaterial;
    private Renderer renderer;

    void Start()
    {
        renderer = this.GetComponent<Renderer>();
        baseMaterial = renderer.materials[1];
    }

    void OnEnable()
    {
        _meteoManager.addMeteoEffect(this);
    }

    void OnDisable()
    {
        _meteoManager.removeMeteoEffect(this);
    }

    public void meteoChange(IMeteoStatus status)
    {
        Material[] materialArray = renderer.materials;

        if (status.getTemperature() < 10.0)
        {
            materialArray[1] = ColdMaterial;
        }
        else if (status.getTemperature() > 25.5)
        {
            materialArray[1] = HotMaterial;
        }
        else
        {
            materialArray[1] = baseMaterial;
        }
        switch (status.getWeatherType())
        {
            case WeatherType.SNOW:
                materialArray[1] = SnowMaterial;
                break;
            default:
                break;
        }
        renderer.materials = materialArray;
    }
}
