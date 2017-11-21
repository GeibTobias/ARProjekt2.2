package com.arvr.services;

import javax.ws.rs.Produces;

import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping(path = "/version")
public class Version {
	
	private String version = "0.1"; 
	
	@RequestMapping(method = RequestMethod.GET)
	@Produces("text/html")
	public String getVersion() {
		
		return this.version; 
	}
}
