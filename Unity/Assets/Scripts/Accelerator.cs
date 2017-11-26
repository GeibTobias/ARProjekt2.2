using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Accelerator : MonoBehaviour {


	public float accX;
	public float accY;
	public float accZ;

	private float th;

	public RestConsumer restConsumer;

	// Use this for initialization
	void Start () {
		restConsumer = GameObject.Find("RestConsumer").GetComponent<RestConsumer>();
		th = 0.5f;
	}


	void Update () 
	{
		
		accX = Input.acceleration.x ;
		accY = Input.acceleration.y ;
		accZ = Input.acceleration.z ;

		if (accX < th) {
			accX = 0;
		}

		if (accY < th) {
			accY = 0;
		}

		if (accZ > 0) {
			restConsumer.deincrementZoom ();
		} else {
			restConsumer.incrementZoom ();
		}
			
		try
		{
			StartCoroutine(restConsumer.setMapSettings(accX, accY)); 
		} catch(Exception e)
		{
			onRestError(e); 
		}
	}

	private void onRestError(Exception e)
	{
		Debug.LogError("Error during server request: " + e.Message); 
	}
}
