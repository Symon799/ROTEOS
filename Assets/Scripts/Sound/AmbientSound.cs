using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AmbientSound : MonoBehaviour, IMeteoEffect
{

    [Inject]
    private IMeteoManager _meteoManager;
    public bool hasWater = true;
    public bool hasLava = false;
    public AudioSource waterFlow;
    public AudioSource lavaFlow;
    public AudioSource rainFlow;

	void OnAwake()
	{
		PauseEverything();
	}

    void OnEnable()
    {
        _meteoManager.addMeteoEffect(this);
    }

    void OnDisable()
    {
        _meteoManager.removeMeteoEffect(this);
    }

	void PauseEverything()
	{
		waterFlow.Pause();
		//rainFlow.Pause();
		//lavaFlow.Pause();
	}

    public void meteoChange(IMeteoStatus status)
    {
		PauseEverything();
		if (status.getWeatherType() == WeatherType.NO)
        {
            if (hasWater)
			{
				waterFlow.UnPause();
			}
        }
        if (status.getWeatherType() == WeatherType.RAIN)
        {
            if (hasWater)
			{
				waterFlow.UnPause();
			}
        }
        if (status.getWeatherType() == WeatherType.SNOW)
        {
            if (hasWater)
			{
				waterFlow.Pause();
			}
        }

        if (status.getTemperature() < 10.0)
        {
            if (hasWater)
			{
				waterFlow.UnPause();
			}
        }
        else if (status.getTemperature() > 25.5)
        {
            if (hasWater)
			{
				waterFlow.UnPause();
			}
        }
        
		
    }
}
