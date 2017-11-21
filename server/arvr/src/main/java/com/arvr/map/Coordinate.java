package com.arvr.map;

import java.math.BigDecimal;

public class Coordinate {

	public BigDecimal lat;
	public BigDecimal lng; 
	
	public Coordinate() {}
	
	public Coordinate(BigDecimal lattitude, BigDecimal longtitude) {
		if( lattitude == null || longtitude == null ) {
			throw new IllegalArgumentException(); 
		}
		
		this.lat = lattitude; 
		this.lng = longtitude; 
	}
}
