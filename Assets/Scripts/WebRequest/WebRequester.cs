using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequester : IWebRequester
{

    public UnityWebRequest Get(string url, Dictionary<string, string> parameters)
    {
        string finalUrl = url;
        if (parameters != null)
        {
            int count = 0;
            finalUrl += '?';
            foreach (KeyValuePair<string, string> arg in parameters)
            {
                count++;
                finalUrl += arg.Key + '=' + arg.Value;
                if (count != parameters.Count) {
                    finalUrl += '&';
                }
            }
            
        }
        Debug.Log("FINAL URL : " + finalUrl);
        UnityWebRequest request = UnityWebRequest.Get(finalUrl);
        request.SendWebRequest();
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
        request.SendWebRequest();
        return request;
    }

    public IEnumerator WaitForRequest(UnityWebRequest request)
    {
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Show results as text
            Debug.Log(request.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = request.downloadHandler.data;
        }
    }
}
