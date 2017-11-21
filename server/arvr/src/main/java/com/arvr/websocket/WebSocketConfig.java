package com.arvr.websocket;


import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.context.annotation.Configuration;
import org.springframework.messaging.simp.config.MessageBrokerRegistry;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.socket.config.annotation.AbstractWebSocketMessageBrokerConfigurer;
import org.springframework.web.socket.config.annotation.EnableWebSocketMessageBroker;
import org.springframework.web.socket.config.annotation.StompEndpointRegistry;

@Configuration
@EnableWebSocketMessageBroker
@CrossOrigin
public class WebSocketConfig extends AbstractWebSocketMessageBrokerConfigurer  {

	private final Logger log = LoggerFactory.getLogger(this.getClass());
	
    @Override
    public void configureMessageBroker(MessageBrokerRegistry config) {
    	log.info("Configure Message Broker");
    	
        config.enableSimpleBroker("/map");
        config.setApplicationDestinationPrefixes("/app");
    }

    @Override
    public void registerStompEndpoints(StompEndpointRegistry registry) {
    	log.info("Register WS Endpoints");
    	
        registry.addEndpoint("/ws-map-update")
        	.setAllowedOrigins("*")
        	.withSockJS();
    }
}
