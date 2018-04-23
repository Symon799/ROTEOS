using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IWebRequester {

	UnityWebRequest Get(string url);
	
	UnityWebRequest Post(string url, Dictionary<string,string> post);

	IEnumerator WaitForRequest(UnityWebRequest www);
}
