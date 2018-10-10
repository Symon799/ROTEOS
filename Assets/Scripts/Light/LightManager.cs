using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LightManager : MonoBehaviour {

	[Inject]
	private IMeteoStatus _meteoStatus;
	private GameObject _astral;
	private GameObject _sun;
	private GameObject _moon;
	private GameObject _clouds;

	// Test values

	public DateTime currentTime;

	void Start()
	{
		_astral = transform.GetChild(0).gameObject;
		_sun = _astral.transform.GetChild(0).gameObject;
		_moon = _astral.transform.GetChild(1).gameObject;
		_clouds = transform.GetChild(1).gameObject;
		currentTime = DateTime.Now;
	}

	public void setPlanetary()
	{
		_astral.transform.rotation = Quaternion.Euler(getAngle(), 0, 0);
		Debug.Log(getAngle());
		if (currentTime >= _meteoStatus.getSunrise() && currentTime <= _meteoStatus.getSunset())
			_moon.SetActive(false);
		else
			_sun.SetActive(false);
	}

	private float getAngle()
	{
		double sunriseToSunset = (_meteoStatus.getSunset() - _meteoStatus.getSunrise()).TotalSeconds;
		double sunsetToSunrise = (24 * 3600) - sunriseToSunset;
		if (currentTime < _meteoStatus.getSunrise())
		{
			TimeSpan tmp = _meteoStatus.getSunrise() - currentTime;
			return (float)(180 * ((sunsetToSunrise - tmp.TotalSeconds)/sunsetToSunrise) + 180);
		}
		else if (currentTime >= _meteoStatus.getSunrise() && currentTime <= _meteoStatus.getSunset())
		{
			TimeSpan tmp = _meteoStatus.getSunset() - currentTime;
			return (float)(180 * ((sunriseToSunset - tmp.TotalSeconds)/sunriseToSunset));
		}
		else
		{
			TimeSpan tmp = currentTime - _meteoStatus.getSunset();
			return (float)(180 * ((tmp.TotalSeconds)/sunsetToSunrise) + 180);
		}
	}

	public void SetDistance(float distance)
	{
		float final = -(distance + 20);
		_sun.transform.GetChild(0).position = new Vector3(0, 0, final);
		_moon.transform.GetChild(0).position = new Vector3(0, 0, -final);
		_clouds.transform.position = new Vector3(0, -final, 0);
	}
}
