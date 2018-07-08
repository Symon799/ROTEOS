using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DebugMeteoManager : MonoBehaviour {

	[Inject]
	private IMeteoManager manager;
	public string city;
    public double temperature;
    public WeatherType weatherType;

	// Use this for initialization
	public void applyChanges()
	{
		DebugMeteoStatus status = new DebugMeteoStatus();
		status._city = city;
		status._temperature = temperature;
		status._weatherType = weatherType;
		manager.applyMeteo(status);
	}
}
