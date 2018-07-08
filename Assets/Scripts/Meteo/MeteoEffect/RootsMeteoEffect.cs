using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RootsMeteoEffect : MonoBehaviour, IMeteoEffect
{
    [Inject]
    private IMeteoManager _meteoManager;

    public GameObject Thorn;

    private Material baseMaterial;
    private Renderer renderer;

    void Start()
    {
        renderer = this.Thorn.GetComponent<Renderer>();
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
        Thorn.SetActive(true);
        Material[] materialArray = renderer.materials;
        if (status.getTemperature() < 10.0)
        {
            Thorn.SetActive(false);
        }
        else if (status.getTemperature() > 25.5)
        {
            Thorn.SetActive(false);
        }
        else
        {
            materialArray[0] = baseMaterial;
        }
        switch (status.getWeatherType())
        {
            case WeatherType.SNOW:
                Thorn.SetActive(false);
                break;
            default:
                break;
        }
        renderer.materials = materialArray;
    }
}
