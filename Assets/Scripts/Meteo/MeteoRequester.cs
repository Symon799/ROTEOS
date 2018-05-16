using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Zenject;
using CI.HttpClient;

public class MeteoRequester : MonoBehaviour
{
    
    [Inject]
    private IWebRequester _webRequester;
    [Inject]
    private IMeteoStatus _meteoStatus;
    [Inject]
    private ILocationFinder _locationFinder;
    [Inject]
    private WeatherType weatherType;

    public string apiKey;
    public int locationMaxTime = 20;
    public Text text;
    public GameObject meteoVisual;

    void Start()
    {
        StartCoroutine(getMeteo());
    }

    private IEnumerator getMeteo() {
        Dictionary<string, string> parameters = new Dictionary<string, string>();

        StartCoroutine(_locationFinder.Refresh(locationMaxTime));
        yield return new WaitUntil(() => _locationFinder.getLastStatus() != LocationFinderStatus.ONGOING);

        if (_locationFinder.getLastStatus() == LocationFinderStatus.ERROR && _locationFinder.hasInitialized()) {
            Debug.Log("Failed to Initialize Meteo");
            yield break;
        }
        
        Debug.Log(_locationFinder.getLastStatus() + " " + _locationFinder.getLastCoordinates().latitude + " " + _locationFinder.getLastCoordinates().longitude);

        parameters.Add("lat", _locationFinder.getLastCoordinates().latitude.ToString());
        parameters.Add("lon", _locationFinder.getLastCoordinates().longitude.ToString());
        parameters.Add("APPID", apiKey);
        parameters.Add("mode", "xml");

        UnityWebRequest resultRequest = _webRequester.Get("http://api.openweathermap.org/data/2.5/weather", parameters);

        yield return new WaitUntil(() => resultRequest.isDone);

        _meteoStatus.Init(resultRequest.downloadHandler.text);
        
        Debug.Log(resultRequest.downloadHandler.text);
        //text.text = _meteoStatus.ToString();
        
        string meteoString = _meteoStatus.ToString();
        if (_meteoStatus.getWeatherType() == WeatherType.NO)
        {
            meteoVisual.transform.GetChild(0).gameObject.SetActive(true);
            meteoVisual.transform.GetChild(1).gameObject.SetActive(false);
            meteoVisual.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (_meteoStatus.getWeatherType() == WeatherType.RAIN)
        {
            meteoVisual.transform.GetChild(0).gameObject.SetActive(false);
            meteoVisual.transform.GetChild(1).gameObject.SetActive(true);
            meteoVisual.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (_meteoStatus.getWeatherType() == WeatherType.SNOW)
        {
            meteoVisual.transform.GetChild(0).gameObject.SetActive(false);
            meteoVisual.transform.GetChild(1).gameObject.SetActive(false);
            meteoVisual.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

}
