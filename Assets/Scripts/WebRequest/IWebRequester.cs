using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IWebRequester {

	UnityWebRequest Get(string url, Dictionary<string, string> parameters);
	UnityWebRequest Delete(string url, long id);
	
	UnityWebRequest Post(string url, Dictionary<string,string> post);
	IEnumerator PostCompleteConnection(string url, string json);
	IEnumerator PostComplete(string url, string json);

	IEnumerator WaitForRequest(UnityWebRequest www);
}
