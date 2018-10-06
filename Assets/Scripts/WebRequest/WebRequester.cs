using System.Collections;
using System.Collections.Generic;
using System.Text;
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
        Debug.Log("UnityWebRequest.Post");
        Debug.Log(url);
        Debug.Log(post);
        
        
        WWWForm form = new WWWForm();
        foreach (KeyValuePair<string, string> arg in post)
        {
            form.AddField(arg.Key, arg.Value);
        }
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.SendWebRequest();
        return request;
    }

    public IEnumerator PostComplete2(string url, string json) {
         var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
 
        yield return request.Send();
 
        Debug.Log("Status Code: " + request.responseCode);
    }

    public IEnumerator PostComplete(string url, Dictionary<string, string> post)
    {
        WWWForm form = new WWWForm();
        /*foreach (KeyValuePair<string, string> arg in post)
        {
            Debug.Log(arg.Key + " " + arg.Value);
            form.AddField(arg.Key, arg.Value);
        }*/
        //Debug.Log("Body : " + form);
        form.AddField("userid", "20");
        form.AddField("leveljson", "{}");
        form.AddField("createdAt", "2018-10-05 22:00:00");
        form.AddField("updatedAt", "2018-10-05 22:00:00");

/* 
        www = UnityWebRequest.Put(url, bodyJSON);
        www.method = "POST";
*/
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        
        www.SetRequestHeader ("Accept", "text/json");        
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("downloadHandler Text : " + www.downloadHandler.text);
            Debug.Log("responseCode : " + www.responseCode);
            Debug.Log("isDone : " + www.isDone);
            Debug.Log("method : " + www.method);   
        }
        
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
