package com.arvr.services;

import java.math.BigDecimal;
import java.net.URLDecoder;

import javax.ws.rs.Produces;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.ResponseBody;
import org.springframework.web.bind.annotation.RestController;

import com.arvr.map.Coordinate;
import com.arvr.map.Map;
import com.arvr.utils.MapSeWrapper;
import com.arvr.websocket.MapSettingUpdate;
import com.arvr.websocket.MapUpdater;
import com.fasterxml.jackson.databind.ObjectMapper;

@RestController
@RequestMapping(path = "/mapsync")
public class MapSync {

	private final Logger log = LoggerFactory.getLogger(this.getClass());
	
	@Autowired
	private MapUpdater mapUpdater; 
	
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
	
	@RequestMapping(path = "/zoom/inc", method = RequestMethod.PUT)
	public  void incrementZoom() {
		
		synchronized(Map.zoomLock) {
			
			int zoom = Map.getZoom();
			if( zoom >= Map.MAX_ZOOM_VALUE )
				return; 
			
			int newZoom = zoom + 1; 
			log.info("Increment zoom value. Value: " + zoom + " -> " + newZoom);
			Map.setZoom(newZoom);
		}
		
		// publish to map
		mapUpdater.sendZoomValue();
	}
	
	@RequestMapping(path = "/zoom/dec", method = RequestMethod.PUT)
	public void decrementZoom() {
		
		synchronized(Map.zoomLock) {
			
			int zoom = Map.getZoom();
			if( zoom <= Map.MIN_ZOOM_VALUE ) 
				return; 
			
			int newZoom = zoom - 1; 
			log.info("Decrement zoom value. Value: " + zoom + " -> " + newZoom);
			Map.setZoom(newZoom);
		}
		
		// publish to map
		mapUpdater.sendZoomValue();
	}
	
	@RequestMapping(path = "/mapget", method = RequestMethod.GET)
	@Produces("application/json")
	public MapSeWrapper getMapSettings() {
		return Map.getMapSettingsWrapper(); 
	}
	
	@RequestMapping(path = "/mapset", method = RequestMethod.POST)
	public void setMapSettings(@RequestBody String json) {
		
		String data = URLDecoder.decode(json);
		
		ObjectMapper mapper = new ObjectMapper();
		MapSeWrapper settings = null;
		try {
			settings = mapper.readValue(data, MapSeWrapper.class);

		
			log.info("Map Setting Update: " + settings.lat + " : " + settings.lng + ". Zoom: " + settings.zoom);
			Map.setZoom(settings.zoom);

			Map.setFocus(new BigDecimal(settings.lat), new BigDecimal(settings.lng));
		} catch (Exception e) {
			log.error(e.getMessage());
		}
	}
	
	@RequestMapping(path = "/mapsetdelta", method = RequestMethod.POST)
	public void setMapSettingsDelta(@RequestBody String json) {
		
		String data = URLDecoder.decode(json);
		
		ObjectMapper mapper = new ObjectMapper();
		MapSeWrapper settings = null;
		try {
			settings = mapper.readValue(data, MapSeWrapper.class);

		
			log.info("Map Setting Update Delta: " + settings.lat + " : " + settings.lng + ". Zoom: " + settings.zoom);
			
			Coordinate currentCoords = Map.getMapSettings().getCoords(); 
			currentCoords.lat = currentCoords.lat.add(new BigDecimal(settings.lat));
			currentCoords.lng = currentCoords.lng.add(new BigDecimal(settings.lng));
			
			Map.setZoom(settings.zoom);
			Map.setFocus(currentCoords.lat, currentCoords.lng);
			
			mapUpdater.sendMapFocusDelta(new BigDecimal(settings.lat), new BigDecimal(settings.lng));
			
		} catch (Exception e) {
			log.error(e.getMessage());
		}
	}
}
