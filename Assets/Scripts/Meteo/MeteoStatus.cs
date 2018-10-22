using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;

public class MeteoStatus : IMeteoStatus
{
    private string _city;
    private double _temperature;
    private WeatherType _weatherType;

    private DateTime _sunrise;
    private DateTime _sunset;

    public MeteoStatus() {
        Debug.Log("Meteo Status...");
        _city = null;
        _temperature = 20;
        _weatherType = WeatherType.NO;
        DateTime tmp = DateTime.Now;
        _sunset = new DateTime(tmp.Year, tmp.Month, tmp.Day, 22, 0, 0);
        _sunrise = new DateTime(tmp.Year, tmp.Month, tmp.Day, 6, 0, 0);
    }

    public string getCity()
    {
        return _city;
    }
    public double getTemperature()
    {
        return _temperature;
    }
    public WeatherType getWeatherType()
    {
        return _weatherType;
    }

    public DateTime getSunset()
    {
        return _sunset;
    }

    public DateTime getSunrise()
    {
        return _sunrise;
    }

    public bool Init(string toParse)
    {
        try
        {
            XDocument weather = XDocument.Parse(toParse);
            setCity(weather);
            setTemperature(weather);
            setWeatherType(weather);
            return true;
        }
        catch   
        {
            return false;
        }
    }

    private void setCity(XDocument weather) {
         _city = weather.Element("current").Element("city").Attribute("name").Value;
    }

    private void setTemperature(XDocument weather) {
         _temperature = Convert.ToDouble(weather.Element("current").Element("temperature").Attribute("value").Value) - 273.15f;
    }

    private void setWeatherType(XDocument weather) {
        switch (weather.Element("current").Element("precipitation").Attribute("mode").Value) {
            case "rain":
                _weatherType = WeatherType.RAIN;
                break;
            case "snow":
                _weatherType = WeatherType.SNOW;
                break;
            default:
                _weatherType = WeatherType.NO;
                break;
        }
    }

    public override string ToString() {
        string res = "Location : " + _city + 
            "\nTemperature : " + _temperature.ToString("0.00") + "°C" +
            "\nWeather Type : " + _weatherType;
        return res;
    }

    
}
