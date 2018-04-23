using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class MeteoRequester : MonoBehaviour
{
    [Inject]
    private IMeteoStatus _meteoStatus;
    [Inject]
    private IWebRequester _webRequester;

    public string apiKey;
    void Start()
    {
        getMeteo();
    }

    private string getMeteo() {
        string result = null;
        UnityWebRequest resultRequest = _webRequester.Get("ddd");
        Debug.Log(resultRequest.ToString());


        return result;
    }

}
