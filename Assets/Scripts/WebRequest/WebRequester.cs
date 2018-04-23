using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequester : IWebRequester
{
	private MonoBehaviour _mono;

    public WebRequester()
    {
		_mono = new MonoBehaviour();
    }

    public UnityWebRequest Get(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        _mono.StartCoroutine(WaitForRequest(request));
        return request;
    }

    public UnityWebRequest Post(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> arg in post)
        {
            form.AddField(arg.Key, arg.Value);
        }
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        _mono.StartCoroutine(WaitForRequest(request));
        return request;
    }

    public IEnumerator WaitForRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
    }
}
