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
        particles = this.gameObject.GetComponent<ParticleSystem>();
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
                particles.Stop();
                break;
            case WeatherType.RAIN:
                particles.Play();
                break;
            default:
                particles.Stop();
                break;
        }
    }
}
