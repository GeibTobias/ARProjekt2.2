using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class HttpExecutor : MonoBehaviour {


    public void HttpDelete(string url)
    {
        this.SimpleHttp(url, "DELETE"); 
    }


    public void HttpPut(string url)
    {
        this.SimpleHttp(url, "PUT");
    }

    private void SimpleHttp(string url, string httpMethod)
    {
        HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
        req.Method = httpMethod;

        using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
        {
            if (resp.StatusCode != HttpStatusCode.OK)
            {
                Debug.LogError("Error in http " + httpMethod);

                throw new WebException(resp.StatusDescription); 
            }
        }
    }

    public string HttpGet(string url)
    {
        HttpWebRequest req = WebRequest.Create(url)
                             as HttpWebRequest;
        string result = null;
        using (HttpWebResponse resp = req.GetResponse()
                                      as HttpWebResponse)
        {
            if( resp.StatusCode != HttpStatusCode.OK )
            {
                throw new WebException(resp.StatusDescription);
            }

            StreamReader reader =
                new StreamReader(resp.GetResponseStream());
            result = reader.ReadToEnd();
        }

        return result;
    }


    public void HttpGetAsync(string url)
    {
        StartCoroutine(url); 
    }

    private IEnumerable doHttpGetAsync(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            throw new WebException(request.error);
        }

        Debug.Log(request.downloadHandler.text);
    }


    public void HttpPost(string url, PoiWrapper data)
    {
        string jsonData = JsonUtility.ToJson(data);
        Debug.Log("Json data");
        Debug.Log(jsonData);

        UnityWebRequest request = UnityWebRequest.Post(url, jsonData);
        request.SetRequestHeader("Content-Type", "application/json");
        UnityWebRequestAsyncOperation op = request.SendWebRequest();
        
        op.completed += Op_completed;

        if( request.responseCode != 0 && request.responseCode != (long)System.Net.HttpStatusCode.OK)
        {
            throw new WebException(request.error);
        }
    }

    private void Op_completed(AsyncOperation obj)
    {
        Debug.Log(obj);
    }
}
