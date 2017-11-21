using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using UnityEngine;

public class RestConsumer : MonoBehaviour {

    public string server = "localhost";
    public string port = "8080";

    private HttpExecutor httpExec; 

    private long coroutineWait = 1;

    # region DELEGATES
    public delegate void onMapSettingUpdate(MapSettings settings);
    public delegate void onRouteUpdate(string[] route);
    # endregion

    #region EVENTS
    public event onMapSettingUpdate mapUpdate;
    public event onRouteUpdate routeUpdate; 
    # endregion


    // Use this for initialization
    void Start () {

        httpExec = new HttpExecutor();

        //Debug.Log("RestConsumer");
        //getPOIList();

        //Debug.Log("Get map settings");
        //getMapSetting();

        PoiWrapper t = new PoiWrapper();
        t.pois = new string[] { "test", "test1" }; 
        httpExec.HttpPost(string.Format("http://{0}:{1}/poimanager/add/list", this.server, this.port), t );

        //mapUpdate += new onMapSettingUpdate(this.testMapEvent);
        //routeUpdate += new onRouteUpdate(this.testRouteEvent);

        this.StartCoroutine(MapSettingUpdater());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void testMapEvent(MapSettings settings)
    {
        Debug.Log("EVENT: " + settings.zoom);
    }

    private void testRouteEvent(string[] route)
    {
        Debug.Log("ROUTE: " + route); 
    }


    public IEnumerator MapSettingUpdater()
    {
        MapSettings lastUpdate = new MapSettings();
        string[] lastPOI = { };
        while(true)
        {
            yield return new WaitForSeconds(this.coroutineWait);
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
        //Debug.Log("Get map settings");
        string url = string.Format("http://{0}:{1}/mapsync/mapget", this.server, this.port);

        string result = httpExec.HttpGet(url);
        //Debug.Log("map: " + result); 

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

    public string[] getPOIList()
    {
        //Debug.Log("Get POI list");
        string url = string.Format("http://{0}:{1}/poimanager/unity/completelist", this.server, this.port);

        string result = httpExec.HttpGet(url);

        PoiWrapper wrapperResult = JsonUtility.FromJson<PoiWrapper>(result);
        //Debug.Log(wrapperResult);

        return wrapperResult.pois; 
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