package com.arvr.services;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.lang.reflect.Array;
import java.net.URLDecoder;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

import javax.ws.rs.Produces;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import com.arvr.utils.ListManager;
import com.arvr.utils.PoiWrapper;
import com.arvr.websocket.MapUpdater;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;

@RestController
@RequestMapping(path = "/poimanager/")
public class POIManager {

	private final Logger log = LoggerFactory.getLogger(this.getClass());
	
	@Autowired
	private ListManager listManager; 
	
	@Autowired 
	private MapUpdater mapUpdater; 
	
	@RequestMapping(path = "/add/{poi_id}", method = RequestMethod.PUT)
	public void addPOI(@PathVariable String poi_id) {
		
		log.info("Add POI to list: " + poi_id);

		this.listManager.addPOI(poi_id);
		
		onMapUpdate(); 
	}
	
	@RequestMapping(path = "/add/list", method = RequestMethod.POST)
	public void addPOIList(@RequestBody String json) {
		
		log.info("Add POI list " + json);
		
		
			String data = URLDecoder.decode(json);
			
			log.info(data);
			ObjectMapper mapper = new ObjectMapper();
			PoiWrapper wrapper = null;
			try {
				wrapper = mapper.readValue(data, PoiWrapper.class);
				
				String[] tmp = Arrays.copyOf(wrapper.pois, wrapper.pois.length, String[].class); 
				ArrayList<String> alist = new ArrayList<String>(Arrays.asList(tmp)); 
				this.listManager.setRoute(alist);
				
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} 
	}
	
	@RequestMapping(path = "/completelist", method = RequestMethod.GET)
	@Produces("application/json")
	public List<String> getCompleteList() {
		
		return this.listManager.getRouteAsList(); 
	}
	
	@RequestMapping(path = "/unity/completelist", method = RequestMethod.GET)
	@Produces("application/json")
	public PoiWrapper getCompleteListUnity() {
		
		List<String> list = this.listManager.getRouteAsList(); 
		PoiWrapper wrapper = new PoiWrapper(); 
		wrapper.pois = list.toArray(); 
		
		return wrapper; 
	}
	
	@RequestMapping(path = "/remove/{poi_id}", method = RequestMethod.DELETE)
	public void remove(@PathVariable String poi_id) {
		
		log.info("Remove POI from list: " + poi_id);
		this.listManager.removePOI(poi_id);
		
		onMapUpdate(); 
	}
	
	private void onMapUpdate() {
				
		this.mapUpdater.onRouteUpdate();
	}
}
