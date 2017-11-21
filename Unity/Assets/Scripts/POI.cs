using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class POI : MonoBehaviour {

	public string poiID;
	public string trackerID;
	public string poiName;
	public Sprite poiImage;	
	public GameObject content;
	private PoiScrollList scrollList;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			scrollList = content.GetComponent<PoiScrollList>();

			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {

				BoxCollider bc = hit.collider as BoxCollider;
				POI poi = gameObject.GetComponent<POI> ();
				BoxCollider poiBC = gameObject.GetComponent<BoxCollider> ();
				if (bc && poiBC.enabled && int.Parse(poi.trackerID) == int.Parse(trackerID)) {
					
					scrollList.AddItem (new Item(poiID, poiName, poiImage));
				}
			}
		}
	}
}
