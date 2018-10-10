using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IWebRequester {

	UnityWebRequest Get(string url, Dictionary<string, string> parameters);
	
	UnityWebRequest Post(string url, Dictionary<string,string> post);
	IEnumerator PostComplete2(string url, string json);
	IEnumerator PostComplete(string url, Dictionary<string, string> post);

	IEnumerator WaitForRequest(UnityWebRequest www);
}
