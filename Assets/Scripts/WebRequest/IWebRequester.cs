using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public interface IWebRequester {

	UnityWebRequest Get(string url, Dictionary<string, string> parameters);
	UnityWebRequest Delete(string url, long id);
	IEnumerator PostCompleteConnection(string url, string json);
	IEnumerator PostComplete(string url, string json);
}
