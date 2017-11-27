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
	private int nextUpdate=1;

	public RestConsumer restConsumer;

	// Use this for initialization
	void Start () {
		restConsumer = GameObject.Find("RestConsumer").GetComponent<RestConsumer>();
		this.accX = this.accY = this.accZ = 0;
		th = 0.01f;
	}
		

	// Update is called once per frame
	void Update(){

		updateAcc ();

		if (Time.time >= nextUpdate) {
			Debug.Log (Time.time + ">=" + nextUpdate);

			nextUpdate = Mathf.FloorToInt (Time.time) + 1;

			setMap (accX, accY, accZ);
		}
	}

	private void updateAcc(){
		if (this.enabled) {

			this.accX = Input.acceleration.x;
			this.accY = Input.acceleration.y;
			this.accZ = Input.acceleration.z;


			this.accX = 0.5f;
			if (Mathf.Abs (this.accX) < th) {
				this.accX = 0;
			}

			if (Mathf.Abs (this.accZ) < th) {
				this.accZ = 0;
			}

			if (Mathf.Abs (this.accY) < th) {
				this.accZ = 0;
			}
		}
	}


	private void setMap(float accX, float accY, float accZ) {
		if (this.enabled) {
			Debug.Log (accX);
			StartCoroutine (restConsumer.setMapSettings (accX, accZ));
			if (accY > 0) {
				StartCoroutine (restConsumer.decrementZoom ());
			} else if (accY < 0) {
				StartCoroutine (restConsumer.incrementZoom ());
			}
			this.accX = this.accY = this.accZ = 0;
		}
	}

	private void onRestError(Exception e)
	{
		Debug.LogError("Error during server request: " + e.Message); 
	}
}
