using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Zenject;
using CI.HttpClient;

public class SailsRequester : ISailsRequester {


	 [Inject]
    private IWebRequester _webRequester;
	
	public string name = "SailsRequester";

    public IEnumerator postJson(string jsonLevel) {

        string sailsUrl = "https://immense-lake-57494.herokuapp.com/userslevelseditor/";
         Dictionary<string, string> parameters = new Dictionary<string, string>();
         parameters.Add("user_id", "20");
         parameters.Add("level_json", jsonLevel);
         parameters.Add("createdAt", "10-05-2018");
         parameters.Add("updatedAt", "10-05-2018");
         UnityWebRequest resultRequest = _webRequester.Post(sailsUrl, parameters);
        yield return new WaitUntil(() => resultRequest.isDone);
         Debug.Log(resultRequest.downloadHandler.text);
    }
}
