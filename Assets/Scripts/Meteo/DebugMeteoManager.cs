using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DebugMeteoManager : MonoBehaviour {

	[Inject]
	private IMeteoManager manager;
	public string city;
    private double temperature = 15;
    private WeatherType weatherType = WeatherType.NO;

	// Use this for initialization
	public void applyRain()
	{
		weatherType = WeatherType.RAIN;
		DebugMeteoStatus status = new DebugMeteoStatus();
		status._city = city;
		status._temperature = temperature;
		status._weatherType = weatherType;
		manager.applyMeteo(status);
	}

	public void applySnow()
	{
		weatherType = WeatherType.SNOW;
		DebugMeteoStatus status = new DebugMeteoStatus();
		status._city = city;
		status._temperature = temperature;
		status._weatherType = weatherType;
		manager.applyMeteo(status);
	}

	public void applyNo()
	{
		weatherType = WeatherType.NO;
		DebugMeteoStatus status = new DebugMeteoStatus();
		status._city = city;
		status._temperature = temperature;
		status._weatherType = weatherType;
		manager.applyMeteo(status);
	}

	public void applyCold()
	{
		temperature = 0;
		DebugMeteoStatus status = new DebugMeteoStatus();
		status._city = city;
		status._temperature = temperature;
		status._weatherType = weatherType;
		manager.applyMeteo(status);
	}

	public void applyTemperate()
	{
		temperature = 15;
		DebugMeteoStatus status = new DebugMeteoStatus();
		status._city = city;
		status._temperature = temperature;
		status._weatherType = weatherType;
		manager.applyMeteo(status);
	}

	public void applyHot()
	{
		temperature = 50;
		DebugMeteoStatus status = new DebugMeteoStatus();
		status._city = city;
		status._temperature = temperature;
		status._weatherType = weatherType;
		manager.applyMeteo(status);
	}
}
