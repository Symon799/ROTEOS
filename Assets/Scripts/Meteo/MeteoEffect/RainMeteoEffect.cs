using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RainMeteoEffect : MonoBehaviour, IMeteoEffect
{
    [Inject]
    private IMeteoManager _meteoManager;

    private ParticleSystem particles;

    void Start()
    {
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
    {   CloudDisappear();
        if (status.getTemperature() < 10.0)
        {
        }
        else if (status.getTemperature() > 25.5)
        {
        }
        else
        {
        }
        switch (status.getWeatherType())
        {
            case WeatherType.SNOW:
                CloudDisappear();
                break;
            case WeatherType.RAIN:
                CloudAppear();
                break;
            default:
                CloudDisappear();
                break;
        }
    }

    private void CloudDisappear()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void CloudAppear()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    
}
