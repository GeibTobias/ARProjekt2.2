package com.arvr.websocket;

import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.scheduling.annotation.EnableScheduling;
import org.springframework.scheduling.annotation.Scheduled;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.CrossOrigin;

import com.arvr.map.Coordinate;
import com.arvr.map.Map;
import com.arvr.utils.ListManager;

@Controller
@EnableScheduling
@CrossOrigin
public class MapUpdater {

	private final Logger log = LoggerFactory.getLogger(this.getClass());
	
	@Autowired
    private SimpMessagingTemplate template;
	
	@Autowired
	ListManager listManager; 
	
	public void updateMap() {
	
		new Thread(() -> {
		    this.sendUpdate(); 
		}).start();
	}
	
	
	@MessageMapping("/")
    @SendTo("/map/update")
	@Scheduled(fixedRate = 500)
	public void sendUpdate() {
		
		MapSettingUpdate msg = new MapSettingUpdate(Map.getCurrentMapFocus(), Map.getZoom()); 
		this.template.convertAndSend("/map/update", msg);
	}
	
	@MessageMapping("/setmap")
	public void getUpdate(MapSettingUpdate update) {
		
		log.info(String.format("Map update: %s", update));
		
		if( update != null ) {
			Map.setZoom(update.getZoom());;
			Coordinate coords = update.getCoords(); 
			try {
				Map.setFocus(coords.lattitude, coords.longtitude);
			} catch (Exception e) {
				log.error("Can#t update map focus", e); 
			}
		}
	}
	
	@SendTo("/map/route/update")
	public void sendRouteListUpdate(List<String> route) {
		
		this.template.convertAndSend("/map/route/update", route);
	}
	
	@MessageMapping("/map/route/clientupdate")
	public void routeUpdate(List<String> route) {
		
		log.info(String.format("Route update: %s", route));
		if( route == null )
			return; 
		
		this.listManager.setRoute(route);
		this.sendRouteListUpdate(this.listManager.getRouteAsList());
	}
	
	@MessageMapping("/map/route/add")
	public void addPOI(String poi) {
		log.info("Add POI to list: " + poi);
		this.listManager.addPOI(poi);
		
		onRouteUpdate();
	}
	
	@MessageMapping("/map/route/remove")
	public void removePOI(String poi) {
		log.info("Remove POI from list: " + poi);
		this.listManager.removePOI(poi);
		
		onRouteUpdate();
	}
	
	@MessageMapping("/map/route/get")
	public void getRouteList() {
		
		this.sendRouteListUpdate(listManager.getRouteAsList());
	}
	
	@MessageMapping("/test")
	@SendTo("/map/test")
	public void testSocket(String msg) {
		
		log.info("Received test message: " + msg); 
		
		this.template.convertAndSend("/map/test", "Test message");
	}
	
	
	public void onRouteUpdate() {
		
		new Thread(() -> {
			List<String> route = this.listManager.getRouteAsList(); 
			this.sendRouteListUpdate(route);
		}).start();
	}
}
