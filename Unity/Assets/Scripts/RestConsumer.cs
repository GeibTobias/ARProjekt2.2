using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class RestConsumer : MonoBehaviour {

    public string server;
    public string port;

    // interval that defines the time between two updates from the server
    // interval is in seconds
    public float refreshInterval = 1;

    private HttpExecutor httpExec = new HttpExecutor(); 


    # region DELEGATES
    public delegate void onMapSettingUpdate(MapSettings settings);
    public delegate void onRouteUpdate(string[] route);
    # endregion

    #region EVENTS
    public event onMapSettingUpdate mapUpdate;
    public event onRouteUpdate routeUpdate; 
    # endregion

    private void Start()
    {
        init();
    }

    private void init()
    {
        //mapUpdate += new onMapSettingUpdate(this.testMapEvent);
        //routeUpdate += new onRouteUpdate(this.testRouteEvent);

        this.StartCoroutine(serverSyncLoop());
    }

    public IEnumerator setMapSettings(double deltaLat, double deltaLng)
    {
        string url = string.Format("http://{0}:{1}/mapsync/mapsetdelta", this.server, this.port);

        // parameter zoom is not used anymore here
        MapSettings ms = new MapSettings(deltaLat, deltaLng, 0);
        string jsonData = JsonUtility.ToJson(ms);

        using (UnityWebRequest request = UnityWebRequest.Post(url, jsonData))
        {
            yield return request.SendWebRequest();

            isWebRequestError(request);
        }
    }

    public IEnumerator serverSyncLoop()
    {
        float interval = this.refreshInterval; 
        while(true)
        {
            yield return new WaitForSeconds(interval);
            try
            {
                StartCoroutine(this.getPOIListAsync());

            } catch(Exception e)
            {
                Debug.LogError("Server connection error: " + e.Message);
            }
        }
    }

    public IEnumerator incrementZoom()
    {
        Debug.Log("Increment zoom value");
        string url = string.Format("http://{0}:{1}/mapsync/zoom/inc", this.server, this.port);

        byte[] data = System.Text.Encoding.UTF8.GetBytes("inc");
        using (UnityWebRequest request = UnityWebRequest.Put(url, data))
        {
            yield return request.SendWebRequest();

            isWebRequestError(request);
        }
    }

    public IEnumerator decrementZoom()
    {
        Debug.Log("Decrement zoom value");
        string url = string.Format("http://{0}:{1}/mapsync/zoom/dec", this.server, this.port);

        byte[] data = System.Text.Encoding.UTF8.GetBytes("dec");
        using (UnityWebRequest request = UnityWebRequest.Put(url, data))
        {
            yield return request.SendWebRequest();

            isWebRequestError(request);
        }
    }

    public MapSettings getMapSetting()
    {
        string url = string.Format("http://{0}:{1}/mapsync/mapget", this.server, this.port);
        string result = httpExec.HttpGet(url);

        MapSettings wrapperResult = JsonUtility.FromJson<MapSettings>(result);
        return wrapperResult;
    }


    public void addPOI(string poi_id)
    {
        if (poi_id == null)
            throw new Exception("Arguemtn is null"); 

        Debug.Log("Add POI to server list: " + poi_id); 
        string url = string.Format("http://{0}:{1}/poimanager/add/{2}", this.server, this.port, poi_id);

        httpExec.HttpPut(url); 
    }

    public IEnumerator addPOIAsync(string poi_id)
    {
        if (poi_id == null)
            throw new Exception("Arguemtn is null");

        Debug.Log("Add POI to server list: " + poi_id);
        string url = string.Format("http://{0}:{1}/poimanager/add/{2}", this.server, this.port, poi_id);

        byte[] data = System.Text.Encoding.UTF8.GetBytes(poi_id);
        using (UnityWebRequest request = UnityWebRequest.Put(url, data))
        {
            yield return request.SendWebRequest();

            isWebRequestError(request);
        }
    }

    public IEnumerator addPOIListAsync(string[] list)
    {
        if (list == null)
            throw new Exception("Argument null");

        string url = string.Format("http://{0}:{1}/poimanager/add/list", this.server, this.port);
        PoiWrapper pw = new PoiWrapper();
        pw.pois = list;
        string jsonData = JsonUtility.ToJson(pw);

        using (UnityWebRequest request = UnityWebRequest.Post(url, jsonData))
        {
            yield return request.SendWebRequest();

            isWebRequestError(request); 
        }
    }

    public void removePOI(string poi_id)
    {
        if (poi_id == null)
            throw new Exception("Arguemtn is null");

        Debug.Log("Remove POI from server list: " + poi_id);
        string url = string.Format("http://{0}:{1}/poimanager/remove/{2}", this.server, this.port, poi_id);

        httpExec.HttpDelete(url);
    }

    public IEnumerator removePOIAsync(string poi_id)
    {
        if (poi_id == null)
            throw new Exception("Arguemtn is null");

        Debug.Log("Remove POI from server list: " + poi_id);
        string url = string.Format("http://{0}:{1}/poimanager/remove/{2}", this.server, this.port, poi_id);

        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            yield return request.SendWebRequest();

            isWebRequestError(request);
        }
    }

    public void removeAllPOI()
    {
        Debug.Log("Remove all POIs");
        string url = string.Format("http://{0}:{1}/poimanager/removeall", this.server, this.port);

        httpExec.HttpDelete(url);
    }

    public IEnumerator removeAllPOIAsync()
    {
        Debug.Log("Remove all POIs");
        string url = string.Format("http://{0}:{1}/poimanager/removeall", this.server, this.port);

        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            yield return request.SendWebRequest();

            isWebRequestError(request);
        }
    }

    public IEnumerator getPOIListAsync()
    {
        string url = string.Format("http://{0}:{1}/poimanager/unity/completelist", this.server, this.port);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            isWebRequestError(request); 

            string result = request.downloadHandler.text;
            PoiWrapper wrapperResult = JsonUtility.FromJson<PoiWrapper>(result);

            if (routeUpdate != null && wrapperResult != null)
            {
                routeUpdate(wrapperResult.pois);
            }
        }
    }

    public string[] getPOIList()
    {
        string url = string.Format("http://{0}:{1}/poimanager/unity/completelist", this.server, this.port);

        string result = httpExec.HttpGet(url);
        PoiWrapper wrapperResult = JsonUtility.FromJson<PoiWrapper>(result);

        return wrapperResult.pois; 
    }

    private void isWebRequestError(UnityWebRequest request)
    {
        if( request.isNetworkError || request.isHttpError )
        {
            Debug.LogError(request.error);
            throw new WebException(request.error); 
        }
    }

    private void testMapEvent(MapSettings settings)
    {
        Debug.Log("EVENT: " + settings.zoom);
    }

    private void testRouteEvent(string[] route)
    {
        Debug.Log("ROUTE: " + route);
    }
}

[Serializable]
public class PoiWrapper
{
    public string[] pois; 
}

[Serializable]
public class MapSettings
{
    public double lat;
    public double lng;
    public int zoom;

    public MapSettings() { }

    public MapSettings(double lat, double lng, int zoom)
    {
        this.lat = lat;
        this.lng = lng;
        this.zoom = zoom; 
    }

    public override bool Equals(object obj)
    {
        if( obj is MapSettings )
        {
            MapSettings tmp = (MapSettings)obj; 
            if( this.lat == tmp.lat &&
                this.lng == tmp.lng && 
                this.zoom == tmp.zoom )
            {
                return true; 
            }
        }

        return false; 
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}