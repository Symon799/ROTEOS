using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequester : IWebRequester
{
    [System.Serializable]
    public class Result
    {
        public string token;
        public bool status;
        public string error;
        public string id;
    }

    public class ConnectionRequest
    {
        public string token;
        public bool status;
        public string error;

    }

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
                if (count != parameters.Count)
                {
                    finalUrl += '&';
                }
            }

        }
        //Debug.Log("FINAL URL : " + finalUrl);
        UnityWebRequest request = UnityWebRequest.Get(finalUrl);
        request.SendWebRequest();
        return request;
    }

    public UnityWebRequest Delete(string url, long id)
    {
        string finalUrl = url + "/" + id.ToString();
        
        Debug.Log("FINAL URL : " + finalUrl);
        UnityWebRequest request = UnityWebRequest.Delete(finalUrl);
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

    public IEnumerator PostCompleteConnection(string url, string json)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();

        if (request.responseCode == 200)
        {
            ConnectionRequest example = JsonUtility.FromJson<ConnectionRequest>(request.downloadHandler.text);
            Result resultObj = JsonUtility.FromJson<Result>(request.downloadHandler.text);
            AccountManager.token = resultObj.token;
            AccountManager.idCurrentUser = Convert.ToInt64(resultObj.id);
        }
        Debug.Log(request.responseCode + " DownloadHandler Text : " + request.downloadHandler.text);
    }

    public IEnumerator PostComplete(string url, string json)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();
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
