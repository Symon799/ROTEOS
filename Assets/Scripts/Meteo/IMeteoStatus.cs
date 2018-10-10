using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeatherType {
    NO,
    RAIN,
    SNOW
}

public interface IMeteoStatus {
    bool Init(string toParse);

    string getCity();
    double getTemperature();
    WeatherType getWeatherType();

    DateTime getSunset();
    DateTime getSunrise();
	
}
