package com.arvr.map;

import java.math.BigDecimal;

public class Coordinate {

	public BigDecimal lattitude;
	public BigDecimal longtitude; 
	
	public Coordinate() {}
	
	public Coordinate(BigDecimal lattitude, BigDecimal longtitude) {
		if( lattitude == null || longtitude == null ) {
			throw new IllegalArgumentException(); 
		}
		
		this.lattitude = lattitude; 
		this.longtitude = longtitude; 
	}
}
