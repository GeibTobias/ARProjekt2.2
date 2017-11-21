package com.arvr.utils;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class ListManager {

	private ArrayList<String> route;
	
	public ListManager() {
		route = new ArrayList<String>();
	}
	
	public void addPOI(String poi) {
		
		if( poi == null )
			throw new IllegalArgumentException(); 
		
		if( !this.route.contains(poi) )
			route.add(poi);
	}
	
	public void removePOI(String poi_id) {
		
		route.remove(poi_id); 
	}
	
	public List<String> getRouteAsList() {
		
		return route; 
	}
	
	public void setRoute(List<String> route) {
		this.route = (ArrayList<String>) route; 
	}
}
