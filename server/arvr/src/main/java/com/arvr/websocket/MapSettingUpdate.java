package com.arvr.websocket;

import com.arvr.map.Coordinate;

public class MapSettingUpdate {

	private Coordinate coords;
	private int zoom; 
	
	public MapSettingUpdate() {}
	
	public MapSettingUpdate(Coordinate coord, int zoom) {
		this.coords = coord; 
		this.zoom = zoom; 
	}

	public Coordinate getCoords() {
		return coords;
	}

	public void setCoords(Coordinate coords) {
		this.coords = coords;
	}

	public int getZoom() {
		return zoom;
	}

	public void setZoom(int zoom) {
		this.zoom = zoom;
	} 
}
