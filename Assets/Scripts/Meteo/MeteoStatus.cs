using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public class MeteoStatus : IMeteoStatus
{
    private int _temperature;
    private WeatherType _weatherType;
    public int getTemperature()
    {
        return _temperature;
    }

    public WeatherType GetWeatherType()
    {
        return _weatherType;
    }

    public bool Init(string toParse)
    {
        try
        {
            XDocument weather = XDocument.Parse(toParse);
            IEnumerable<XElement> current = weather.Elements();
            
            return true;
        }
        catch
        {
            return false;
        }
    }
}
