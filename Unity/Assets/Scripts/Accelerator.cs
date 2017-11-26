using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Accelerator : MonoBehaviour {


	public float accX;
	public float accY;
	public float accZ;
	public RestConsumer restConsumer;

	// Use this for initialization
	void Start () {
		restConsumer = GameObject.Find("RestConsumer").GetComponent<RestConsumer>();
	}


	void Update () 
	{
		
		accX = Input.acceleration.x ;
		accY = Input.acceleration.y ;
		accZ = Input.acceleration.z ;

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
