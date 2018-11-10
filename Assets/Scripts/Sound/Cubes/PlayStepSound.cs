using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayStepSound : MonoBehaviour, IMeteoEffect
{

    [Inject]
    private IMeteoManager _meteoManager;
    private AudioSource _footSoundSource;
    private AudioClip currentStep;
    public AudioClip normalStep;
    public AudioClip snowStep;
    public AudioClip rainStep;
    public AudioClip hotStep;
    public AudioClip coldStep;

    // Use this for initialization
    void Start()
    {
        currentStep = normalStep;
        _footSoundSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        _meteoManager.addMeteoEffect(this);
    }

    void OnDisable()
    {
        _meteoManager.removeMeteoEffect(this);
    }

    public void StepSound()
    {
        try
        {
            //Debug.Log("Playing Cube Sound...");
            _footSoundSource.PlayOneShot(currentStep);
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    public void meteoChange(IMeteoStatus status)
    {
        if (status.getTemperature() < 10.0)
        {
            currentStep = coldStep;
        }
        else if (status.getTemperature() > 25.5)
        {
            currentStep = hotStep;
        }
        if (status.getWeatherType() == WeatherType.NO)
        {
            currentStep = normalStep;
        }
        if (status.getWeatherType() == WeatherType.RAIN)
        {
            currentStep = rainStep;
        }
        if (status.getWeatherType() == WeatherType.SNOW)
        {
            currentStep = snowStep;
        }
    }
}
