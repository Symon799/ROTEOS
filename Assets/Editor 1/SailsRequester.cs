using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Zenject;
using CI.HttpClient;

public class SailsRequester : MonoBehaviour
{


	[Inject]
    private IWebRequester _webRequester;
	
    [System.Serializable]
    public class Body
    {
        public string userid;
        public string leveljson;
        public string createdAt;
        public string updatedAt;
        
    }


    public void postJson(string jsonLevel) {
         Debug.Log("Welcome to postJson");
         Debug.Log(jsonLevel);
        string sailsUrl = "https://immense-lake-57494.herokuapp.com/editors";

        Body body = new Body();
        body.userid = "20";
        body.leveljson = jsonLevel;
        body.createdAt = "2018-10-05 22:00:00";
        body.updatedAt = "2018-10-05 22:00:00";
        string bodyJson = JsonUtility.ToJson(body);

         Dictionary<string, string> parameters = new Dictionary<string, string>();
         parameters.Add("userid", "20");
         parameters.Add("leveljson", jsonLevel);
         parameters.Add("createdAt", "2018-10-05 22:00:00");
         parameters.Add("updatedAt", "2018-10-05 22:00:00");


        //StartCoroutine(_webRequester.PostComplete(sailsUrl, parameters));
        StartCoroutine(_webRequester.PostComplete2(sailsUrl, bodyJson));
        
       /*  UnityWebRequest resultRequest = _webRequester.Post(sailsUrl, parameters);
        yield return new WaitUntil(() => resultRequest.isDone);
        Debug.Log("Error : " + resultRequest.error);

        Debug.Log("Text : " + resultRequest.downloadHandler.text);*/

          Debug.Log("Bye Bye from postJson");
    }
}
