using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Accelerator : MonoBehaviour {

	public Button myButton;

	private bool runScript;

	public float accX;
	public float accY;
	public float accZ;

	public int mapConstant;

	public float timeStep;

	private float th;
	private float nextUpdate=0.5f;

	public RestConsumer restConsumer;

	// Use this for initialization
	void Start () {
		restConsumer = GameObject.Find("RestConsumer").GetComponent<RestConsumer>();
		this.accX = this.accY = this.accZ = 0;
		th = 0.25f;
		myButton.onClick.AddListener (ActivateNav);
		runScript = false;
		Screen.orientation = ScreenOrientation.LandscapeRight; 
		mapConstant = 100;
		timeStep = 0.2f;
	}
		

	// Update is called once per frame
	void Update(){
		
		updateAcc ();

		if (Time.time >= nextUpdate) {
            //			Debug.Log (Time.time + ">=" + nextUpdate);
			nextUpdate = Time.time + timeStep;

			setMap (accX, accY, accZ);
		}
	}

	private void updateAcc(){
		if (runScript) {

			Vector3 acc = Input.acceleration;
			acc = Quaternion.Euler (90, 0, 0) * acc;

			this.accX = acc.x;
			this.accY = acc.y;
			this.accZ = acc.z;

//			this.accX = 0.5f;
			if (Mathf.Abs (this.accX) < th) {
				this.accX = 0;
			}

			if (Mathf.Abs (this.accY) < th) {
				this.accY = 0;
			}

			if (Mathf.Abs (this.accZ) < th) {
				this.accZ = 0;
			}

		} 
	}

	void ActivateNav (){
		runScript = !runScript;
	}

	private void setMap(float accX, float accY, float accZ) {
		if (runScript && (accX != 0 || accZ != 0)) {
			//Debug.Log (accX);
			float deltaX = accX * mapConstant ;
			float deltaZ = -accZ * mapConstant ;
			StartCoroutine (restConsumer.setMapSettings (deltaX, deltaZ));


			this.accX = this.accY = this.accZ = 0;
		}
	}

	private void onRestError(Exception e)
	{
		Debug.LogError("Error during server request: " + e.Message); 
	}
}
