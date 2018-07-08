using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TreeMeteoEffect : MonoBehaviour, IMeteoEffect
{
    [Inject]
    private IMeteoManager _meteoManager;
    public Material ColdMaterial;
    public Material SnowMaterial;

    public GameObject Foliage;

    private Material baseMaterial;
    private Renderer renderer;

    void Start()
    {
        renderer = this.Foliage.GetComponent<Renderer>();
        baseMaterial = renderer.materials[0];
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
        Foliage.SetActive(true);
        Material[] materialArray = renderer.materials;

        if (status.getTemperature() < 10.0)
        {

            materialArray[0] = ColdMaterial;
        }
        else if (status.getTemperature() > 25.5)
        {
            Foliage.SetActive(false);
        }
        else
        {
            materialArray[0] = baseMaterial;
        }
        switch (status.getWeatherType())
        {
            case WeatherType.SNOW:
                materialArray[0] = SnowMaterial;
                break;
            default:
                break;
        }
        renderer.materials = materialArray;
    }
}
