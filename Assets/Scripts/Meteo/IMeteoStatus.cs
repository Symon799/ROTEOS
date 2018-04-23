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
    int getTemperature();
    WeatherType GetWeatherType();
	
}
