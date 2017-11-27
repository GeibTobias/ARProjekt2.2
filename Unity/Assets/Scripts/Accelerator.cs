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
		th = 0.01f;
	}


	void Update () 
	{
		if (this.enabled) {
			Debug.Log (Input.acceleration.x);
			accX = Input.acceleration.x;
			accY = Input.acceleration.y;
			accZ = Input.acceleration.z;

			accX = 0.5f;
			Debug.Log (accX);

			if (accX < th) {
				accX = 0;
			}

			if (accY < th) {
				accY = 0;
			}

			if (accZ > th) {
				StartCoroutine (restConsumer.decrementZoom ());
			} else if (accZ < -th) {
				StartCoroutine (restConsumer.incrementZoom ());
			}
			
			try {
				StartCoroutine (restConsumer.setMapSettings (accX, accY)); 
			} catch (Exception e) {
				onRestError (e); 
			}
		}
	}

	private void onRestError(Exception e)
	{
		Debug.LogError("Error during server request: " + e.Message); 
	}
}
