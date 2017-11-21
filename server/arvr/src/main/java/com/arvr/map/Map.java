package com.arvr.map;

import java.math.BigDecimal;

public class Map {

	private static Map instance = new Map(); 
	
	private static Coordinate mapFocus; 
	private static int mapZoom; 
	
	protected Map() {
		// first init mapFocus to Berlin
		mapFocus = new Coordinate(new BigDecimal(13.413215), new BigDecimal(52.521918)); 
		mapZoom = 4; 
	}
	
	public static Map getInstance() {	
		return instance; 
	}
	
	public static void setFocus(BigDecimal lattitude, BigDecimal longtitude) throws Exception {
		
		if( lattitude == null || longtitude == null ) {
			throw new Exception("Parameters are null"); 
		}
		
		mapFocus = new Coordinate(lattitude, longtitude);
	}
	
	public static Coordinate getCurrentMapFocus() {
		return mapFocus; 
	}

	public static int getZoom() {
		return mapZoom;
	}

	public static void setZoom(int zoom) {
		Map.mapZoom = zoom;
	}
}
