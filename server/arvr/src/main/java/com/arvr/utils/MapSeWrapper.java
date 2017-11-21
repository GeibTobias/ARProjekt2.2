package com.arvr.utils;

import com.arvr.map.Coordinate;

public class MapSeWrapper {

	public double lat; 
	public double lng; 
	public int zoom; 
	
	public MapSeWrapper() {}
	
	public MapSeWrapper(Coordinate coord, int zoom) {
		
		this.lat = coord.lat.doubleValue(); 
		this.lng = coord.lng.doubleValue(); 
		this.zoom = zoom; 
	}
}
