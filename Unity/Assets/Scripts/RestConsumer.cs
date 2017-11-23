using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;

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

        this.StartCoroutine(MapSettingUpdater());

    }

    public IEnumerator MapSettingUpdater()
    {
        MapSettings lastUpdate = new MapSettings();
        string[] lastPOI = { };
        while(true)
        {
            yield return new WaitForSeconds(this.refreshInterval);
            MapSettings ms = getMapSetting(); 
            if( ms != null && !lastUpdate.Equals(ms) )
            {
                // trigger event
                if (mapUpdate != null)
                {
                    mapUpdate(ms);
                }
                lastUpdate = ms; 
            }

            string[] pois = this.getPOIList(); 
            if( !pois.SequenceEqual(lastPOI) )
            {
                if (routeUpdate != null)
                {
                    routeUpdate(pois);
                }
                lastPOI = pois; 
            }
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

    public void addPOIList(string[] list)
    {
        if (list == null)
            throw new Exception("Argument null"); 

        string url = string.Format("http://{0}:{1}/poimanager/add/list", this.server, this.port);

        PoiWrapper pw = new PoiWrapper();
        pw.pois = list; 

        httpExec.HttpPost(url, pw);
    }

    public void removePOI(string poi_id)
    {
        if (poi_id == null)
            throw new Exception("Arguemtn is null");

        Debug.Log("Remove POI from server list: " + poi_id);
        string url = string.Format("http://{0}:{1}/poimanager/remove/{2}", this.server, this.port, poi_id);

        httpExec.HttpDelete(url);
    }


    public void removeAllPOI()
    {
        Debug.Log("Remove all POIs");
        string url = string.Format("http://{0}:{1}/poimanager/removeall", this.server, this.port);

        httpExec.HttpDelete(url);
    }

    public string[] getPOIList()
    {
        string url = string.Format("http://{0}:{1}/poimanager/unity/completelist", this.server, this.port);

        string result = httpExec.HttpGet(url);
        PoiWrapper wrapperResult = JsonUtility.FromJson<PoiWrapper>(result);

        return wrapperResult.pois; 
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