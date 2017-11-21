package com.arvr.services;

import java.math.BigDecimal;

import javax.ws.rs.Produces;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

import com.arvr.map.Coordinate;
import com.arvr.map.Map;

@RestController
@RequestMapping(path = "/mapsync/")
public class MapSync {

	private final Logger log = LoggerFactory.getLogger(this.getClass());
	
	@RequestMapping(path = "/focus/set", method = RequestMethod.POST)
	public void setMapFocus(@RequestHeader("lattitude") String lattitude, 
			@RequestHeader("longtitude") String longtitude) throws Exception {
		
		log.info("Set new map focus");
		
		try {
			Map.setFocus(new BigDecimal(lattitude), new BigDecimal(longtitude));
		} catch (Exception e) {
			log.error("Error while setting map focus.", e);
			throw e; 
		}
	}
	
	@RequestMapping(path = "/focus/get", method = RequestMethod.GET)
	@Produces("application/json")
	public @ResponseBody Coordinate getCurrentMapFocus() {
		
		return Map.getCurrentMapFocus();
	}
	
	@RequestMapping(path = "/zoom/set", method = RequestMethod.POST)
	public void setZoom(@RequestHeader("zoom") int zoom) {
		
		log.info("Set new map zoom");
		
		Map.setZoom(zoom);
	}
	
	@RequestMapping(path = "/zoom/get", method = RequestMethod.GET)
	@Produces("application/json")
	public  @ResponseBody String getZoom() {
		
		return String.valueOf(Map.getZoom()); 
	}
}
